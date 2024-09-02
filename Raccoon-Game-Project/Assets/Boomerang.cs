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
    float timeTraveled;
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectsByType<Boomerang>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
        
        boomerang = SaveManager.GetSave().CurrentBoomerang;
        if(boomerang < 0 || boomerang >= 4)
        {
            //this shouldnt happen, but just in case! :)
            Destroy(gameObject);
            return;
        }
        GetComponent<SpriteRenderer>().sprite = boomerangSprites[boomerang];
        player = FindAnyObjectByType<PlayerStateManager>();
        FineDirectionedObject fineDirectioned = GetComponent<FineDirectionedObject>();
        fineDirectioned.direction = (Vector2)player.directionedObject.direction * SPEED;
        if(player.rawInput.x != 0 && player.rawInput.y != 0)
        {
            fineDirectioned.direction = player.rawInput.normalized * SPEED;
        }
        GetComponent<Homing>().speed = SPEED;
        GetComponent<DamagesEnemy>().amount = boomerang*3;
        GetComponent<Projectile>().IsMagic = boomerang == 3;
        noiseMaker = GetComponent<NoiseMaker>();
        noiseMaker.Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 3 == 0)
        {
            transform.Rotate(Vector3.back*90);
        }
        if (isReturning && Vector2.Distance(transform.position, player.transform.position) < 1f)
        {
            Destroy(gameObject);
        }
        timeTraveled += Time.deltaTime * SPEED;
        if(timeTraveled > (boomerang+1)*5)
        {
            OnHitSomething(); //return early.
        }
    }
    public void OnHitSomething()
    {
        GetComponent<Projectile>().isHitting = false;
        if(!isReturning)
        {
            noiseMaker.Play(1);
            isReturning = true;
            GetComponent<Homing>().enabled = true;
            GetComponent<Homing>().target = player.transform;
            GetComponent<MoveStraight>().enabled = false;

        }
    }
}
