using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoots : MonoBehaviour
{
    [SerializeField] GameObject iceBlock;
    void Start()
    {
        FindFirstObjectByType<PlayerStateManager>().SwitchState(new JumpingPlayerState(iceBlock));
        Destroy(gameObject);
    }
}
