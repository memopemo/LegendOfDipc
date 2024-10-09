using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RefillPoint : TrashBin
{
    [SerializeField] GameObject HealParticles;
    public override void OnAction()
    {
        PlayerHealth.SetHealth(SaveManager.GetSave().HeartContainersCollected.Count(gotten => gotten)*2+6); //Set health to amount of collected heartcontainers * 2 + initial amount.
        Invoke(nameof(SpitOutPlayer),1);
        Instantiate(HealParticles, transform.position+Vector3.left*0.5f+Vector3.back*2, HealParticles.transform.rotation);
        
    }
}
