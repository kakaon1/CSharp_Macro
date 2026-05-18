using Macro.Core;
using Macro.Models;
using System.ComponentModel;
using System.Drawing;

namespace Macro;

public sealed partial class MainForm : Form
{
    private readonly List<MacroAction>  _actions = new();
    private readonly GlobalHook         _hook = null!;
    private readonly MacroRecorder      _recorder = null!;
    private readonly MacroPlayer        _player = null!;
    private readonly System.Windows.Forms.Timer _cursorTimer = null!;

    private bool  _isCapturing;
    private Keys  _execKey    = Keys.None;
    private bool  _waitingKey = false;
    private bool  _waitingAdd = false;
    private bool  _loading    = false;
    private bool  _runtimeInitialized;
    private Rectangle _macroScreenBounds;

    private string? _procPath;

    public MainForm()
    {
        InitializeComponent();

        if (IsDesignTime())
            return;

        WireEvents();
        _macroScreenBounds = NativeMethods.GetVirtualScreenBounds();

        _hook     = new GlobalHook();
        _recorder = new MacroRecorder(_hook, _actions, this);
        _player   = new MacroPlayer();

        _hook.KeyDown            += OnHotkey;
        _recorder.ActionAdded    += OnActionAdded;
        _player.PlaybackFinished += (_, _) => SafeInvoke(OnRunFinished);

        _hook.Install();
        LoadSettings();

        _cursorTimer = new System.Windows.Forms.Timer { Interval = 80 };
        _cursorTimer.Tick += (_, _) =>
        {
            NativeMethods.GetCursorPos(out var p);
            lblCursor.Text = $"({p.X}, {p.Y})";
        };
        _cursorTimer.Start();
        _runtimeInitialized = true;
    }

    private static bool IsDesignTime() =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
        AppDomain.CurrentDomain.FriendlyName.Contains("DesignToolsServer", StringComparison.OrdinalIgnoreCase);

    private void WireEvents()
    {
        btnCapture.Click  += (_, _) => ToggleCapture();
        btnRun.Click      += (_, _) => ToggleRun();
        btnClear.Click    += (_, _) => ClearList();
        btnSave.Click     += (_, _) => SaveAs();
        btnLoad.Click     += (_, _) => LoadFrom();

        btnAddLeft.Click   += (_, _) => AddClickNow(ActionType.MouseLeftClick);
        btnAddRight.Click  += (_, _) => AddClickNow(ActionType.MouseRightClick);
        btnAddKey.Click    += (_, _) => StartKeyAdd();
        btnAddMove.Click   += (_, _) => ShowMoveDialog();
        btnAddDelay.Click  += (_, _) => ShowDelayDialog();

        btnSetKey.Click  += (_, _) => StartKeySet();

        btnBrowse.Click  += (_, _) => BrowseProc();
        btnLaunch.Click  += (_, _) => AddProcLaunch();
        btnKill.Click    += (_, _) => ShowKillDialog();

        btnMoveUp.Click   += (_, _) => MoveSelectedUp();
        btnMoveDown.Click += (_, _) => MoveSelectedDown();
        pnlArrows.Resize  += (_, _) => CenterArrowButtons();

        mnuDelete.Click    += (_, _) => DeleteSelected();
        lstActions.KeyDown += (_, e) => { if (e.KeyCode == Keys.Delete) DeleteSelected(); };

        nudRepeat.ValueChanged  += (_, _) => SaveSettings();
        chkDelay.CheckedChanged += (_, _) => SaveSettings();
    }

    // ── 설정 ──────────────────────────────────────────────────

    private void LoadSettings()
    {
        _loading = true;
        List<MacroAction> loaded;
        bool needsScreenSave = false;
        try
        {
            var (actions, settings) = MacroStorage.Load();
            loaded           = actions;
            _execKey         = settings.ExecuteKey;
            nudRepeat.Value  = Math.Clamp(settings.RepeatCount, (int)nudRepeat.Minimum, (int)nudRepeat.Maximum);
            chkDelay.Checked = settings.UseDelay;
            if (settings.ScreenWidth > 1 && settings.ScreenHeight > 1)
            {
                _macroScreenBounds = new Rectangle(settings.ScreenLeft, settings.ScreenTop, settings.ScreenWidth, settings.ScreenHeight);
            }
            else
            {
                _macroScreenBounds = NativeMethods.GetVirtualScreenBounds();
                needsScreenSave = true;
            }
            UpdateExecKeyBtn();
        }
        finally
        {
            _loading = false;
        }

        if (loaded.Count > 0)
        {
            _actions.AddRange(loaded);
            RefreshList();
        }

        if (needsScreenSave)
            SaveSettings();
    }

