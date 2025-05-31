using UnityEngine;
using System;
using System.Text;
using TMPro;
using Newtonsoft.Json.Linq;

public class UIToolTipText : MonoBehaviour
{
    [SerializeField] string path;
    [NonSerialized] public string textResult;
    TooltipContext tt;
    [SerializeField] TMP_FontAsset emergencySmallFont;
    void Start()
    {
        if (path == "") return;
        JObject jObject = LanguageManager.LoadLanguage();
        JToken textResult = jObject.SelectToken(path);
        tt = textResult.ToObject<TooltipContext>();   
    }
    void Update()
    {
        TMP_Text textBox = GetComponent<TMP_Text>();
        textBox.text = tt.ToString();

        //incase the text wont fit, use a smaller font.
        if(textBox.isTextOverflowing)
        {
            textBox.font = emergencySmallFont;
            textBox.fontSize = 8;
        }
    }

}

[Serializable]
    public class TooltipContext
    {
        [Serializable]
        public class TooltipButton
        {
            public Button button;
            public string action;
        }
        public TooltipButton[] tooltips;
        public TooltipContext(int numButtons)
        {
            tooltips = new TooltipButton[numButtons];
        }
        override public string ToString()
        {
            StringBuilder stringBuilder = new();
            foreach (var item in tooltips)
            {
                stringBuilder.Append(Buttons.isUsingButtons ? item.button.ControllerName : item.button.KeyName);
                stringBuilder.Append(":");
                stringBuilder.Append(item.action);
                stringBuilder.Append("  ");
            }
            return stringBuilder.ToString();
        }

    }
