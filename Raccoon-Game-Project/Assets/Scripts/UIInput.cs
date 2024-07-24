using UnityEngine;
public static class UIInput
{
    public static bool IsDownPressed() 
    {
        return Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.S);
    }

    public static bool IsUpPressed() 
    {
        return Input.GetKeyDown(KeyCode.UpArrow)   ||
            Input.GetKeyDown(KeyCode.W);
    }
    
    public static bool IsRightPressed()
    {
        return Input.GetKeyDown(KeyCode.RightArrow)    ||
            Input.GetKeyDown(KeyCode.D);
    }

    public static bool IsLeftPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow)     ||
            Input.GetKeyDown(KeyCode.A);
    }

    public static bool IsSwitchPressed() 
    {
        return Input.GetButtonDown("Fire1") ||
            Input.GetButtonDown("Fire2") ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.LeftControl);
    }
}