using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class LanguageManager
{
    static JObject currentLanguage;
    public static string playerPrefKey = "lang";

    public static JObject GetLanguage()
    {
        if(currentLanguage == null)
        {
            currentLanguage = LoadLanguage();
        }
        return currentLanguage;
    }

    public static JObject LoadLanguage()
    {
        string lang = PlayerPrefs.GetString(playerPrefKey, "en");
        TextAsset langFile = Resources.Load($"lang_{lang}") as TextAsset;
        return JsonConvert.DeserializeObject<JObject>(langFile.text);
    }
    public static void SwitchLanguage(string lang)
    {
        PlayerPrefs.SetString(playerPrefKey, lang);
        PlayerPrefs.Save();
        currentLanguage = LoadLanguage();
    }
}
