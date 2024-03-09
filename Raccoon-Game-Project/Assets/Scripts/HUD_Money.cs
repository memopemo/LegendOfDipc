using UnityEngine;

public class HUD_Money : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var x in gameObject.GetComponentsInChildren<TMPro.TMP_Text>())
        {
            x.text = SaveManager.GetSave().Money.ToString();
        }
    }
}
