using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public UnityEvent OnFlashAction;
    float timeBetweenFlashes;
    bool playerCanInteract;
    public void Update()
    {
        timeBetweenFlashes += Time.deltaTime;
    }
    public void Interact()
    {
        OnInteract.Invoke();
    }
    public void PlayerCanInteract()
    {
        if (timeBetweenFlashes > 0.75)
        {
            timeBetweenFlashes = 0;
            OnFlashAction.Invoke();
        }
    }

}
