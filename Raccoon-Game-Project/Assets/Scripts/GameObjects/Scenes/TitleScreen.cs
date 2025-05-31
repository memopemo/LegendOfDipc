using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    bool isDemoMode = false;
    [SerializeField] GameObject messageBox;
    AudioClip select;
    [SerializeField] SceneReference overworld;

    void Start()
    {
    }

    public void LoadSaveAndPlay()
    {
        SaveManager.GetSave();
        PlayTransition();
    }
    public void ShowConfirmationNewSave()
    {
        if (!SaveManager.SaveFileExists())
        {
            NewSaveAndPlay();
            return;
        }
        messageBox.SetActive(true);
    }
    public void NewSaveAndPlay()
    {
        SaveManager.ResetSave();
        PlayTransition();
    }
    void PlayTransition()
    {
        Invoke(nameof(EnterGame), 1);
        FindFirstObjectByType<CircleFadeInUI>().Out();
        FindFirstObjectByType<SongPlayer>().OnSceneChange(overworld);
    }
    public void EnterGame()
    {
        SceneManager.LoadScene(overworld);
        ExitHandler.ExitLoadingSavePoint();
    }
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();        
    }

}
