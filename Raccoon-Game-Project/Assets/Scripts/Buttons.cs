using UnityEngine;

//a remappable button thing for easy access.
public static class Buttons
{
    public static Button Sword = new("A", KeyCode.Z);
    public static Button KeyItem = new("B", KeyCode.X);
    public static Button ConsumableItem = new("X", KeyCode.C);
    public static Button Jump = new("Y", KeyCode.Space);
    public static Button Shield = new("ZR", KeyCode.LeftShift);
    public static Button Boomerang = new("ZL", KeyCode.LeftControl);
    public static Button IncrementItem = new("R", KeyCode.S);
    public static Button DecrementItem = new("L", KeyCode.A);
    public static Button ToggleItem = new("SR", KeyCode.D);
    public static Button Pause = new("Select", KeyCode.Escape);
    public static Button Inventory = new("Start", KeyCode.E);
    public static Vector2Int AxisDown;
    public static Vector2Int AxisHeld;
    public static Vector2Int AxisUp;
    static Vector2Int TriggerAxisDown; //L is Horizontal, R is Vertical
    static Vector2Int TriggerAxisHeld;
    static Vector2Int TriggerAxisUp;

    
    public static bool IsButtonDown(Button buttonThing)
    {
        if(buttonThing.ControllerName == "ZL" || buttonThing.ControllerName == "ZR")
        {
            return GetTriggerDown(buttonThing.ControllerName) || Input.GetKeyDown(buttonThing.KeyName);
        }
        return Input.GetButtonDown(buttonThing.ControllerName) || Input.GetKeyDown(buttonThing.KeyName);
    }
    public static bool IsButtonHeld(Button buttonThing)
    {
        if(buttonThing.ControllerName == "ZL" || buttonThing.ControllerName == "ZR")
        {
            return GetTriggerHeld(buttonThing.ControllerName) || Input.GetKey(buttonThing.KeyName);
        }
        return Input.GetButton(buttonThing.ControllerName) || Input.GetKey(buttonThing.KeyName);
    }
    public static bool IsButtonUp(Button buttonThing)
    {
        if(buttonThing.ControllerName == "ZL" || buttonThing.ControllerName == "ZR")
        {
            return GetTriggerUp(buttonThing.ControllerName) || Input.GetKeyUp(buttonThing.KeyName);
        }
        return Input.GetButtonUp(buttonThing.ControllerName) || Input.GetKeyUp(buttonThing.KeyName);
    }
    public static void MapButton(Button button, string controllerName)
    {
        button.ControllerName = controllerName;
    }
    public static void MapButton(Button button, KeyCode keyName)
    {
        button.KeyName = keyName;
    }
    public static void UpdateAxis()
    {
        Vector2Int inputtedAxis = Vector2Int.RoundToInt(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);
        if(inputtedAxis != AxisHeld)
        {
            AxisDown = inputtedAxis;
            AxisUp = AxisHeld;
        }
        else
        {
            AxisDown = Vector2Int.zero;
            AxisUp = Vector2Int.zero;
        }
        AxisHeld = inputtedAxis;

        //do trigger axes.
        Vector2Int inputtedTriggerAxis = Vector2Int.RoundToInt(new Vector2(Input.GetAxisRaw("ZL"), Input.GetAxisRaw("ZR")).normalized);
        if(inputtedTriggerAxis != TriggerAxisHeld)
        {
            TriggerAxisDown = inputtedTriggerAxis;
            TriggerAxisUp = TriggerAxisHeld;
        }
        else
        {
            TriggerAxisDown = Vector2Int.zero;
            TriggerAxisUp = Vector2Int.zero;
        }
        TriggerAxisHeld = inputtedTriggerAxis;
        //Debug.Log($"Held:{AxisHeld} Down:{AxisDown} Up:{AxisUp}");
    }
    static bool GetTriggerDown(string trigger)
    {
        if(trigger == "ZL") return TriggerAxisDown.x == 1;
        else if(trigger == "ZR") return TriggerAxisDown.y == 1;
        else return false;
    }
    static bool GetTriggerHeld(string trigger)
    {
        if(trigger == "ZL") return TriggerAxisHeld.x == 1;
        else if(trigger == "ZR") return TriggerAxisHeld.y == 1;
        else return false;
    }
    static bool GetTriggerUp(string trigger)
    {
        if(trigger == "ZL") return TriggerAxisUp.x == 1;
        else if(trigger == "ZR") return TriggerAxisUp.y == 1;
        else return false;
    }
}

public class Button
{
    public Button(string controllerName, KeyCode keyCode)
    {
        ControllerName = controllerName;
        KeyName = keyCode;
    }
    public string ControllerName;
    public KeyCode KeyName;
}
