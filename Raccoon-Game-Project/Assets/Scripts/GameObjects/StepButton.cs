using System.Collections;
using System.Collections.Generic;
using Animator2D;
using UnityEngine;

//Your Button's new husband's button. Sometimes gets stuck.
public class StepButton : MonoBehaviour
{
    [SerializeField] private bool isStuck; //once pressed, will not unpress.
    Animator2D.SimpleAnimator2D animator;
    Switch switcher;
    NoiseMaker noiseMaker;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator2D.SimpleAnimator2D>();
        switcher = GetComponent<Switch>();
        noiseMaker = GetComponent<NoiseMaker>();
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
            noiseMaker.Play(0);
            switcher.ActivateSwitchA(1);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(isStuck) return; //stuck
        if (collider.TryGetComponent(out PlayerStateManager _))
        {
            noiseMaker.Play(1);
            GetComponent<Switch>().DeactivateSwitchA();
        }
    }
}
