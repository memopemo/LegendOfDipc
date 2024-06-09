using System.Collections;
using System.Collections.Generic;
using Animator2D;
using UnityEngine;

//Your Button's new husband's button. Sometimes gets stuck.
public class StepButton : MonoBehaviour
{
    [SerializeField] private bool isStuck; //once pressed, will not unpress.
    Animator2D.Animator2D animator;
    Switch switcher;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator2D.Animator2D>();
        switcher = GetComponent<Switch>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetAnimation(switcher.switchA);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PlayerStateManager _))
        {
            switcher.ActivateSwitchA(1);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(isStuck) return; //stuck
        if (collider.TryGetComponent(out PlayerStateManager _))
        {
            GetComponent<Switch>().DeactivateSwitchA();
        }
    }
}
