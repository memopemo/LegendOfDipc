using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for best effectiveness, put this in the first scene loaded by Unity. (title screen)
public class Music : MonoBehaviour
{
    AudioSource audioSource;
    void Awake()
    {
        DontDestroyOnLoad(gameObject); //always active
        audioSource = GetComponent<AudioSource>();
    }
}
