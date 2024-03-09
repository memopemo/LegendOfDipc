using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerWaterReflection : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer followRenderer;
    [SerializeField] float distance;
    [SerializeField] float yOffset;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnValidate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, distance);
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
        transform.position += Vector3.forward * distance;
        transform.position += Vector3.down * (a.height.height + yOffset);
        spriteRenderer.flipX = followRenderer.flipX;
        spriteRenderer.flipY = !followRenderer.flipY;
    }
}
