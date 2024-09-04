using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    PlayerStateManager player;
    [SerializeField] UnityEvent OnCollect;
    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)return;
        if (Vector2.Distance(player.transform.position, transform.position) <= 1)
        {
            OnCollect.Invoke();
        }
    }
}
