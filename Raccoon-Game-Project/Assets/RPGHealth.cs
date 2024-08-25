using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RPGHealth : MonoBehaviour
{
    int maxHealth;
    TMP_Text text;
    BattleScene battleScene;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        battleScene = FindFirstObjectByType<BattleScene>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $":{battleScene.player.Health}/{battleScene.player.MaxHealth}";
    }
}
