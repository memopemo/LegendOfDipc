using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DarkBlock : MonoBehaviour
{
    [SerializeField] bool isVoid; //must require both lights to be shined on if true. Else, only require either one.
    LightSource lightSource1;
    LightSource lightSource2;
    float secondsBeingShinedOn;
    const float SECS_UNTIL_DESTROY = 2f;
    bool isBeingShinedOn;
    SpriteRenderer spriteRenderer;
    SpriteRenderer childSR;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(CheckLightSources), 0.1f, 0.1f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        childSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    void Update()
    {       
        secondsBeingShinedOn += (isBeingShinedOn ? 1 : -2) * Time.deltaTime; //add or subtract based on time being shun on.
        if(secondsBeingShinedOn >= SECS_UNTIL_DESTROY)
        {
            Die();
        }
        secondsBeingShinedOn = Mathf.Clamp(secondsBeingShinedOn, 0, SECS_UNTIL_DESTROY+1);
        spriteRenderer.color = new Color(secondsBeingShinedOn, secondsBeingShinedOn, secondsBeingShinedOn, 1);
        if(isVoid)
            childSR.color = new Color(childSR.color.r, childSR.color.g, childSR.color.b, 1-secondsBeingShinedOn);

    }

    // Update is called once per frame
    void CheckLightSources()
    {
        if(!lightSource1 && !lightSource2)
        {
            isBeingShinedOn = false;
            return;
        }
        bool canSeeLight1 = false;
        bool canSeeLight2 = false;
        if(lightSource1)
        {
            CheckLineOfSight(ref canSeeLight1, lightSource1);
        }
        if(lightSource2)
        {
            CheckLineOfSight(ref canSeeLight2, lightSource2);
        }
        isBeingShinedOn = isVoid ? canSeeLight1 && canSeeLight2 : canSeeLight1 || canSeeLight2;

    }

    private void CheckLineOfSight(ref bool canSee, LightSource lightSource)
    {
        //disable raycasting for ourselves temporarily.
        int layer = gameObject.layer;
        int lightLayer = lightSource.gameObject.layer;
        gameObject.layer = 2;
        lightSource.gameObject.layer = 2;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, lightSource.transform.position);

        //reenable
        gameObject.layer = layer;
        lightSource.gameObject.layer = lightLayer;
        if (hit)
        {
            print($"{gameObject.name} hit {hit.collider.gameObject.name}");
            return;
        }
        canSee = true;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(isBeingShinedOn) return;
        if (collider.TryGetComponent(out LightSource light))
        {
            if (lightSource1 != null && lightSource1 != light)
            {
                lightSource2 = light;
            }
            else
            {
                lightSource1 = light;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        //check if it was our light
        if (collider.TryGetComponent(out LightSource light))
        {
            if (light == lightSource1)
            {
                lightSource1 = null;
                //move other light source if applicable
                if(lightSource2)
                {
                    lightSource1 = lightSource2;
                    lightSource2 = null;
                }
            }
            else if (light == lightSource2)
            {
                lightSource2 = null;
            }
        }
    }
    void Die()
    {
        GetComponent<PoofDestroy>()?.Poof();
        Destroy(gameObject);
    }
}
