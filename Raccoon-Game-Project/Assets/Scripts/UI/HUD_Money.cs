using UnityEngine;

public class HUD_Money : MonoBehaviour
{
    const int NUMBER_OF_DIGITS = 3; // "000"
    int displayValue; //this will be ticked over time to match the actual value.
    int actualValue;
    [SerializeField] AudioSource ticker;
    void Start()
    {
        displayValue = SaveManager.GetSave().Money; //prevent ticking when we enter a new scene or start the game.
        ticker = GetComponent<AudioSource>();
        InvokeRepeating(nameof(CheckTickSound), 0, 0.05f);
        actualValue = SaveManager.GetSave().Money;
    }
    void CheckTickSound()
    {
        if (displayValue != actualValue || (displayValue + 1 == actualValue))
        {
            ticker.Play();
        }
    }

    void Update()
    {
        actualValue = SaveManager.GetSave().Money;
        //tick the display value up or down over time.
        if (displayValue > actualValue)
        {
            displayValue--;
        }
        if (displayValue < actualValue)
        {
            displayValue++;
        }

        //print to screen 
        string text = displayValue.ToString();
        text = text.PadLeft(NUMBER_OF_DIGITS, '0'); //pad with 0's (EG: θx044)
        foreach (var x in gameObject.GetComponentsInChildren<TMPro.TMP_Text>()) //we need to tell each of the outline objects to be the same text.
        {
            x.text = text;
        }
    }
}
