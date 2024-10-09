using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGboy : MonoBehaviour
{
    [SerializeField] GameObject combatScreen;
    RPGable rPGable;
    int ANIM_PLAYER_POINT = 4;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.animator.SetAnimation(ANIM_PLAYER_POINT);
        Vector2 direction = player.directionedObject.direction;
        transform.rotation = Quaternion.Euler(0, 0, Rotation.DirectionToAngle(direction) - 90);
        
        rPGable = null;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, direction, 10);
        foreach (var hit in hits)
        {
            if(hit.collider.TryGetComponent(out RPGable testRPGable))
            {
                rPGable = testRPGable;
            }
        }
        FreezeManager.FreezeAll<PauseFreezer>();
        Invoke(nameof(StartBattle), 0.3f);
        if(rPGable == null)
        {
            Invoke(nameof(FailedBattle), 1.5f);
        }
        Invoke(nameof(Die), 3f);
        
    }
    void StartBattle()
    {
        UIRPGBoyIntroBar bar = FindFirstObjectByType<UIRPGBoyIntroBar>(FindObjectsInactive.Include);
        bar.gameObject.SetActive(true);
        bar.Play(rPGable);
        
    }
    void Die()
    {
        UIRPGBoyIntroBar bar = FindFirstObjectByType<UIRPGBoyIntroBar>(FindObjectsInactive.Include);
        bar.gameObject.SetActive(false);
        Destroy(gameObject);  
    }
    void FailedBattle()
    {
        Destroy(gameObject);
        FreezeManager.UnfreezeAll<PauseFreezer>();
    }

}
