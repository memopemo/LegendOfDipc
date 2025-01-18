using UnityEngine;
public static class UIInput
{
    public static bool IsDownPressed => Buttons.AxisDown.y == -1 || Input.GetKeyDown(KeyCode.S);

    public static bool IsUpPressed => Buttons.AxisDown.y == 1 || Input.GetKeyDown(KeyCode.W);

    public static bool IsRightPressed => Buttons.AxisDown.x == 1 || Input.GetKeyDown(KeyCode.D);

    public static bool IsLeftPressed => Buttons.AxisDown.x == -1 || Input.GetKeyDown(KeyCode.A);

    public static bool IsSwitchPressed => Buttons.IsButtonDown(Buttons.Sword) ||
        Buttons.IsButtonDown(Buttons.KeyItem) ||
        Buttons.IsButtonDown(Buttons.ConsumableItem) ||
        Input.GetKeyDown(KeyCode.Return) ||
        Input.GetKeyDown(KeyCode.Space);
    public static bool IsConfirmPressed => Buttons.IsButtonDown(Buttons.Sword) || Input.GetKeyDown(KeyCode.Return) ||
        Input.GetKeyDown(KeyCode.Space);
    public static bool IsConfirmReleased => Buttons.IsButtonUp(Buttons.Sword) || Input.GetKeyUp(KeyCode.Return) ||
        Input.GetKeyUp(KeyCode.Space);
    public static bool IsBackPressed => Buttons.IsButtonDown(Buttons.KeyItem) || Input.GetKeyDown(KeyCode.Backspace);
    public static bool IsBackReleased => Buttons.IsButtonUp(Buttons.KeyItem) || Input.GetKeyUp(KeyCode.Backspace);
}