using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroppedBomb : MonoBehaviour
{
    [SerializeField] AnimationCurve dropAnimation;
    float timer;
    Heightable heightable;
    PoofDestroy poof;
    // Start is called before the first frame update
    void Start()
    {
        heightable = GetComponent<Heightable>();
        poof = GetComponent<PoofDestroy>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > dropAnimation.keys.Last().time)
        {
            poof.Poof();
            Destroy(gameObject);
        }
        heightable.height = dropAnimation.Evaluate(timer)*Drone.HEIGHT;
    }
}
