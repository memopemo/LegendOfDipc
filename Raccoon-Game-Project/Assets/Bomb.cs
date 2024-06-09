using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float timeUntilBoomSecs;
    void Update()
    {
        timeUntilBoomSecs -= Time.deltaTime;
        if(timeUntilBoomSecs <= 0)
        {
            GetComponent<PoofDestroy>().Poof();
            Destroy(gameObject);
        }
    }
}
