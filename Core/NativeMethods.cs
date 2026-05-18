using System.Runtime.InteropServices;

namespace Macro.Core;

internal static class NativeMethods
{
    // Hook 종류
    public const int WH_KEYBOARD_LL = 13;
    public const int WH_MOUSE_LL    = 14;

    // 키보드 메시지
    public const int WM_KEYDOWN    = 0x0100;
    public const int WM_KEYUP      = 0x0101;
    public const int WM_SYSKEYDOWN = 0x0104;
    public const int WM_SYSKEYUP   = 0x0105;

    // 마우스 메시지
    public const int WM_LBUTTONDOWN  = 0x0201;
    public const int WM_RBUTTONDOWN  = 0x0204;

    // SendInput 상수
    public const int  INPUT_MOUSE    = 0;
    public const int  INPUT_KEYBOARD = 1;
    public const uint KEYEVENTF_KEYUP        = 0x0002;
    public const uint MOUSEEVENTF_MOVE       = 0x0001;
    public const uint MOUSEEVENTF_LEFTDOWN   = 0x0002;
    public const uint MOUSEEVENTF_LEFTUP     = 0x0004;
    public const uint MOUSEEVENTF_RIGHTDOWN  = 0x0008;
    public const uint MOUSEEVENTF_RIGHTUP    = 0x0010;
    public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
    public const uint MOUSEEVENTF_ABSOLUTE   = 0x8000;

    // 주입된 이벤트 식별 플래그
    public const uint LLKHF_INJECTED = 0x10;
    public const uint LLMHF_INJECTED = 0x01;

    // 우리가 주입한 이벤트임을 표시하는 마커
    public const uint MACRO_MARKER = 0xFEED1234;

    // 화면 크기 조회
    public const int SM_CXSCREEN = 0;
    public const int SM_CYSCREEN = 1;
    public const int SM_XVIRTUALSCREEN  = 76;
    public const int SM_YVIRTUALSCREEN  = 77;
    public const int SM_CXVIRTUALSCREEN = 78;
    public const int SM_CYVIRTUALSCREEN = 79;

    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")] public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);
    [DllImport("user32.dll")] public static extern bool   UnhookWindowsHookEx(IntPtr hhk);
    [DllImport("user32.dll")] public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")] public static extern uint   SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    [DllImport("user32.dll")] public static extern int    GetSystemMetrics(int nIndex);
    [DllImport("user32.dll")] public static extern bool   GetCursorPos(out POINT lpPoint);
    [DllImport("user32.dll")] public static extern bool   SetCursorPos(int X, int Y);

    public static System.Drawing.Rectangle GetVirtualScreenBounds() =>
        new(
            GetSystemMetrics(SM_XVIRTUALSCREEN),
            GetSystemMetrics(SM_YVIRTUALSCREEN),
            GetSystemMetrics(SM_CXVIRTUALSCREEN),
            GetSystemMetrics(SM_CYVIRTUALSCREEN));

    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT
    {
        public uint   vkCode;
        public uint   scanCode;
        public uint   flags;
        public uint   time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT  pt;
        public uint   mouseData;
        public uint   flags;
        public uint   time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT { public int X; public int Y; }

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public int        type;
        public InputUnion U;
        public static int Size => Marshal.SizeOf(typeof(INPUT));
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT  mi;
        [FieldOffset(0)] public KEYBDINPUT  ki;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int    dx;
        public int    dy;
        public uint   mouseData;
        public uint   dwFlags;
        public uint   time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint   dwFlags;
        public uint   time;
        public IntPtr dwExtraInfo;
    }
}
