using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject_TV : SmartObject
{
    public bool IsOn {get; protected set;} = false;

    public void ToggleState()
    {
        IsOn = !IsOn;

        Debug.Log($"TV is now {(IsOn ? "ON" : "Off")}");
    }

}
