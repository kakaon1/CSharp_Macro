using Macro.Models;

namespace Macro.Core;

/// <summary>
/// INI 형식으로 매크로와 설정을 저장/불러오기.
/// 섹션: [Settings], [Macro], [Action_N]
/// </summary>
internal static class MacroStorage
{
    public static string AutoSavePath =>
        Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath ?? "") ?? AppContext.BaseDirectory,
            "macro.ini");

    // ── 저장 ────────────────────────────────────────────────

    public static void Save(List<MacroAction> actions, MacroSettings settings, string? path = null)
    {
        string target = path ?? AutoSavePath;
        var dir = Path.GetDirectoryName(target);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        var lines = new List<string>();

        lines.Add("[Settings]");
        lines.Add($"ExecuteKey={(int)settings.ExecuteKey}");
        lines.Add($"RepeatCount={settings.RepeatCount}");
        lines.Add($"UseDelay={settings.UseDelay}");
        lines.Add($"ScreenLeft={settings.ScreenLeft}");
        lines.Add($"ScreenTop={settings.ScreenTop}");
        lines.Add($"ScreenWidth={settings.ScreenWidth}");
        lines.Add($"ScreenHeight={settings.ScreenHeight}");
        lines.Add("");

        lines.Add("[Macro]");
        lines.Add($"Count={actions.Count}");
        lines.Add("");

        for (int i = 0; i < actions.Count; i++)
        {
            var a = actions[i];
            lines.Add($"[Action_{i}]");
            lines.Add($"Type={(int)a.Type}");
            lines.Add($"X={a.X}");
            lines.Add($"Y={a.Y}");
            lines.Add($"VkCode={a.VkCode}");
            lines.Add($"KeyName={a.KeyName}");
            lines.Add($"FilePath={a.FilePath}");
            lines.Add($"ProcessName={a.ProcessName}");
            lines.Add($"DelayMs={a.DelayMs}");
            lines.Add("");
        }

        File.WriteAllLines(target, lines);
    }

    // ── 불러오기 ─────────────────────────────────────────────

    public static (List<MacroAction> Actions, MacroSettings Settings) Load(string? path = null)
    {
        string target  = path ?? AutoSavePath;
        var actions    = new List<MacroAction>();
        var settings   = new MacroSettings();

        if (!File.Exists(target)) return (actions, settings);

        try
        {
            var ini = ParseIni(File.ReadAllLines(target));

            // Settings
            if (ini.TryGetValue("Settings", out var s))
            {
                if (s.TryGetValue("ExecuteKey", out var ek) && int.TryParse(ek, out int ekv))
                    settings.ExecuteKey = (Keys)ekv;
                if (s.TryGetValue("RepeatCount", out var rc) && int.TryParse(rc, out int rcv))
                    settings.RepeatCount = Math.Max(1, rcv);
                if (s.TryGetValue("UseDelay", out var ud))
                    settings.UseDelay = ud.Equals("true", StringComparison.OrdinalIgnoreCase);
                if (s.TryGetValue("ScreenLeft",   out var sl) && int.TryParse(sl, out int slv)) settings.ScreenLeft   = slv;
                if (s.TryGetValue("ScreenTop",    out var st) && int.TryParse(st, out int stv)) settings.ScreenTop    = stv;
                if (s.TryGetValue("ScreenWidth",  out var sw) && int.TryParse(sw, out int swv)) settings.ScreenWidth  = swv;
                if (s.TryGetValue("ScreenHeight", out var sh) && int.TryParse(sh, out int shv)) settings.ScreenHeight = shv;
            }

            // 액션 수
            int count = 0;
            if (ini.TryGetValue("Macro", out var m) && m.TryGetValue("Count", out var cv))
                int.TryParse(cv, out count);

            for (int i = 0; i < count; i++)
            {
                if (!ini.TryGetValue($"Action_{i}", out var av)) break;
                var a = new MacroAction();
                if (av.TryGetValue("Type",        out var t)  && int.TryParse(t, out int tv))   a.Type        = (ActionType)tv;
                if (av.TryGetValue("X",           out var x)  && int.TryParse(x, out int xv))   a.X           = xv;
                if (av.TryGetValue("Y",           out var y)  && int.TryParse(y, out int yv))   a.Y           = yv;
                if (av.TryGetValue("VkCode",      out var vk) && int.TryParse(vk, out int vkv)) a.VkCode      = vkv;
                if (av.TryGetValue("KeyName",     out var kn)) a.KeyName     = kn;
                if (av.TryGetValue("FilePath",    out var fp)) a.FilePath    = fp;
                if (av.TryGetValue("ProcessName", out var pn)) a.ProcessName = pn;
                if (av.TryGetValue("DelayMs",     out var dm) && int.TryParse(dm, out int dmv)) a.DelayMs = dmv;
                actions.Add(a);
            }
        }
        catch { /* 손상된 파일은 무시 */ }

        return (actions, settings);
    }

    // ── INI 파서 ─────────────────────────────────────────────

    private static Dictionary<string, Dictionary<string, string>> ParseIni(string[] lines)
    {
        var result  = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        string? sec = null;

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith(';') || line.StartsWith('#')) continue;

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                sec = line[1..^1].Trim();
                result.TryAdd(sec, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
            }
            else if (sec != null)
            {
                var eq = line.IndexOf('=');
                if (eq > 0)
                    result[sec][line[..eq].Trim()] = line[(eq + 1)..].Trim();
            }
        }
        return result;
    }
}

public class MacroSettings
{
    public Keys ExecuteKey  { get; set; } = Keys.None;
    public int  RepeatCount { get; set; } = 1;
    public bool UseDelay    { get; set; } = true;
    public int  ScreenLeft   { get; set; }
    public int  ScreenTop    { get; set; }
    public int  ScreenWidth  { get; set; }
    public int  ScreenHeight { get; set; }
}
