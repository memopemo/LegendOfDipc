using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrainWarpPipe : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerStateManager>())
        {
            //FreezeManager.FreezeAll<PauseFreezer>();
            GameObject go = Instantiate(new GameObject());
            TextMeshPro tmp = go.AddComponent<TextMeshPro>();
            tmp.text = "hi";
            tmp.fontSize = 18;

        }
    }
}