    private void SaveSettings()
    {
        if (_loading) return;
        try
        {
            MacroStorage.Save(_actions, CreateSettings());
        }
        catch { }
    }

    private MacroSettings CreateSettings() => new()
    {
        ExecuteKey   = _execKey,
        RepeatCount  = (int)nudRepeat.Value,
        UseDelay     = chkDelay.Checked,
        ScreenLeft   = _macroScreenBounds.Left,
        ScreenTop    = _macroScreenBounds.Top,
        ScreenWidth  = _macroScreenBounds.Width,
        ScreenHeight = _macroScreenBounds.Height
    };

    private void UpdateExecKeyBtn() =>
        btnSetKey.Text = _execKey == Keys.None ? "없음" : _execKey.ToString();

    // ── 전역 핫키 ──────────────────────────────────────────────

    private void OnHotkey(object? sender, KeyHookArgs e)
    {
        // ESC로 대기 상태 취소
        if (e.Key == Keys.Escape)
        {
            if (_waitingKey || _waitingAdd)
            {
                e.Suppress = true;
                SafeInvoke(CancelWaiting);
                return;
            }
        }

        if (_waitingKey)
        {
            if (e.Key is Keys.F6 or Keys.F7 or Keys.LControlKey or Keys.RControlKey
                      or Keys.LShiftKey or Keys.RShiftKey or Keys.LMenu or Keys.RMenu)
                return;

            e.Suppress  = true;
            _waitingKey = false;
            _execKey    = e.Key;
            SafeInvoke(() =>
            {
                UpdateExecKeyBtn();
                SaveSettings();
                SetStatus("대기", Color.FromArgb(50, 50, 60));
            });
            return;
        }

        if (_waitingAdd)
        {
            if (e.Key is Keys.F6 or Keys.F7) return;
            e.Suppress  = true;
            _waitingAdd = false;
            SafeInvoke(() =>
            {
                btnAddKey.Text = "+키입력";
                AddAction(new MacroAction
                {
                    Type    = ActionType.KeyPress,
                    VkCode  = (int)e.Key,
                    KeyName = e.Key.ToString(),
                    DelayMs = 0
                });
                SaveSettings();
            });
            return;
        }

        switch (e.Key)
        {
            case Keys.F6:
                e.Suppress = true;
                SafeInvoke(ToggleCapture);
                break;
            case Keys.F7:
                e.Suppress = true;
                if (!_isCapturing)
                {
                    var pos = Cursor.Position;
                    SafeInvoke(() =>
                    {
                        AddAction(new MacroAction { Type = ActionType.MouseMove, X = pos.X, Y = pos.Y, DelayMs = 0 });
                        SaveSettings();
                    });
                }
                break;
            default:
                if (e.Key == _execKey && _execKey != Keys.None)
                {
                    e.Suppress = true;
                    SafeInvoke(ToggleRun);
                }
                break;
        }
    }

    private void CancelWaiting()
    {
        if (_waitingKey)
        {
            _waitingKey    = false;
            UpdateExecKeyBtn();
            SetStatus("대기", Color.FromArgb(50, 50, 60));
        }
        if (_waitingAdd)
        {
            _waitingAdd    = false;
            btnAddKey.Text = "+키입력";
        }
    }

    // ── 기록 / 실행 ───────────────────────────────────────────

    private void ToggleCapture()
    {
        if (!_runtimeInitialized) return;
        if (_player.IsPlaying) return;
        CancelWaiting();

        if (!_isCapturing)
        {
            _isCapturing    = true;
            _recorder.Start();
            SetStatus("● 기록 중  —  F6: 중지 / F7: 위치 기록", Color.FromArgb(180, 40, 40));
            btnCapture.Text = "■ 중지 (F6)";
        }
        else
        {
            _isCapturing    = false;
            _recorder.Stop();
            SetStatus("대기", Color.FromArgb(50, 50, 60));
            btnCapture.Text = "● 기록";
            SaveSettings();
        }
    }

