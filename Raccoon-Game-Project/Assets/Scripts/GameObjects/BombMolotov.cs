using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class BombMolotov : MonoBehaviour
{
    Vector2Int direction;
    [SerializeField] AnimationCurve throwHeight;
    float time;
    bool isSpreadingFire;
    [SerializeField] GameObject fire;
    Rigidbody2D rb;
    Heightable heightable;
    int fireSpreadStage;

    readonly Vector2Int[,] spawnFirePositions = 
    {
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        },
        {
            new Vector2Int(1,1),
            new Vector2Int(-1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,-1),
        },
        {
            Vector2Int.up*2,
            Vector2Int.down*2,
            Vector2Int.left*2,
            Vector2Int.right*2
        },
    };
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        heightable = GetComponent<Heightable>();
        direction = FindFirstObjectByType<PlayerStateManager>().directionedObject.direction;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSpreadingFire)
        {
            rb.velocity = direction;
            time += Time.deltaTime;
            if(time >= throwHeight.keys.Last().time)
            {
                isSpreadingFire = true;
                transform.position = SnapGrid.SnapToGridCentered(transform.position);
                Instantiate(fire, gameObject.transform.position, Quaternion.identity);
                InvokeRepeating(nameof(SpreadFire),1,1);
                rb.velocity = Vector2.zero;
            }
            heightable.height = throwHeight.Evaluate(time);
        }
    }
    void SpreadFire()
    {
        if(!isSpreadingFire) return;
        int layer = gameObject.layer; //temp disable ourselves from colision.
        Vector2 pos = transform.position;
        gameObject.layer = 2;
        for (int i = 0; i < 4; i++)
        {   
            RaycastHit2D rc = Physics2D.Linecast(pos, pos + spawnFirePositions[fireSpreadStage,i]);
            if(!rc)
            {
                Instantiate(fire, pos + spawnFirePositions[fireSpreadStage, i], Quaternion.identity);
                continue;
            }
            print("cant create fire: "+ rc.collider.name);
        }
        gameObject.layer = layer;
        fireSpreadStage++;
        if(fireSpreadStage == 3)
        {
            print("done spreading fire zzzzzz");
            Destroy(gameObject);
        }
    }
}
