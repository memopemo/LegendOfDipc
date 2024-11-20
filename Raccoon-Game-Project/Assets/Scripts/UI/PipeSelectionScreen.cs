using UnityEngine;

public class UIPipeSelection : MonoBehaviour
{
    public GameObject previouslySelected;

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
            SelectPipe(GameObjectParser.GetIndexFromName(go));
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

    }
    void SelectPipe(int index)
    {
        FindFirstObjectByType<CircleFadeInUI>().Out();

        Invoke(nameof(Warp), 1f);
    }
    void Warp()
    {

    }
}
