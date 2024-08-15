using System.Collections.Generic;
using Animator2D;
using UnityEngine;

public class SwipableGrass : MonoBehaviour
{
    [SerializeField] GameObject objectUnderneath;
    [SerializeField] GameObject particle;
    SimpleAnimator2D animator2D;

    private void Start()
    {
        animator2D = GetComponent<SimpleAnimator2D>();
    }
    public void OnSwipe()
    {
        if (objectUnderneath)
            Instantiate(objectUnderneath, transform.position, Quaternion.identity);
        if (particle)
            Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        enabled = false;
    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.TryGetComponent(out PlayerStateManager player))
        {
            animator2D.RestartAnimation();
        }
        if(collider2D.TryGetComponent(out SwordHitBox _))
        {
            OnSwipe();
        }
    }
}
