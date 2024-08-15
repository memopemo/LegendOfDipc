using UnityEngine;

public class HUD_Money : MonoBehaviour
{
    const int NUMBER_OF_DIGITS = 3; // "000"
    int displayValue; //this will be ticked over time to match the actual value.
    [SerializeField] AudioClip ticker;
    void Start()
    {
        displayValue = SaveManager.GetSave().Money; //prevent ticking when we enter a new scene or start the game.
    }

    void Update()
    {
        int actualValue = SaveManager.GetSave().Money;
        //Play Tick Sound if ticking and on every 3rd frame.
        if(Time.frameCount % 3 == 0 && displayValue != actualValue)
        {
            AudioSource.PlayClipAtPoint(ticker,FindFirstObjectByType<CameraFocus>().transform.position);
        }

        //tick the display value up or down over time.
        if(displayValue > actualValue )
        {
            displayValue--;
        }
        if(displayValue < actualValue)
        {
            displayValue++;
        }

        //print to screen 
        string text = displayValue.ToString();
        text = text.PadLeft(NUMBER_OF_DIGITS,'0'); //pad with 0's (EG: θx044)
        foreach (var x in gameObject.GetComponentsInChildren<TMPro.TMP_Text>()) //we need to tell each of the outline objects to be the same text.
        {
            x.text = text;
        }
    }
}
