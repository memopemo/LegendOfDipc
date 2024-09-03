using UnityEngine;
public static class UIInput
{
    public static bool IsDownPressed => Buttons.AxisDown.y == -1;

    public static bool IsUpPressed => Buttons.AxisDown.y == 1;

    public static bool IsRightPressed => Buttons.AxisDown.x == 1;

    public static bool IsLeftPressed => Buttons.AxisDown.x == -1;

    public static bool IsSwitchPressed => Buttons.IsButtonDown(Buttons.Sword) ||
        Buttons.IsButtonDown(Buttons.KeyItem) ||
        Buttons.IsButtonDown(Buttons.ConsumableItem);
}