using UnityEditor;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject DebugPanel;
    UISelectable initialSelection;
    // Start is called before the first frame update
    void Start()
    {
        initialSelection = FindFirstObjectByType<UISelectionPage>(FindObjectsInactive.Include).currentlySelected;
    }

    // Update is called once per frame
    void Update()
    {
        if (Buttons.IsButtonUp(Buttons.Pause))
        {
            if (SettingsPanel.activeSelf)
            {
                return;
            }
            else if (DebugPanel.activeSelf)
            {
                DebugPanel.SetActive(false);
            }
            else
            {
                if (!transform.GetChild(0).gameObject.activeSelf)
                {
                    FreezeManager.FreezeAll<PauseFreezer>();
                    FindFirstObjectByType<Inventory>().enabled = false;
                }
                else
                {
                    FreezeManager.UnfreezeAll<PauseFreezer>();
                    FindFirstObjectByType<Inventory>().enabled = true;
                }
                transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);

            }

        }
#if DEBUG
        //Debug pausing without pause screen
        if (Input.GetKeyDown(KeyCode.Pause))
        {
            FreezeManager.FreezeAll<PauseFreezer>();
        }
        if (Input.GetKeyUp(KeyCode.Pause))
        {
            FreezeManager.UnfreezeAll<PauseFreezer>();
        }
#endif
    }
    public void ButtonUnpause()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        FreezeManager.UnfreezeAll<PauseFreezer>();
        FindFirstObjectByType<Inventory>().enabled = true;
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
    public void ButtonDebug()
    {
#if DEBUG
        DebugPanel.SetActive(true);
#endif
    }
}
