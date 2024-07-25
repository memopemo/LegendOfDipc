using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Sprite[] itemPreviews;
    [SerializeField] AudioClip[] sounds;
    [SerializeField] AnimationCurve flyIn;
    [SerializeField] AnimationCurve flyOut;
    float timer;
    bool isOn;
    public bool inTransition;
    float extent;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        isOn = false;
        transform.GetChild(0).gameObject.SetActive(false);
        extent = transform.GetChild(0).GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        Transform bg = transform.GetChild(0);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(inTransition) return;
            Invoke(nameof(ToggleBG), isOn ? flyOut.keys.Last().time : flyIn.keys.Last().time - 0.5f);
            isOn = !isOn;
            timer = 0;
        }
        if(isOn)
        {
            if(UIInput.IsUpPressed() || UIInput.IsDownPressed())
            {
                AudioSource.PlayClipAtPoint(sounds[0],FindFirstObjectByType<PlayerStateManager>().transform.position);
            }
            else if(UIInput.IsLeftPressed() || UIInput.IsRightPressed())
            {
                AudioSource.PlayClipAtPoint(sounds[1],FindFirstObjectByType<PlayerStateManager>().transform.position);
            }
            else if(UIInput.IsSwitchPressed())
            {
                AudioSource.PlayClipAtPoint(sounds[2],FindFirstObjectByType<PlayerStateManager>().transform.position);
            }
        }
        timer += Time.deltaTime;
        Vector3 tempPos = bg.localPosition;

        bg.localPosition = new Vector3(tempPos.x, isOn ? flyIn.Evaluate(timer)*extent : flyOut.Evaluate(timer)*extent, tempPos.z);
        inTransition = timer < (isOn ? flyIn.keys.Last().time : flyOut.keys.Last().time); //cooldown

        #if DEBUG
        //Debug Keys
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.DebugMaxOut();

        }
        #endif
    }
    void ToggleBG()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }
}
