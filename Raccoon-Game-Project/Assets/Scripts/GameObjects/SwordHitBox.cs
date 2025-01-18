using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwordHitBox : MonoBehaviour
{
    readonly Dictionary<Vector2Int, float> KurtAngle = new()
    {
        { Vector2Int.down, 0 },
        { Vector2Int.up, 180 },
        { Vector2Int.left, -90 },
        { Vector2Int.right, 90 }
    };
    const float stabBoxWidth = 0.5f;
    float swipeBoxWidth = 2f;
    float delayTime;
    const float HIT_DELAY = 0f;
    BoxCollider2D boxCollider2D;
    bool oneFrameDelay;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // boxCollider2D.enabled = false;
        // //enforce delay and only continue once delay is finished.
        // if (delayTime > 0)
        // {
        //     delayTime -= Time.deltaTime;
        //     return;
        // }
        // boxCollider2D.enabled = true;
        if (oneFrameDelay)
        {
            boxCollider2D.enabled = false;
            oneFrameDelay = false;
        }
        else
        {
            boxCollider2D.enabled = true;
        }
    }
    private void OnEnable()
    {
        oneFrameDelay = true;
        boxCollider2D = GetComponent<BoxCollider2D>();
        //-1 = no equipped sword.
        if (SaveManager.GetSave().CurrentSword == -1) return;

        //Set delay
        delayTime = HIT_DELAY;
        int sword = SaveManager.GetSave().CurrentSword;
        //set box width depending on sword swing type.
        Sword.Type swordType = (Sword.Type)(sword % 3); //types of swords are always in order: stab = 0, swing = 1, triple = 2. 
        float width = swordType == Sword.Type.Stab ? stabBoxWidth : swipeBoxWidth; //both swing and triple always start with swipe.
        boxCollider2D.size = new Vector2(width, boxCollider2D.size.y);

        //add Kurt Angle to the mix
        float angle = KurtAngle.GetValueOrDefault(transform.parent.GetComponent<DirectionedObject>().direction, 0);
        transform.eulerAngles = new Vector3(0, 0, angle);

        GetComponent<DamagesEnemy>().amount = (sword / 3) + 1; //for instance, sword 0 does 1 damage.
        if (DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Trap))
        {
            GetComponent<DamagesEnemy>().knockBack = false;
        }
        else
        {
            GetComponent<DamagesEnemy>().knockBack = swordType != Sword.Type.TripleHit;
        }
    }
    public void TripleSwipeAgain(Sword.SwingType swipeType) //we check the swipe cause triple has both swipe and stab.
    {
        if (swipeType == Sword.SwingType.Stab)
        {
            boxCollider2D.size = new Vector2(stabBoxWidth, boxCollider2D.size.y);
        }
        //set box width depending on sword swing type.
        else
        {
            boxCollider2D.size = new Vector2(swipeBoxWidth, boxCollider2D.size.y);
        }
        if (DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Trap))
        {
            GetComponent<DamagesEnemy>().knockBack = false; //trap will set enemy's knockback to only apply after a fixed amount of time after the first hit.
        }
        else
        {
            GetComponent<DamagesEnemy>().knockBack = swipeType == Sword.SwingType.Stab; //otherwise, only enable knockback when
        }

        oneFrameDelay = true;
    }
    private void OnDisable()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
public class Sword
{
    public enum Type { Stab, Swing, TripleHit };
    public enum SwingType { Forehand, Backhand, Stab };
}
