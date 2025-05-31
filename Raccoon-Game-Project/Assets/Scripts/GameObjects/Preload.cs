using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(1);
        //be the first to load save
        SaveManager.GetSave();

    }
}
