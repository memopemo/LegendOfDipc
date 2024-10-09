using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerWaterReflection : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer followRenderer;
    const float DISTANCE = 4;
    const float Y_OFFSET = 1.125f;
    float tspFade;
    bool exiting;
    // Start is called before the first frame update
    void Start()
    {
        tspFade = 0;
        exiting = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.clear;
    }
    private void OnValidate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, DISTANCE);
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerStateManager a = FindFirstObjectByType<PlayerStateManager>();
        if (a == null)
        {
            return;
        }
        // Jump sprite is a different obj/sprite renderer
        /*if (a.currentPlayerState is JumpingPlayerState)
        {
            followRenderer = a.height.
        }
        else
        {*/
            followRenderer = a.defaultSpriteRenderer;
        //}
        //set fade
        tspFade += Time.deltaTime * (exiting ? -1: 1);
        tspFade = Mathf.Clamp(tspFade, 0,1);
        spriteRenderer.color = new Color(1,1,1,tspFade);
        if(exiting && tspFade <= 0)
        {
            Destroy(gameObject);
        }

        if (a.currentPlayerState is SwimPlayerState)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        /* Mimic the sprite renderer except:
         * 
         * flip sprite opposite of sprite.
         * offset y-pos by set amount (artistically)
         * position behind water with distance variable.
         */
        spriteRenderer.sprite = followRenderer.sprite;
        transform.position = a.transform.position;
        transform.position += Vector3.forward * DISTANCE;
        transform.position += Vector3.down * (a.height.height + Y_OFFSET);
        spriteRenderer.flipX = followRenderer.flipX;
        spriteRenderer.flipY = !followRenderer.flipY;
    }
    public void Exit()
    {
        tspFade = 1;
        exiting = true;
    }
}