    private void ToggleRun()
    {
        if (!_runtimeInitialized) return;
        if (_isCapturing || _actions.Count == 0) return;

        if (!_player.IsPlaying)
        {
            var list = chkDelay.Checked
                ? _actions.ToList()
                : _actions.Select(a => new MacroAction
                  { Type = a.Type, X = a.X, Y = a.Y, VkCode = a.VkCode, KeyName = a.KeyName,
                    FilePath = a.FilePath, ProcessName = a.ProcessName, DelayMs = 30 }).ToList();

            int repeat = (int)nudRepeat.Value;
            _player.Start(list, repeat, _macroScreenBounds);
            string repeatLabel = repeat == 0 ? "무한 반복" : $"{repeat}회 반복";
            string keyLabel    = _execKey == Keys.None ? "" : $"  ({_execKey}: 중지)";
            SetStatus($"▶ 실행 중  —  {repeatLabel}{keyLabel}", Color.FromArgb(30, 110, 55));
            btnRun.Text = "■ 중지";
        }
        else
        {
            _player.Stop();
        }
    }

    private void OnRunFinished()
    {
        SetStatus("대기", Color.FromArgb(50, 50, 60));
        btnRun.Text = "▶ 실행";
    }

    // ── 실행키 설정 ───────────────────────────────────────────

    private void StartKeySet()
    {
        if (_isCapturing || _player.IsPlaying) return;
        CancelWaiting();
        _waitingKey    = true;
        btnSetKey.Text = "대기...";
        SetStatus("실행 키로 사용할 키를 누르세요.  (ESC: 취소)", Color.FromArgb(60, 60, 130));
    }

    // ── 수동 추가 ─────────────────────────────────────────────

    private void AddClickNow(ActionType type)
    {
        if (_player.IsPlaying) return;
        CancelWaiting();
        NativeMethods.GetCursorPos(out var pos);
        AddAction(new MacroAction { Type = type, X = pos.X, Y = pos.Y, DelayMs = 0 });
        SaveSettings();
    }

    private void StartKeyAdd()
    {
        if (_player.IsPlaying) return;
        CancelWaiting();
        _waitingAdd    = true;
        btnAddKey.Text = "대기... (ESC취소)";
    }

    private void ShowMoveDialog()
    {
        if (_player.IsPlaying) return;
        CancelWaiting();
        NativeMethods.GetCursorPos(out var cur);

        using var dlg = new Form
        {
            Text            = "마우스 이동 추가",
            Size            = new Size(280, 160),
            StartPosition   = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox     = false,
            MinimizeBox     = false,
            Font            = new Font("맑은 고딕", 9.5f)
        };

        var lblX      = new Label   { Text = "X:", Left = 16,  Top = 16, Width = 20 };
        var txtX      = new TextBox { Left = 40,  Top = 12, Width = 80, Text = cur.X.ToString() };
        var lblY      = new Label   { Text = "Y:", Left = 140, Top = 16, Width = 20 };
        var txtY      = new TextBox { Left = 164, Top = 12, Width = 80, Text = cur.Y.ToString() };
        var lblMs     = new Label   { Text = "딜레이(ms):", Left = 16, Top = 48, Width = 80 };
        var txtMs     = new TextBox { Left = 100, Top = 44, Width = 60, Text = "0" };
        var lblHint   = new Label   { Text = "※ F7: 현재 커서 위치 즉시 추가", Left = 16, Top = 78, Width = 220, ForeColor = Color.Gray, Font = new Font("맑은 고딕", 8F) };
        var btnOk     = new Button  { Text = "추가", Left = 140, Top = 76, Width = 54, Height = 26, DialogResult = DialogResult.OK };
        var btnCancel = new Button  { Text = "취소", Left = 198, Top = 76, Width = 54, Height = 26, DialogResult = DialogResult.Cancel };

        dlg.Controls.AddRange(new Control[] { lblX, txtX, lblY, txtY, lblMs, txtMs, lblHint, btnOk, btnCancel });
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnCancel;

        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (!int.TryParse(txtX.Text, out int x) || !int.TryParse(txtY.Text, out int y)) return;
        int.TryParse(txtMs.Text, out int ms);
        AddAction(new MacroAction { Type = ActionType.MouseMove, X = x, Y = y, DelayMs = Math.Max(0, ms) });
        SaveSettings();
    }

