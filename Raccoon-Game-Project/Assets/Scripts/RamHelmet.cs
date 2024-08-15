using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamHelmet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindFirstObjectByType<PlayerStateManager>().SwitchState(new RammingPlayerState());
        Destroy(gameObject);
    }
}
