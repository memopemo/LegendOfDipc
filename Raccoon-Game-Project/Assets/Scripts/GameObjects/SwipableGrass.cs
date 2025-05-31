using System.Collections.Generic;
using Animator2D;
using UnityEngine;

public class SwipableGrass : MonoBehaviour
{
    [SerializeField] GameObject objectUnderneath;
    [SerializeField] GameObject randomHeart;
    [SerializeField] GameObject randomMoney;
    PoofDestroy poofDestroy;

    SimpleAnimator2D animator2D;

    private void Start()
    {
        animator2D = GetComponent<SimpleAnimator2D>();
        poofDestroy = GetComponent<PoofDestroy>();
    }
    public void OnSwipe()
    {
        if (objectUnderneath)
            Instantiate(objectUnderneath, transform.position, Quaternion.identity);
        if(randomHeart && Random.Range(0,6) == 0)
            Instantiate(randomHeart, transform.position, Quaternion.identity);
        else if(randomMoney && Random.Range(0,6) == 0)
            Instantiate(randomMoney, transform.position, Quaternion.identity);
        poofDestroy.PoofAndDestroy();
    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.TryGetComponent(out PlayerStateManager player))
        {
            animator2D.RestartAnimation();
            GetComponent<NoiseMaker>().Play(0);
        }
        if(collider2D.TryGetComponent(out SwordHitBox _))
        {
            OnSwipe();
        }
    }
}
