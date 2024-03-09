using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject SettingsPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingsPanel.activeSelf)
            {
                SettingsPanel.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            }

        }
    }
    public void ButtonUnpause()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void ButtonSettings()
    {
        SettingsPanel.SetActive(true);
        //toggle settings panel.
        //disable escape closing the pause screen, should only close settings screen first, *then* the pause menu.
    }
    public void ButtonTitle()
    {
        Application.Quit();
    }
}
