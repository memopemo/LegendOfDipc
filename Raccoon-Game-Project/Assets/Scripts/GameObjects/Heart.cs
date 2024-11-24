using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Heightable))]
public class Heart : MonoBehaviour
{
    [SerializeField] AnimationCurve bounce;
    float time;
    Vector2 direction;
    bool onFallDone; //bool for only doing things once
    Bouncing bouncing;
    // Start is called before the first frame update
    void Start()
    {
        direction = Random.onUnitSphere;
        bouncing = GetComponent<Bouncing>();
    }

    // Update is called once per frame
    void Update()
    {
        var player = FindAnyObjectByType<PlayerStateManager>();
        if (!player) return;
        if (onFallDone)
        {
            if (Random.Range(1, 100) == 37)
            {
                GetComponent<Animator2D.Animator2D>().RestartAnimation();
            }
            return;
        }

        transform.position += (Vector3)direction * Time.deltaTime * 3;

        if (bouncing.IsBounceDone())
        {
            GetComponent<Animator2D.Animator2D>().SetAnimation(Random.Range(1, 3));
            direction = Vector2.zero;
            onFallDone = true;
        }


    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (!onFallDone) return;
        if (collider.TryGetComponent(out PlayerStateManager _) || collider.TryGetComponent(out SwordHitBox _))
        {
            gameObject.SetActive(false);
            PlayerHealth.Heal(4, FindFirstObjectByType<PlayerStateManager>());
        }
        else if (collider.TryGetComponent(out Boomerang b))
        {
            transform.parent = b.transform;
        }
    }
}
