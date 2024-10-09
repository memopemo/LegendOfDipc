using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverKeyItem : MonoBehaviour
{
    [SerializeField] HoverPlayerState.HoverType hoverType;
    void Start()
    {
        FindFirstObjectByType<PlayerStateManager>().SwitchState(new HoverPlayerState(hoverType));
        Destroy(gameObject);
    }
}
