using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloak : MonoBehaviour
{
    void Start()
    {
        FindFirstObjectByType<PlayerStateManager>().SwitchState(new CloakedPlayerState());
        Destroy(gameObject);
    }
}
