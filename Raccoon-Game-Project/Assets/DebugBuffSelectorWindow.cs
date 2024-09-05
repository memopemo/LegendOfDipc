using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBuffSelectorWindow : MonoBehaviour
{
    #if DEBUG
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        }
    }
    #endif
}
