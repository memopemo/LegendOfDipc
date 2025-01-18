using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPipeSelection : MonoBehaviour
{
    public GameObject previouslySelected;
    int selection;
    public bool holdDrainExit;

    public void OnButtonPush(GameObject go)
    {
        if (!previouslySelected)
        {
            previouslySelected = go;
            //enable text on new one.
            go.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (go == previouslySelected)
        {
            selection = GameObjectParser.GetIndexFromName(go);
            SelectPipe();
        }
        else
        {
            //disable text on previous one
            previouslySelected.transform.GetChild(0).gameObject.SetActive(false);
            previouslySelected = go;
            //enable text on new one.
            go.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        holdDrainExit = true;
    }
    public void Cancel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        holdDrainExit = false;

    }
    void SelectPipe()
    {

        FindFirstObjectByType<CircleFadeInUI>().Out();

        Invoke(nameof(Warp), 1f);
    }
    void Warp()
    {
        ExitHandler.ExitViaPipe(selection);
        SceneManager.LoadScene("SampleScene");
    }
}
