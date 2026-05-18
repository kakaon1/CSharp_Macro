namespace Macro.Models;

public enum ActionType
{
    MouseLeftClick,
    MouseRightClick,
    MouseMove,
    KeyPress,
    Delay,
    LaunchProgram,
    KillProgram
}

public class MacroAction
{
    public ActionType Type        { get; set; }
    // 마우스
    public int        X           { get; set; }
    public int        Y           { get; set; }
    // 키보드
    public int        VkCode      { get; set; }
    public string     KeyName     { get; set; } = string.Empty;
    // 프로그램
    public string     FilePath    { get; set; } = string.Empty;
    public string     ProcessName { get; set; } = string.Empty;
    // 공통
    public int        DelayMs     { get; set; }

    public string GetDescription() => Type switch
    {
        ActionType.MouseLeftClick  => "마우스 좌클릭",
        ActionType.MouseRightClick => "마우스 우클릭",
        ActionType.MouseMove       => $"마우스 이동  ({X}, {Y})",
        ActionType.KeyPress        => $"키 입력: {KeyName}",
        ActionType.Delay           => $"지연: {DelayMs}ms",
        ActionType.LaunchProgram   => $"프로그램 실행: {Path.GetFileName(FilePath)}",
        ActionType.KillProgram     => $"프로그램 종료: {ProcessName}",
        _                          => "알 수 없음"
    };
}
