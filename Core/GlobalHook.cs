using System.Runtime.InteropServices;
using Macro.Models;

namespace Macro.Core;

public sealed class KeyHookArgs : EventArgs
{
    public Keys Key      { get; }
    public bool Suppress { get; set; }
    public KeyHookArgs(Keys key) => Key = key;
}

public sealed class MouseHookArgs : EventArgs
{
    public int        X      { get; }
    public int        Y      { get; }
    public ActionType Button { get; }
    public MouseHookArgs(int x, int y, ActionType button) { X = x; Y = y; Button = button; }
}

internal sealed class GlobalHook : IDisposable
{
    private IntPtr _kbHook;
    private IntPtr _msHook;

    // 콜백 델리게이트 — GC 수집 방지를 위해 필드로 유지
    private readonly NativeMethods.HookProc _kbProc;
    private readonly NativeMethods.HookProc _msProc;

    public event EventHandler<KeyHookArgs>?   KeyDown;
    public event EventHandler<MouseHookArgs>? MouseDown;

    public GlobalHook()
    {
        _kbProc = KbCallback;
        _msProc = MsCallback;
    }

    public void Install()
    {
        _kbHook = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, _kbProc, IntPtr.Zero, 0);
        _msHook = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL,    _msProc, IntPtr.Zero, 0);
    }

    private IntPtr KbCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var s        = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
            bool injected = (s.flags & NativeMethods.LLKHF_INJECTED) != 0;
            int  msg      = (int)wParam;

            if (!injected && (msg == NativeMethods.WM_KEYDOWN || msg == NativeMethods.WM_SYSKEYDOWN))
            {
                var args = new KeyHookArgs((Keys)s.vkCode);
                KeyDown?.Invoke(this, args);
                if (args.Suppress) return (IntPtr)1;
            }
        }
        return NativeMethods.CallNextHookEx(_kbHook, nCode, wParam, lParam);
    }

    private IntPtr MsCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var s        = Marshal.PtrToStructure<NativeMethods.MSLLHOOKSTRUCT>(lParam);
            bool injected = (s.flags & NativeMethods.LLMHF_INJECTED) != 0;

            if (!injected)
            {
                ActionType? btn = (int)wParam switch
                {
                    NativeMethods.WM_LBUTTONDOWN => ActionType.MouseLeftClick,
                    NativeMethods.WM_RBUTTONDOWN => ActionType.MouseRightClick,
                    _                            => null
                };
                if (btn.HasValue)
                    MouseDown?.Invoke(this, new MouseHookArgs(s.pt.X, s.pt.Y, btn.Value));
            }
        }
        return NativeMethods.CallNextHookEx(_msHook, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (_kbHook != IntPtr.Zero) { NativeMethods.UnhookWindowsHookEx(_kbHook); _kbHook = IntPtr.Zero; }
        if (_msHook != IntPtr.Zero) { NativeMethods.UnhookWindowsHookEx(_msHook); _msHook = IntPtr.Zero; }
    }
}