    private void ShowDelayDialog()
    {
        if (_player.IsPlaying) return;
        CancelWaiting();

        using var dlg = new Form
        {
            Text            = "지연 추가",
            Size            = new Size(240, 130),
            StartPosition   = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox     = false,
            MinimizeBox     = false,
            Font            = new Font("맑은 고딕", 9.5f)
        };
        var lblMs = new Label   { Text = "지연 시간 (ms):", Left = 16, Top = 16, Width = 100 };
        var txtMs = new TextBox { Left = 120, Top = 12, Width = 80, Text = "1000" };
        var btnOk = new Button  { Text = "추가", Left = 68,  Top = 46, Width = 54, Height = 26, DialogResult = DialogResult.OK };
        var btnNo = new Button  { Text = "취소", Left = 126, Top = 46, Width = 54, Height = 26, DialogResult = DialogResult.Cancel };
        dlg.Controls.AddRange(new Control[] { lblMs, txtMs, btnOk, btnNo });
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnNo;

        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (!int.TryParse(txtMs.Text, out int ms) || ms < 0) return;
        AddAction(new MacroAction { Type = ActionType.Delay, DelayMs = ms });
        SaveSettings();
    }

    // ── 목록 조작 ─────────────────────────────────────────────

    private void OnActionAdded(object? sender, MacroAction action)
    {
        SafeInvoke(() =>
        {
            AppendRow(_actions.Count, action);
            lblStatus.Text = $"● 기록 중  ({_actions.Count}개)  —  F6: 중지 / F7: 위치";
        });
    }

    private void AddAction(MacroAction action)
    {
        _actions.Add(action);
        AppendRow(_actions.Count, action);
    }

    private void AppendRow(int no, MacroAction action)
    {
        var item = new ListViewItem(no.ToString());
        item.SubItems.Add(action.GetDescription());
        item.SubItems.Add(action.DelayMs.ToString());
        lstActions.Items.Add(item);
        item.EnsureVisible();
    }

    private void RefreshList()
    {
        lstActions.Items.Clear();
        for (int i = 0; i < _actions.Count; i++)
        {
            var a    = _actions[i];
            var item = new ListViewItem((i + 1).ToString());
            item.SubItems.Add(a.GetDescription());
            item.SubItems.Add(a.DelayMs.ToString());
            lstActions.Items.Add(item);
        }
        if (lstActions.Items.Count > 0) lstActions.Items[^1].EnsureVisible();
    }

    private void DeleteSelected()
    {
        if (_isCapturing || _player.IsPlaying) return;
        foreach (int i in lstActions.SelectedIndices.Cast<int>().OrderByDescending(x => x))
            _actions.RemoveAt(i);
        RefreshList();
        SaveSettings();
    }

    private void CenterArrowButtons()
    {
        int totalH = btnMoveUp.Height + btnMoveDown.Height;
        int top    = (pnlArrows.Height - totalH) / 2;
        btnMoveUp.Location   = new Point(0, Math.Max(0, top));
        btnMoveDown.Location = new Point(0, Math.Max(0, top) + btnMoveUp.Height);
    }

    private void MoveSelectedUp()
    {
        if (_isCapturing || _player.IsPlaying) return;
        var indices = lstActions.SelectedIndices.Cast<int>().OrderBy(x => x).ToList();
        if (indices.Count == 0 || indices[0] == 0) return;
        foreach (int i in indices)
        { var tmp = _actions[i - 1]; _actions[i - 1] = _actions[i]; _actions[i] = tmp; }
        RefreshList();
        foreach (int i in indices) lstActions.Items[i - 1].Selected = true;
        SaveSettings();
    }

    private void MoveSelectedDown()
    {
        if (_isCapturing || _player.IsPlaying) return;
        var indices = lstActions.SelectedIndices.Cast<int>().OrderByDescending(x => x).ToList();
        if (indices.Count == 0 || indices[0] == _actions.Count - 1) return;
        foreach (int i in indices)
        { var tmp = _actions[i + 1]; _actions[i + 1] = _actions[i]; _actions[i] = tmp; }
        RefreshList();
        foreach (int i in indices.OrderBy(x => x)) lstActions.Items[i + 1].Selected = true;
        SaveSettings();
    }

