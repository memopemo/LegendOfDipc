using UnityEngine;
using System.Linq;

public class DemonBuffMenu : MonoBehaviour
{
    [SerializeField] AnimationCurve flyIn;
    [SerializeField] AnimationCurve flyOut;
    [SerializeField] AudioClip[] sounds;
    [SerializeField] Transform cameraTarget;
    float timer;
    float extent;
    bool isOn;
    bool inTransition;
    public void Start()
    {
        isOn = false;
        transform.GetChild(0).gameObject.SetActive(false);
        extent = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
    }
    // Update is called once per frame
    void Update()
    {

        Transform bg = transform.GetChild(0);

        if (isOn)
        {
            if (UIInput.IsUpPressed || UIInput.IsDownPressed)
            {
                AudioSource.PlayClipAtPoint(sounds[0], FindFirstObjectByType<Camera>().transform.position + Vector3.back);
            }
            else if (UIInput.IsLeftPressed || UIInput.IsRightPressed)
            {
                AudioSource.PlayClipAtPoint(sounds[1], FindFirstObjectByType<Camera>().transform.position + Vector3.back);
            }
            else if (UIInput.IsConfirmPressed)
            {
                AudioSource.PlayClipAtPoint(sounds[2], FindFirstObjectByType<Camera>().transform.position + Vector3.back);
            }
        }
        timer += Time.deltaTime;
        Vector3 tempPos = bg.localPosition;

        bg.localPosition = new Vector3(isOn ? flyIn.Evaluate(timer) * extent : flyOut.Evaluate(timer) * extent, tempPos.y, tempPos.z);
        inTransition = timer < (isOn ? flyIn.keys.Last().time : flyOut.keys.Last().time); //cooldown
    }
    void ToggleBG()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);

    }
    void Pause()
    {
        FreezeManager.FreezeAll<CutSceneFreezer>();
    }
    public void Toggle()
    {
        if (inTransition) return;
        Invoke(nameof(ToggleBG), isOn ? flyOut.keys.Last().time : flyIn.keys.Last().time - 0.5f);
        isOn = !isOn;
        timer = 0;
        if (isOn)
        {
            FindFirstObjectByType<CameraFocus>().SetTarget(cameraTarget);
            FindFirstObjectByType<PauseMenu>().enabled = FindFirstObjectByType<Inventory>().enabled = false;
            FindFirstObjectByType<DemonBuffSelector>(FindObjectsInactive.Include).enabled ^= true; //flip
            Invoke(nameof(Pause), 0.5f);
        }
        else
        {
            FreezeManager.UnfreezeAll<CutSceneFreezer>();
            FindFirstObjectByType<CameraFocus>().SetTarget(FindFirstObjectByType<PlayerStateManager>().transform);
            FindFirstObjectByType<PauseMenu>().enabled = FindFirstObjectByType<Inventory>().enabled = true;
            FindFirstObjectByType<DemonBuffSelector>(FindObjectsInactive.Include).enabled ^= true; //flip
        }
    }
}
