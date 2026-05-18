using Macro.Models;
using System.Drawing;

namespace Macro.Core;

internal sealed class MacroPlayer
{
    private CancellationTokenSource? _cts;

    public bool IsPlaying => _cts != null && !_cts.IsCancellationRequested;

    public event EventHandler? PlaybackFinished;

    public void Start(IEnumerable<MacroAction> actions, int repeatCount, Rectangle sourceScreenBounds)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        var list       = actions.ToList();
        var token      = _cts.Token;
        var currentCts = _cts;
        Task.Run(() => RunAsync(list, repeatCount, sourceScreenBounds, token, currentCts));
    }

    public void Stop() => _cts?.Cancel();

    private async Task RunAsync(List<MacroAction> actions, int repeatCount, Rectangle sourceScreenBounds, CancellationToken token, CancellationTokenSource runCts)
    {
        try
        {
            bool infinite = repeatCount == 0;
            for (int r = 0; (infinite || r < repeatCount) && !token.IsCancellationRequested; r++)
            {
                foreach (var a in actions)
                {
                    if (token.IsCancellationRequested) break;
                    if (a.DelayMs > 0)
                        await Task.Delay(a.DelayMs, token);
                    Execute(a, sourceScreenBounds);
                }
            }
        }
        catch (TaskCanceledException) { }
        finally
        {
            bool isCurrent = ReferenceEquals(_cts, runCts);
            if (isCurrent) _cts = null;
            runCts.Dispose();
            if (isCurrent) PlaybackFinished?.Invoke(this, EventArgs.Empty);
        }
    }

    private static void Execute(MacroAction a, Rectangle sourceScreenBounds)
    {
        switch (a.Type)
        {
            case ActionType.MouseLeftClick:  Click(a, sourceScreenBounds, left: true);  break;
            case ActionType.MouseRightClick: Click(a, sourceScreenBounds, left: false); break;
            case ActionType.MouseMove:       Move(a, sourceScreenBounds);               break;
            case ActionType.KeyPress:        PressKey((ushort)a.VkCode);   break;
            case ActionType.LaunchProgram:   RunProgram(a.FilePath);        break;
            case ActionType.KillProgram:     TermProcess(a.ProcessName);    break;
        }
    }

    private static void RunProgram(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = path, UseShellExecute = true }); }
        catch { }
    }

    private static void TermProcess(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        string clean = name.Replace(".exe", "", StringComparison.OrdinalIgnoreCase);
        foreach (var p in System.Diagnostics.Process.GetProcessesByName(clean))
            try { p.Kill(entireProcessTree: true); p.Dispose(); } catch { }
    }

    private static Point ScalePoint(MacroAction a, Rectangle source)
    {
        if (source.Width <= 1 || source.Height <= 1)
            return new Point(a.X, a.Y);

        var current = NativeMethods.GetVirtualScreenBounds();
        double nx = (a.X - source.Left) / (double)(source.Width - 1);
        double ny = (a.Y - source.Top) / (double)(source.Height - 1);

        int x = current.Left + (int)Math.Round(nx * (current.Width - 1));
        int y = current.Top  + (int)Math.Round(ny * (current.Height - 1));

        return new Point(
            Math.Clamp(x, current.Left, current.Right - 1),
            Math.Clamp(y, current.Top, current.Bottom - 1));
    }

    private static readonly IntPtr MarkerPtr = unchecked((IntPtr)NativeMethods.MACRO_MARKER);

    private static void Move(MacroAction action, Rectangle sourceScreenBounds)
    {
        var point = ScalePoint(action, sourceScreenBounds);
        NativeMethods.SetCursorPos(point.X, point.Y);
    }

    private static void Click(MacroAction action, Rectangle sourceScreenBounds, bool left)
    {
        uint downFlag = left ? NativeMethods.MOUSEEVENTF_LEFTDOWN  : NativeMethods.MOUSEEVENTF_RIGHTDOWN;
        uint upFlag   = left ? NativeMethods.MOUSEEVENTF_LEFTUP    : NativeMethods.MOUSEEVENTF_RIGHTUP;

        NativeMethods.SendInput(2, new[]
        {
            new NativeMethods.INPUT { type = NativeMethods.INPUT_MOUSE, U = new() { mi = new() { dwFlags = downFlag, dwExtraInfo = MarkerPtr } } },
            new NativeMethods.INPUT { type = NativeMethods.INPUT_MOUSE, U = new() { mi = new() { dwFlags = upFlag,   dwExtraInfo = MarkerPtr } } }
        }, NativeMethods.INPUT.Size);
    }

    private static void PressKey(ushort vk)
    {
        NativeMethods.SendInput(2, new[]
        {
            new NativeMethods.INPUT { type = NativeMethods.INPUT_KEYBOARD, U = new() { ki = new() { wVk = vk, dwExtraInfo = MarkerPtr } } },
            new NativeMethods.INPUT { type = NativeMethods.INPUT_KEYBOARD, U = new() { ki = new() { wVk = vk, dwFlags = NativeMethods.KEYEVENTF_KEYUP, dwExtraInfo = MarkerPtr } } }
        }, NativeMethods.INPUT.Size);
    }

    private static void Send(NativeMethods.INPUT input) =>
        NativeMethods.SendInput(1, new[] { input }, NativeMethods.INPUT.Size);
}
