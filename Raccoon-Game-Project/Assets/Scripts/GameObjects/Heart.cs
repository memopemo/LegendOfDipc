using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Heightable))]
public class Heart : MonoBehaviour
{
    [SerializeField] AnimationCurve bounce;
    float time;
    Vector2 direction;
    bool doneFalling;
    // Start is called before the first frame update
    void Start()
    {
        direction = Random.onUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        var player = FindAnyObjectByType<PlayerStateManager>();
        if(!player) return;
        if (doneFalling)
        {
            if (Random.Range(1, 100) == 37)
            {
                GetComponent<Animator2D.Animator2D>().RestartAnimation();
            }
            return;
        }

        GetComponent<Heightable>().height = bounce.Evaluate(time);
        time += Time.deltaTime;
        transform.position += (Vector3)direction * Time.deltaTime*3;

        if(time > 0.55f)
        {
            GetComponent<Animator2D.Animator2D>().SetAnimation(Random.Range(1,3));
            direction = Vector2.zero;
            doneFalling = true;
        }
        
        
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if(!doneFalling) return;
        if(collider.TryGetComponent(out PlayerStateManager _) || collider.TryGetComponent(out SwordHitBox _))
        {
            gameObject.SetActive(false);
            PlayerHealth.Heal(4, FindFirstObjectByType<PlayerStateManager>());
        }
        else if(collider.TryGetComponent(out Boomerang b))
        {
            transform.parent = b.transform;
        }
    }
}
