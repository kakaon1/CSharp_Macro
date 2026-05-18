using Macro.Models;

namespace Macro.Core;

internal sealed class MacroRecorder
{
    private readonly GlobalHook        _hook;
    private readonly List<MacroAction> _actions;
    private readonly Form              _owner;

    private bool     _recording;
    private DateTime _lastTime;

    private static readonly HashSet<Keys> HotkeySet = new() { Keys.F6, Keys.F7 };

    public event EventHandler<MacroAction>? ActionAdded;

    public MacroRecorder(GlobalHook hook, List<MacroAction> actions, Form owner)
    {
        _hook    = hook;
        _actions = actions;
        _owner   = owner;
    }

    public void Start()
    {
        if (_recording) return;
        _recording = true;
        _lastTime  = DateTime.Now;
        _hook.KeyDown   += OnKeyDown;
        _hook.MouseDown += OnMouseDown;
    }

    public void Stop()
    {
        if (!_recording) return;
        _recording = false;
        _hook.KeyDown   -= OnKeyDown;
        _hook.MouseDown -= OnMouseDown;
    }

    private int TakeDelay()
    {
        var now = DateTime.Now;
        int ms  = (int)(now - _lastTime).TotalMilliseconds;
        _lastTime = now;
        return Math.Clamp(ms, 0, 5000);
    }

    private void Add(MacroAction action)
    {
        _actions.Add(action);
        ActionAdded?.Invoke(this, action);
    }

    private void OnKeyDown(object? sender, KeyHookArgs e)
    {
        if (!_recording) return;

        if (e.Key == Keys.F7)
        {
            var pos = Cursor.Position;
            Add(new MacroAction { Type = ActionType.MouseMove, X = pos.X, Y = pos.Y, DelayMs = TakeDelay() });
            return;
        }

        if (HotkeySet.Contains(e.Key)) return;

        Add(new MacroAction
        {
            Type    = ActionType.KeyPress,
            VkCode  = (int)e.Key,
            KeyName = e.Key.ToString(),
            DelayMs = TakeDelay()
        });
    }

    private void OnMouseDown(object? sender, MouseHookArgs e)
    {
        if (!_recording) return;
        if (_owner.Bounds.Contains(e.X, e.Y)) return;
        Add(new MacroAction { Type = e.Button, X = e.X, Y = e.Y, DelayMs = TakeDelay() });
    }
}
