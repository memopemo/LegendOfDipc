using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public Flasher flasher; //flasher can be on any object with a flashing material.
    public float timeBetweenFlashes;
    bool playerCanInteract;
    public void Update()
    {
        timeBetweenFlashes += Time.deltaTime;
    }
    public void Interact()
    {
        OnInteract.Invoke();
        enabled = false;
    }
    public void PlayerCanInteract()
    {
        if(timeBetweenFlashes > 0.75)
        {
            timeBetweenFlashes = 0;
            flasher.StartFlash();
        }
    }

}
