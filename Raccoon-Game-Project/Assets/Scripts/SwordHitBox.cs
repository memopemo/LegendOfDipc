using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        boxCollider2D.enabled = false;
        //enforce delay and only continue once delay is finished.
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
            return;
        }
        boxCollider2D.enabled = true;

    }
    private void OnEnable()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        //-1 = no equipped sword.
        if (SaveManager.GetSave().CurrentSword == -1) return;

        //Set delay
        delayTime = HIT_DELAY;

        //set box width depending on sword swing type.
        int swordType = SaveManager.GetSave().CurrentSword % 3; //types of swords are always in order: stab = 0, swing = 1, triple = 2. 
        float width = swordType == 0 ? stabBoxWidth : swipeBoxWidth; //both swing and triple always start with swipe.
        boxCollider2D.size = new Vector2(width, boxCollider2D.size.y);

        //add Kurt Angle to the mix
        float angle = KurtAngle.GetValueOrDefault(transform.parent.GetComponent<DirectionedObject>().direction, 0);
        transform.eulerAngles = new Vector3(0, 0, angle);


    }
    public void TripleSwipeAgain(int swipeType) //we check the swipe cause triple has both swipe and stab.
    {

        if (swipeType == 2)
        {
            boxCollider2D.size = new Vector2(stabBoxWidth, boxCollider2D.size.y);
        }
        //set box width depending on sword swing type.
        else
        {
            boxCollider2D.size = new Vector2(swipeBoxWidth, boxCollider2D.size.y);
        }
    }
    private void OnDisable()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
