using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Mario Galaxy-like general purpose switches for everything from buttons opening doors to enemy kill counters. You can even chain switches together.
public class Switch : MonoBehaviour
{
    public int switchA;
    public int switchB;
    [SerializeField] UnityEvent<int> OnActivateA;
    [SerializeField] UnityEvent<int> OnActivateB;
    [SerializeField] UnityEvent OnDeactivateA;
    [SerializeField] UnityEvent OnDeactivateB;
    [SerializeField] UnityEvent OnDead;
    [SerializeField] UnityEvent OnAwake;
    [SerializeField] UnityEvent OnAppear;
    
    void Start()
    {
    }
    public void ActivateOnAwake()
    {
        OnAwake.Invoke();
    }
    public void ActivateOnAppear()
    {
        OnAppear.Invoke();
    }
    public void ActivateOnDead()
    {
        OnDead.Invoke();
    }
    public void ActivateSwitchA(int code)
    {
        switchA = code;
        OnActivateA.Invoke(code);
    }
    public void DeactivateSwitchA()
    {
        switchA = 0;
        OnDeactivateA.Invoke();
    }
    public void ActivateSwitchB(int code)
    {
        switchB = code;
        OnActivateB.Invoke(code);
    }
    public void DeactivateSwitchB()
    {
        switchB = 0;
        OnDeactivateB.Invoke();
    }
    public void ToggleA(int code)
    {
        if(switchA == 0)
        {
            ActivateSwitchA(switchA>0?0:code);
        }
        else
        {
            DeactivateSwitchA();
        }
    }

}
