using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Repeater : MonoBehaviour
{
    [SerializeField] float repeatRate;
    [SerializeField] UnityEvent action;

    void Start()
    {
        InvokeRepeating(nameof(Invoker), 0, repeatRate);   
    }
    public void Invoker()
    {
        action.Invoke();
    }
}
