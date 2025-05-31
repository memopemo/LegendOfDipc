using System;
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
    int volIndex;
    public static readonly string[] exposedNames = { "MasterParam", "EffectsParam", "MusicParam" };
    public static int[] volumes = {-80, -65, -51, -39, -29 ,-20, -13, -7, -3, -1, 0}; //precalculated parabola for f(x) = (x-10^2)/-1.25

    // Start is called before the first frame update
    void Awake()
    {
        volIndex = PlayerPrefs.GetInt(exposedNames[index], volIndex);
        audioMixer.SetFloat(exposedNames[index], volumes[volIndex] );
    }
    void Start()
    {
        index = GameObjectParser.GetIndexFromName(gameObject);
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        volIndex = PlayerPrefs.GetInt(exposedNames[index], volIndex);
        rectTransform.sizeDelta = new Vector2(volIndex * 8, 8);
    }
    public void IncreaseVolume()
    {
        volIndex++;
        SaveVolume();

    }
    public void DecreaseVolume()
    {
        volIndex--;
        SaveVolume();
    }

    private void SaveVolume()
    {
        volIndex = Mathf.Clamp(volIndex, 0, 10);
        audioMixer.SetFloat(exposedNames[index], volumes[volIndex] );
        PlayerPrefs.SetInt(exposedNames[index], volIndex);
        PlayerPrefs.Save();
    }
}
