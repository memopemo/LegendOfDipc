using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGboy : MonoBehaviour
{
    [SerializeField] GameObject combatScreen;
    RPGable rPGable;
    GameObject battleScreen;
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
        Invoke(nameof(StartBattle), 0.75f);
        FreezeMmanager.FreezeAll<PauseFreezer>();
    }
    void StartBattle()
    {
        battleScreen = Instantiate(combatScreen, FindFirstObjectByType<Canvas>().transform);
        battleScreen.GetComponent<BattleSceneStarter>().rPGable = rPGable;
        Invoke(nameof(TempEscapeBattle), 2);
    }
        
    void TempEscapeBattle()
    {
        FreezeMmanager.UnfreezeAll<PauseFreezer>();
        Destroy(gameObject);
        Destroy(battleScreen);
    }
}
