using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuumable : MonoBehaviour
{
    PlayerStateManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Vacuum))
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * (1/Vector2.Distance(transform.position, player.transform.position))*3);
        }
    }
}
