namespace Macro;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpMain = new TableLayoutPanel();
        pnlTop = new Panel();
        lblCursor = new Label();
        lblStatus = new Label();
        tlpCenter = new TableLayoutPanel();
        lstActions = new ListView();
        colNo = new ColumnHeader();
        colAction = new ColumnHeader();
        colDelay = new ColumnHeader();
        ctxList = new ContextMenuStrip(components);
        mnuDelete = new ToolStripMenuItem();
        pnlArrows = new Panel();
        pnlButtons = new FlowLayoutPanel();
        btnCapture = new Button();
        btnRun = new Button();
        btnClear = new Button();
        btnSave = new Button();
        btnLoad = new Button();
        lblSepAdd = new Label();
        btnAddLeft = new Button();
        btnAddRight = new Button();
        btnAddKey = new Button();
        btnAddMove = new Button();
        btnAddDelay = new Button();
        pnlOpts = new FlowLayoutPanel();
        lblRepeat = new Label();
        nudRepeat = new NumericUpDown();
        chkDelay = new CheckBox();
        lblExecKey = new Label();
        btnSetKey = new Button();
        pnlProc = new FlowLayoutPanel();
        btnBrowse = new Button();
        lblProcPath = new Label();
        btnLaunch = new Button();
        btnKill = new Button();
        toolTip = new ToolTip(components);
        btnMoveUp = new Button();
        btnMoveDown = new Button();
        tlpMain.SuspendLayout();
        pnlTop.SuspendLayout();
        tlpCenter.SuspendLayout();
        ctxList.SuspendLayout();
        pnlArrows.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlOpts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudRepeat).BeginInit();
        pnlProc.SuspendLayout();
        SuspendLayout();
        // 
        // tlpMain
        // 
        tlpMain.ColumnCount = 1;
        tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpMain.Controls.Add(pnlTop, 0, 0);
        tlpMain.Controls.Add(tlpCenter, 0, 1);
        tlpMain.Controls.Add(pnlOpts, 0, 2);
        tlpMain.Controls.Add(pnlProc, 0, 3);
        tlpMain.Dock = DockStyle.Fill;
        tlpMain.Location = new Point(0, 0);
        tlpMain.Margin = new Padding(0);
        tlpMain.Name = "tlpMain";
        tlpMain.RowCount = 4;
        tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tlpMain.Size = new Size(544, 514);
        tlpMain.TabIndex = 0;
        // 
        // pnlTop
        // 
        pnlTop.BackColor = Color.FromArgb(245, 245, 248);
        pnlTop.Controls.Add(lblCursor);
        pnlTop.Controls.Add(lblStatus);
        pnlTop.Dock = DockStyle.Fill;
        pnlTop.Location = new Point(0, 0);
        pnlTop.Margin = new Padding(0);
        pnlTop.Name = "pnlTop";
        pnlTop.Size = new Size(544, 32);
        pnlTop.TabIndex = 0;
        // 
        // lblCursor
        // 
        lblCursor.Dock = DockStyle.Right;
        lblCursor.Font = new Font("Consolas", 8.5F);
        lblCursor.ForeColor = Color.Silver;
        lblCursor.Location = new Point(434, 0);
        lblCursor.Name = "lblCursor";
        lblCursor.Padding = new Padding(0, 0, 10, 0);
        lblCursor.Size = new Size(110, 32);
        lblCursor.TabIndex = 1;
        lblCursor.Text = "(0, 0)";
        lblCursor.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblStatus
        // 
        lblStatus.Dock = DockStyle.Left;
        lblStatus.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        lblStatus.ForeColor = Color.FromArgb(50, 50, 60);
        lblStatus.Location = new Point(0, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(10, 0, 0, 0);
        lblStatus.Size = new Size(400, 32);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "대기";
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // tlpCenter
        // 
        tlpCenter.ColumnCount = 3;
        tlpCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));
        tlpCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));
        tlpCenter.Controls.Add(lstActions, 0, 0);
        tlpCenter.Controls.Add(pnlArrows, 1, 0);
        tlpCenter.Controls.Add(pnlButtons, 2, 0);
        tlpCenter.Dock = DockStyle.Fill;
        tlpCenter.Location = new Point(0, 32);
        tlpCenter.Margin = new Padding(0);
        tlpCenter.Name = "tlpCenter";
        tlpCenter.RowCount = 1;
        tlpCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpCenter.Size = new Size(544, 406);
        tlpCenter.TabIndex = 1;
        // 
        // lstActions
        // 
        lstActions.BackColor = Color.White;
        lstActions.BorderStyle = BorderStyle.None;
        lstActions.Columns.AddRange(new ColumnHeader[] { colNo, colAction, colDelay });
        lstActions.ContextMenuStrip = ctxList;
        lstActions.Dock = DockStyle.Fill;
        lstActions.Font = new Font("맑은 고딕", 9F);
        lstActions.FullRowSelect = true;
        lstActions.GridLines = true;
        lstActions.Location = new Point(0, 0);
        lstActions.Margin = new Padding(0);
        lstActions.Name = "lstActions";
        lstActions.Size = new Size(362, 406);
        lstActions.TabIndex = 0;
        lstActions.UseCompatibleStateImageBehavior = false;
        lstActions.View = View.Details;
        // 
        // colNo
        // 
        colNo.Text = "No.";
        colNo.TextAlign = HorizontalAlignment.Center;
        colNo.Width = 44;
        // 
        // colAction
        // 
        colAction.Text = "동작";
        colAction.Width = 230;
        // 
        // colDelay
        // 
        colDelay.Text = "딜레이(ms)";
        colDelay.TextAlign = HorizontalAlignment.Right;
        colDelay.Width = 80;
        // 
        // ctxList
        // 
        ctxList.Items.AddRange(new ToolStripItem[] { mnuDelete });
        ctxList.Name = "ctxList";
        ctxList.Size = new Size(129, 26);
        // 
        // mnuDelete
        // 
        mnuDelete.Name = "mnuDelete";
        mnuDelete.Size = new Size(128, 22);
        mnuDelete.Text = "삭제 (Del)";
        // 
        // pnlArrows
        // 
        pnlArrows.BackColor = Color.FromArgb(238, 238, 244);
        pnlArrows.Controls.Add(btnMoveUp);
        pnlArrows.Controls.Add(btnMoveDown);
        pnlArrows.Dock = DockStyle.Fill;
        pnlArrows.Location = new Point(362, 0);
        pnlArrows.Margin = new Padding(0);
        pnlArrows.Name = "pnlArrows";
        pnlArrows.Size = new Size(34, 406);
        pnlArrows.TabIndex = 1;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(242, 243, 248);
        pnlButtons.Controls.Add(btnCapture);
        pnlButtons.Controls.Add(btnRun);
        pnlButtons.Controls.Add(btnClear);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnLoad);
        pnlButtons.Controls.Add(lblSepAdd);
        pnlButtons.Controls.Add(btnAddLeft);
        pnlButtons.Controls.Add(btnAddRight);
        pnlButtons.Controls.Add(btnAddKey);
        pnlButtons.Controls.Add(btnAddMove);
        pnlButtons.Controls.Add(btnAddDelay);
        pnlButtons.Dock = DockStyle.Fill;
        pnlButtons.FlowDirection = FlowDirection.TopDown;
        pnlButtons.Location = new Point(396, 0);
        pnlButtons.Margin = new Padding(0);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(8, 8, 8, 4);
        pnlButtons.Size = new Size(148, 406);
        pnlButtons.TabIndex = 2;
        pnlButtons.WrapContents = false;
        // 
        // btnCapture
        // 
        btnCapture.BackColor = Color.FromArgb(190, 45, 45);
        btnCapture.Cursor = Cursors.Hand;
        btnCapture.FlatStyle = FlatStyle.Flat;
        btnCapture.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnCapture.ForeColor = Color.White;
        btnCapture.Location = new Point(8, 8);
        btnCapture.Margin = new Padding(0, 0, 0, 4);
        btnCapture.Name = "btnCapture";
        btnCapture.Size = new Size(132, 30);
        btnCapture.TabIndex = 0;
        btnCapture.Text = "● 기록";
        btnCapture.UseVisualStyleBackColor = false;
        // 
        // btnRun
        // 
        btnRun.BackColor = Color.FromArgb(35, 125, 60);
        btnRun.Cursor = Cursors.Hand;
        btnRun.FlatStyle = FlatStyle.Flat;
        btnRun.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnRun.ForeColor = Color.White;
        btnRun.Location = new Point(8, 42);
        btnRun.Margin = new Padding(0, 0, 0, 4);
        btnRun.Name = "btnRun";
        btnRun.Size = new Size(132, 30);
        btnRun.TabIndex = 1;
        btnRun.Text = "▶ 실행";
        btnRun.UseVisualStyleBackColor = false;
        // 
        // btnClear
        // 
        btnClear.BackColor = Color.FromArgb(110, 110, 120);
        btnClear.Cursor = Cursors.Hand;
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnClear.ForeColor = Color.White;
        btnClear.Location = new Point(8, 76);
        btnClear.Margin = new Padding(0, 0, 0, 4);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(132, 30);
        btnClear.TabIndex = 2;
        btnClear.Text = "초기화";
        btnClear.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(50, 90, 175);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(8, 110);
        btnSave.Margin = new Padding(0, 0, 0, 4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(132, 30);
        btnSave.TabIndex = 3;
        btnSave.Text = "저장";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnLoad
        // 
        btnLoad.BackColor = Color.FromArgb(50, 90, 175);
        btnLoad.Cursor = Cursors.Hand;
        btnLoad.FlatStyle = FlatStyle.Flat;
        btnLoad.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnLoad.ForeColor = Color.White;
        btnLoad.Location = new Point(8, 144);
        btnLoad.Margin = new Padding(0, 0, 0, 4);
        btnLoad.Name = "btnLoad";
        btnLoad.Size = new Size(132, 30);
        btnLoad.TabIndex = 4;
        btnLoad.Text = "불러오기";
        btnLoad.UseVisualStyleBackColor = false;
        // 
        // lblSepAdd
        // 
        lblSepAdd.Font = new Font("맑은 고딕", 7.5F);
        lblSepAdd.ForeColor = Color.FromArgb(140, 140, 155);
        lblSepAdd.Location = new Point(8, 182);
        lblSepAdd.Margin = new Padding(0, 4, 0, 2);
        lblSepAdd.Name = "lblSepAdd";
        lblSepAdd.Size = new Size(132, 20);
        lblSepAdd.TabIndex = 5;
        lblSepAdd.Text = "─ 수동 추가 ─";
        lblSepAdd.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnAddLeft
        // 
        btnAddLeft.BackColor = Color.FromArgb(55, 90, 160);
        btnAddLeft.Cursor = Cursors.Hand;
        btnAddLeft.FlatStyle = FlatStyle.Flat;
        btnAddLeft.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnAddLeft.ForeColor = Color.White;
        btnAddLeft.Location = new Point(8, 204);
        btnAddLeft.Margin = new Padding(0, 0, 0, 4);
        btnAddLeft.Name = "btnAddLeft";
        btnAddLeft.Size = new Size(132, 30);
        btnAddLeft.TabIndex = 6;
        btnAddLeft.Text = "+좌클릭";
        btnAddLeft.UseVisualStyleBackColor = false;
        // 
        // btnAddRight
        // 
        btnAddRight.BackColor = Color.FromArgb(55, 90, 160);
        btnAddRight.Cursor = Cursors.Hand;
        btnAddRight.FlatStyle = FlatStyle.Flat;
        btnAddRight.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnAddRight.ForeColor = Color.White;
        btnAddRight.Location = new Point(8, 238);
        btnAddRight.Margin = new Padding(0, 0, 0, 4);
        btnAddRight.Name = "btnAddRight";
        btnAddRight.Size = new Size(132, 30);
        btnAddRight.TabIndex = 7;
        btnAddRight.Text = "+우클릭";
        btnAddRight.UseVisualStyleBackColor = false;
        // 
        // btnAddKey
        // 
        btnAddKey.BackColor = Color.FromArgb(80, 55, 150);
        btnAddKey.Cursor = Cursors.Hand;
        btnAddKey.FlatStyle = FlatStyle.Flat;
        btnAddKey.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnAddKey.ForeColor = Color.White;
        btnAddKey.Location = new Point(8, 272);
        btnAddKey.Margin = new Padding(0, 0, 0, 4);
        btnAddKey.Name = "btnAddKey";
        btnAddKey.Size = new Size(132, 30);
        btnAddKey.TabIndex = 8;
        btnAddKey.Text = "+키입력";
        btnAddKey.UseVisualStyleBackColor = false;
        // 
        // btnAddMove
        // 
        btnAddMove.BackColor = Color.FromArgb(25, 110, 110);
        btnAddMove.Cursor = Cursors.Hand;
        btnAddMove.FlatStyle = FlatStyle.Flat;
        btnAddMove.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnAddMove.ForeColor = Color.White;
        btnAddMove.Location = new Point(8, 306);
        btnAddMove.Margin = new Padding(0, 0, 0, 4);
        btnAddMove.Name = "btnAddMove";
        btnAddMove.Size = new Size(132, 30);
        btnAddMove.TabIndex = 9;
        btnAddMove.Text = "+이동";
        btnAddMove.UseVisualStyleBackColor = false;
        // 
        // btnAddDelay
        // 
        btnAddDelay.BackColor = Color.FromArgb(100, 100, 110);
        btnAddDelay.Cursor = Cursors.Hand;
        btnAddDelay.FlatStyle = FlatStyle.Flat;
        btnAddDelay.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnAddDelay.ForeColor = Color.White;
        btnAddDelay.Location = new Point(8, 340);
        btnAddDelay.Margin = new Padding(0, 0, 0, 4);
        btnAddDelay.Name = "btnAddDelay";
        btnAddDelay.Size = new Size(132, 30);
        btnAddDelay.TabIndex = 10;
        btnAddDelay.Text = "+지연";
        btnAddDelay.UseVisualStyleBackColor = false;
        // 
        // pnlOpts
        // 
        pnlOpts.BackColor = Color.FromArgb(245, 245, 248);
        pnlOpts.Controls.Add(lblRepeat);
        pnlOpts.Controls.Add(nudRepeat);
        pnlOpts.Controls.Add(chkDelay);
        pnlOpts.Controls.Add(lblExecKey);
        pnlOpts.Controls.Add(btnSetKey);
        pnlOpts.Dock = DockStyle.Fill;
        pnlOpts.Location = new Point(0, 438);
        pnlOpts.Margin = new Padding(0);
        pnlOpts.Name = "pnlOpts";
        pnlOpts.Padding = new Padding(10, 0, 0, 0);
        pnlOpts.Size = new Size(544, 38);
        pnlOpts.TabIndex = 2;
        // 
        // lblRepeat
        // 
        lblRepeat.AutoSize = true;
        lblRepeat.ForeColor = Color.FromArgb(50, 50, 60);
        lblRepeat.Location = new Point(10, 10);
        lblRepeat.Margin = new Padding(0, 10, 4, 0);
        lblRepeat.Name = "lblRepeat";
        lblRepeat.Size = new Size(92, 17);
        lblRepeat.TabIndex = 0;
        lblRepeat.Text = "반복 (0=무한):";
        // 
        // nudRepeat
        // 
        nudRepeat.Location = new Point(106, 7);
        nudRepeat.Margin = new Padding(0, 7, 16, 0);
        nudRepeat.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        nudRepeat.Name = "nudRepeat";
        nudRepeat.Size = new Size(56, 24);
        nudRepeat.TabIndex = 1;
        nudRepeat.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // chkDelay
        // 
        chkDelay.AutoSize = true;
        chkDelay.Checked = true;
        chkDelay.CheckState = CheckState.Checked;
        chkDelay.ForeColor = Color.FromArgb(50, 50, 60);
        chkDelay.Location = new Point(178, 10);
        chkDelay.Margin = new Padding(0, 10, 20, 0);
        chkDelay.Name = "chkDelay";
        chkDelay.Size = new Size(97, 21);
        chkDelay.TabIndex = 2;
        chkDelay.Text = "딜레이 적용";
        // 
        // lblExecKey
        // 
        lblExecKey.AutoSize = true;
        lblExecKey.ForeColor = Color.FromArgb(50, 50, 60);
        lblExecKey.Location = new Point(295, 10);
        lblExecKey.Margin = new Padding(0, 10, 6, 0);
        lblExecKey.Name = "lblExecKey";
        lblExecKey.Size = new Size(50, 17);
        lblExecKey.TabIndex = 3;
        lblExecKey.Text = "실행키:";
        // 
        // btnSetKey
        // 
        btnSetKey.BackColor = Color.FromArgb(70, 70, 160);
        btnSetKey.Cursor = Cursors.Hand;
        btnSetKey.FlatStyle = FlatStyle.Flat;
        btnSetKey.Font = new Font("맑은 고딕", 8.5F);
        btnSetKey.ForeColor = Color.White;
        btnSetKey.Location = new Point(351, 7);
        btnSetKey.Margin = new Padding(0, 7, 0, 0);
        btnSetKey.Name = "btnSetKey";
        btnSetKey.Size = new Size(80, 24);
        btnSetKey.TabIndex = 4;
        btnSetKey.Text = "없음";
        toolTip.SetToolTip(btnSetKey, "클릭 후 키를 누르면 실행 키로 설정됩니다.");
        btnSetKey.UseVisualStyleBackColor = false;
        // 
        // pnlProc
        // 
        pnlProc.BackColor = Color.FromArgb(238, 238, 244);
        pnlProc.Controls.Add(btnBrowse);
        pnlProc.Controls.Add(lblProcPath);
        pnlProc.Controls.Add(btnLaunch);
        pnlProc.Controls.Add(btnKill);
        pnlProc.Dock = DockStyle.Fill;
        pnlProc.Location = new Point(0, 476);
        pnlProc.Margin = new Padding(0);
        pnlProc.Name = "pnlProc";
        pnlProc.Padding = new Padding(10, 0, 8, 0);
        pnlProc.Size = new Size(544, 38);
        pnlProc.TabIndex = 3;
        // 
        // btnBrowse
        // 
        btnBrowse.BackColor = Color.FromArgb(90, 90, 100);
        btnBrowse.Cursor = Cursors.Hand;
        btnBrowse.FlatStyle = FlatStyle.Flat;
        btnBrowse.Font = new Font("맑은 고딕", 9F);
        btnBrowse.ForeColor = Color.White;
        btnBrowse.Location = new Point(10, 6);
        btnBrowse.Margin = new Padding(0, 6, 8, 0);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(72, 26);
        btnBrowse.TabIndex = 0;
        btnBrowse.Text = "찾아보기";
        btnBrowse.UseVisualStyleBackColor = false;
        // 
        // lblProcPath
        // 
        lblProcPath.BackColor = Color.White;
        lblProcPath.BorderStyle = BorderStyle.FixedSingle;
        lblProcPath.Font = new Font("맑은 고딕", 8.5F);
        lblProcPath.ForeColor = Color.Gray;
        lblProcPath.Location = new Point(90, 6);
        lblProcPath.Margin = new Padding(0, 6, 8, 0);
        lblProcPath.Name = "lblProcPath";
        lblProcPath.Padding = new Padding(4, 0, 0, 0);
        lblProcPath.Size = new Size(248, 26);
        lblProcPath.TabIndex = 1;
        lblProcPath.Text = "선택된 파일 없음";
        lblProcPath.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnLaunch
        // 
        btnLaunch.BackColor = Color.FromArgb(35, 125, 60);
        btnLaunch.Cursor = Cursors.Hand;
        btnLaunch.Enabled = false;
        btnLaunch.FlatStyle = FlatStyle.Flat;
        btnLaunch.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnLaunch.ForeColor = Color.White;
        btnLaunch.Location = new Point(346, 6);
        btnLaunch.Margin = new Padding(0, 6, 4, 0);
        btnLaunch.Name = "btnLaunch";
        btnLaunch.Size = new Size(90, 26);
        btnLaunch.TabIndex = 2;
        btnLaunch.Text = "+실행 추가";
        btnLaunch.UseVisualStyleBackColor = false;
        // 
        // btnKill
        // 
        btnKill.BackColor = Color.FromArgb(190, 45, 45);
        btnKill.Cursor = Cursors.Hand;
        btnKill.Enabled = true;
        btnKill.FlatStyle = FlatStyle.Flat;
        btnKill.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        btnKill.ForeColor = Color.White;
        btnKill.Location = new Point(440, 6);
        btnKill.Margin = new Padding(0, 6, 0, 0);
        btnKill.Name = "btnKill";
        btnKill.Size = new Size(90, 26);
        btnKill.TabIndex = 3;
        btnKill.Text = "+종료 추가";
        btnKill.UseVisualStyleBackColor = false;
        // 
        // btnMoveUp
        // 
        btnMoveUp.BackColor = Color.FromArgb(210, 212, 220);
        btnMoveUp.Cursor = Cursors.Hand;
        btnMoveUp.FlatAppearance.BorderColor = Color.FromArgb(190, 192, 200);
        btnMoveUp.FlatStyle = FlatStyle.Flat;
        btnMoveUp.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        btnMoveUp.ForeColor = Color.FromArgb(50, 50, 70);
        btnMoveUp.Location = new Point(3, 3);
        btnMoveUp.Name = "btnMoveUp";
        btnMoveUp.Size = new Size(31, 69);
        btnMoveUp.TabIndex = 2;
        btnMoveUp.Text = "▲";
        btnMoveUp.UseVisualStyleBackColor = false;
        // 
        // btnMoveDown
        // 
        btnMoveDown.BackColor = Color.FromArgb(210, 212, 220);
        btnMoveDown.Cursor = Cursors.Hand;
        btnMoveDown.FlatAppearance.BorderColor = Color.FromArgb(190, 192, 200);
        btnMoveDown.FlatStyle = FlatStyle.Flat;
        btnMoveDown.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        btnMoveDown.ForeColor = Color.FromArgb(50, 50, 70);
        btnMoveDown.Location = new Point(3, 76);
        btnMoveDown.Name = "btnMoveDown";
        btnMoveDown.Size = new Size(31, 78);
        btnMoveDown.TabIndex = 3;
        btnMoveDown.Text = "▼";
        btnMoveDown.UseVisualStyleBackColor = false;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(544, 514);
        Controls.Add(tlpMain);
        Font = new Font("맑은 고딕", 9.5F);
        MinimumSize = new Size(440, 430);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "매크로";
        tlpMain.ResumeLayout(false);
        pnlTop.ResumeLayout(false);
        tlpCenter.ResumeLayout(false);
        ctxList.ResumeLayout(false);
        pnlArrows.ResumeLayout(false);
        pnlButtons.ResumeLayout(false);
        pnlOpts.ResumeLayout(false);
        pnlOpts.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudRepeat).EndInit();
        pnlProc.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel  tlpMain;
    private System.Windows.Forms.Panel             pnlTop;
    private System.Windows.Forms.Label             lblStatus;
    private System.Windows.Forms.Label             lblCursor;
    private System.Windows.Forms.TableLayoutPanel  tlpCenter;
    private System.Windows.Forms.ListView          lstActions;
    private System.Windows.Forms.ColumnHeader      colNo;
    private System.Windows.Forms.ColumnHeader      colAction;
    private System.Windows.Forms.ColumnHeader      colDelay;
    private System.Windows.Forms.Panel             pnlArrows;
    private System.Windows.Forms.FlowLayoutPanel   pnlButtons;
    private System.Windows.Forms.Button            btnCapture;
    private System.Windows.Forms.Button            btnRun;
    private System.Windows.Forms.Button            btnClear;
    private System.Windows.Forms.Button            btnSave;
    private System.Windows.Forms.Button            btnLoad;
    private System.Windows.Forms.Label             lblSepAdd;
    private System.Windows.Forms.Button            btnAddLeft;
    private System.Windows.Forms.Button            btnAddRight;
    private System.Windows.Forms.Button            btnAddKey;
    private System.Windows.Forms.Button            btnAddMove;
    private System.Windows.Forms.Button            btnAddDelay;
    private System.Windows.Forms.FlowLayoutPanel   pnlOpts;
    private System.Windows.Forms.Label             lblRepeat;
    private System.Windows.Forms.NumericUpDown     nudRepeat;
    private System.Windows.Forms.CheckBox          chkDelay;
    private System.Windows.Forms.Label             lblExecKey;
    private System.Windows.Forms.Button            btnSetKey;
    private System.Windows.Forms.FlowLayoutPanel   pnlProc;
    private System.Windows.Forms.Button            btnBrowse;
    private System.Windows.Forms.Label             lblProcPath;
    private System.Windows.Forms.Button            btnLaunch;
    private System.Windows.Forms.Button            btnKill;
    private System.Windows.Forms.ContextMenuStrip  ctxList;
    private System.Windows.Forms.ToolStripMenuItem mnuDelete;
    private System.Windows.Forms.ToolTip           toolTip;
    private Button btnMoveUp;
    private Button btnMoveDown;
}
