using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoofDestroy))]
public class ToiletPaper : MonoBehaviour
{
    PlayerStateManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerStateManager>();
        GetComponent<NoiseMaker>().Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Vector3.Distance(player.transform.position, transform.position) < 1)
        {
            SaveManager.GetSave().ToiletPaperRolls++;
            GetComponent<NoiseMaker>().Play(1);
            GetComponent<PoofDestroy>().PoofAndDestroy();
        }
    }
}