    private void ClearList()
    {
        if (_isCapturing || _player.IsPlaying || _actions.Count == 0) return;
        if (MessageBox.Show("매크로를 초기화하시겠습니까?", "확인",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _actions.Clear();
        lstActions.Items.Clear();
        _macroScreenBounds = NativeMethods.GetVirtualScreenBounds();
        SaveSettings();
    }

    private void SaveAs()
    {
        using var dlg = new SaveFileDialog { Filter = "매크로 INI (*.ini)|*.ini", DefaultExt = "ini", FileName = "macro" };
        if (dlg.ShowDialog() == DialogResult.OK)
            MacroStorage.Save(_actions, CreateSettings(), dlg.FileName);
    }

    private void LoadFrom()
    {
        using var dlg = new OpenFileDialog { Filter = "매크로 INI (*.ini)|*.ini" };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        var (actions, settings) = MacroStorage.Load(dlg.FileName);
        _actions.Clear();
        _actions.AddRange(actions);
        bool needsScreenSave = settings.ScreenWidth <= 1 || settings.ScreenHeight <= 1;
        _macroScreenBounds = needsScreenSave
            ? NativeMethods.GetVirtualScreenBounds()
            : new Rectangle(settings.ScreenLeft, settings.ScreenTop, settings.ScreenWidth, settings.ScreenHeight);

        if (needsScreenSave)
        {
            settings.ScreenLeft   = _macroScreenBounds.Left;
            settings.ScreenTop    = _macroScreenBounds.Top;
            settings.ScreenWidth  = _macroScreenBounds.Width;
            settings.ScreenHeight = _macroScreenBounds.Height;
            MacroStorage.Save(actions, settings, dlg.FileName);
        }

        RefreshList();
        SaveSettings();
    }

    // ── 프로세스 추가 ──────────────────────────────────────────

    private void BrowseProc()
    {
        using var dlg = new OpenFileDialog
        {
            Title  = "실행 파일 선택",
            Filter = "실행 파일 (*.exe;*.bat;*.cmd;*.lnk)|*.exe;*.bat;*.cmd;*.lnk|모든 파일 (*.*)|*.*"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        _procPath             = dlg.FileName;
        lblProcPath.Text      = _procPath;
        lblProcPath.ForeColor = Color.FromArgb(32, 32, 42);
        toolTip.SetToolTip(lblProcPath, _procPath);
        btnLaunch.Enabled     = true;
    }

    private void AddProcLaunch()
    {
        if (string.IsNullOrEmpty(_procPath) || _player.IsPlaying) return;
        AddAction(new MacroAction { Type = ActionType.LaunchProgram, FilePath = _procPath, DelayMs = 0 });
        SaveSettings();
    }

    private void ShowKillDialog()
    {
        if (_player.IsPlaying) return;
        CancelWaiting();

        using var dlg = new Form
        {
            Text            = "프로세스 종료 추가",
            Size            = new Size(300, 135),
            StartPosition   = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox     = false,
            MinimizeBox     = false,
            Font            = new Font("맑은 고딕", 9.5f)
        };

        var lblName   = new Label   { Text = "프로세스명:", Left = 16, Top = 18, Width = 80 };
        var txtName   = new TextBox { Left = 96, Top = 14, Width = 168 };
        var btnOk     = new Button  { Text = "추가", Left = 148, Top = 50, Width = 54, Height = 26, DialogResult = DialogResult.OK };
        var btnCancel = new Button  { Text = "취소", Left = 210, Top = 50, Width = 54, Height = 26, DialogResult = DialogResult.Cancel };

        dlg.Controls.AddRange(new Control[] { lblName, txtName, btnOk, btnCancel });
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnCancel;

        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        string processName = txtName.Text.Trim();
        if (string.IsNullOrWhiteSpace(processName)) return;

        AddAction(new MacroAction { Type = ActionType.KillProgram, ProcessName = processName, DelayMs = 0 });
        SaveSettings();
    }

    // ── 유틸 ──────────────────────────────────────────────────

    private void SetStatus(string text, Color foreColor)
    {
        lblStatus.Text      = text;
        lblStatus.ForeColor = foreColor;
    }

    private void SafeInvoke(Action action)
    {
        if (IsDisposed || !IsHandleCreated) return;
        try { if (InvokeRequired) Invoke(action); else action(); }
        catch (ObjectDisposedException) { }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_runtimeInitialized)
        {
            base.OnFormClosing(e);
            return;
        }

        _cursorTimer.Stop();
        _cursorTimer.Dispose();
        try { SaveSettings(); } catch { }
        _player.Stop();
        _recorder.Stop();
        _hook.Dispose();
        toolTip.Dispose();
        base.OnFormClosing(e);
    }
}
