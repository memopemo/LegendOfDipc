using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    bool isDemoMode = false;
    EventSystem eventSystem;
    [SerializeField] GameObject messageBox;
    AudioClip select;

    void Start()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();
        print(eventSystem);
        if (!SaveManager.SaveFileExists())
        {
            GameObject.Find("Load").GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void LoadSaveAndPlay()
    {
        eventSystem.enabled = false;
        SaveManager.GetSave();
        Invoke(nameof(EnterGame), 1);
        FindFirstObjectByType<CircleFadeInUI>().Out();

    }
    public void ShowConfirmationNewSave()
    {
        if (!SaveManager.SaveFileExists())
        {
            NewSaveAndPlay();
            return;
        }
        messageBox.SetActive(true);
        eventSystem.SetSelectedGameObject(messageBox.transform.GetChild(0).gameObject, new BaseEventData(eventSystem));
    }
    public void CancelConfirmationNewSave()
    {
        messageBox.SetActive(false);
        eventSystem.SetSelectedGameObject(GameObject.Find("New"));

    }
    public void NewSaveAndPlay()
    {
        eventSystem.enabled = false;
        SaveManager.ResetSave();
        Invoke(nameof(EnterGame), 1);
        FindFirstObjectByType<CircleFadeInUI>().Out();
    }
    public void EnterGame()
    {
        SceneManager.LoadScene(2);
        ExitHandler.ExitLoadingSavePoint();
    }

}
