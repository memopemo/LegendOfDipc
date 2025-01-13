using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class FollowEnemy : MonoBehaviour
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
        var previous = (Vector2)transform.position;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
        GetComponent<DirectionedObject>().direction = Vector2Int.RoundToInt((Vector2)transform.position - previous);
        /*
        if (Vector2.Distance(transform.position, player.transform.position) < 1f)
        {
            Utils.ForceCrash(ForcedCrashCategory.FatalError);
            FindFirstObjectByType<SongPlayer>().GetComponent<AudioSource>().timeSamples = 1000;
        }*/
    }
}
