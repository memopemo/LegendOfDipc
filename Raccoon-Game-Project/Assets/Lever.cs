using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    Switch switcher;
    void Start()
    {
        switcher = GetComponent<Switch>();
    }
    public void Flip()
    {
        if(switcher.switchA == 0)
        {
            switcher.ActivateSwitchA(1);
            GetComponent<NoiseMaker>().Play(0);
            GetComponent<DirectionedObject>().direction = Vector2Int.left;
        }
        else
        {
            switcher.DeactivateSwitchA();
            GetComponent<NoiseMaker>().Play(0);
            GetComponent<DirectionedObject>().direction = Vector2Int.right;
        }
    }
}
