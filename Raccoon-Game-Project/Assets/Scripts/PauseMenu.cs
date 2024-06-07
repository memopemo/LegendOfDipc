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
                if (!transform.GetChild(0).gameObject.activeSelf)
                {
                    FreezeMmanager.FreezeAll<PauseFreezer>();
                }
                else
                {
                    FreezeMmanager.UnfreezeAll<PauseFreezer>();
                }
                transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            }

        }
        #if DEBUG
        //Debug pausing without pause screen
        if(Input.GetKeyDown(KeyCode.Pause))
        {
            FreezeMmanager.FreezeAll<PauseFreezer>();
        }
        if(Input.GetKeyUp(KeyCode.Pause))
        {
            FreezeMmanager.UnfreezeAll<PauseFreezer>();
        }
        #endif
    }
    public void ButtonUnpause()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        FreezeMmanager.UnfreezeAll<PauseFreezer>();
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
