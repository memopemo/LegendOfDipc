using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    float distance;
    const float SPEED = 10f;
    [SerializeField] Sprite[] boomerangSprites;
    int boomerang;
    bool isReturning;
    PlayerStateManager player;
    NoiseMaker noiseMaker;
    // Start is called before the first frame update
    void Start()
    {
        boomerang = SaveManager.GetSave().CurrentBoomerang;
        if(boomerang < 0 || boomerang >= 4)
        {
            //this shouldnt happen, but just in case! :)
            Destroy(gameObject);
            return;
        }
        GetComponent<SpriteRenderer>().sprite = boomerangSprites[boomerang];
        player = FindAnyObjectByType<PlayerStateManager>();
        GetComponent<FineDirectionedObject>().direction = (Vector2)player.directionedObject.direction * SPEED;
        GetComponent<Homing>().speed = SPEED;
        GetComponent<DamagesEnemy>().amount = boomerang*3;
        noiseMaker = GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 3 == 0)
        {
            if(transform.rotation.eulerAngles.z == 0)
            {
                noiseMaker.Play(0);
            }
            transform.Rotate(Vector3.back*90);
        }
        if (isReturning && Vector2.Distance(transform.position, player.transform.position) < 1f)
        {
            Destroy(gameObject);
        }
    }
    public void OnHitSomething()
    {
        GetComponent<Projectile>().isHitting = false;
        if(!isReturning)
        {
            isReturning = true;
            GetComponent<Homing>().enabled = true;
            GetComponent<Homing>().target = player.transform;
            GetComponent<MoveStraight>().enabled = false;

        }
    }
}
