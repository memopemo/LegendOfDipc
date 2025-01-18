using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class UIAudioVolume : MonoBehaviour
{
    int index;
    [SerializeField] AudioMixer audioMixer;
    RectTransform rectTransform;
    int volAsInt;
    public static readonly string[] exposedNames = { "MasterParam", "EffectsParam", "MusicParam" };
    // Start is called before the first frame update
    void Start()
    {
        index = GameObjectParser.GetIndexFromName(gameObject);
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        audioMixer.GetFloat(exposedNames[index], out float tempVolume);
        volAsInt = Mathf.RoundToInt(tempVolume) / 10;
        volAsInt += 10;
        rectTransform.sizeDelta = new Vector2(volAsInt * 8, 8);
    }
    public void IncreaseVolume()
    {
        volAsInt++;
        SaveVolume();

    }
    public void DecreaseVolume()
    {
        volAsInt--;
        SaveVolume();
    }

    private void SaveVolume()
    {
        volAsInt = Mathf.Clamp(volAsInt, 0, 10);
        volAsInt = (volAsInt - 10) * 10;
        audioMixer.SetFloat(exposedNames[index], volAsInt);
        PlayerPrefs.SetFloat(exposedNames[index], volAsInt);
        PlayerPrefs.Save();
    }
}
