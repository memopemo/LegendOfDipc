using UnityEngine;
using Newtonsoft.Json.Linq;
using TMPro;
using System;

public class UILanguagedText : MonoBehaviour
{
    [SerializeField] string path; //update this to change text eg: "settingsText.audioSettings.title"
    [NonSerialized] public string textResult;
    [SerializeField] TMP_FontAsset emergencySmallFont;
    void Start()
    {
        if (path == "") return;
        textResult = LanguageManager.GetLanguage().SelectToken(path).Value<string>();
    }
    public void SetPath(string path)
    {
        this.path = path;
        textResult = LanguageManager.GetLanguage().SelectToken(path).Value<string>();
    }
    void Update()
    {
        TMP_Text textBox = GetComponent<TMP_Text>();
        textBox.text = textResult;

        //incase the text wont fit, change to a smaller font.
        if(textBox.isTextOverflowing)
        {
            textBox.font = emergencySmallFont;
            textBox.fontSize = 8;
        }
        
    }
}
