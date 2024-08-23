using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//this is complete poopy butt code
public class UIRPGBoyIntroBar : MonoBehaviour
{
    
    [SerializeField] Sprite emptyEnemySprite;
    [SerializeField] Image enemyImage;
    [SerializeField] GameObject BattleScene;
    RPGable rPGable;
    public void Start()
    {
        GetComponent<Animator>().speed = 0;
    }
    
    public void Play(RPGable foundRPGable)
    {
        rPGable = foundRPGable;
        enemyImage.sprite = foundRPGable ? foundRPGable.UIShockedSprite : emptyEnemySprite;
        GetComponent<Animator>().Play(foundRPGable ? "success" : "fail", -1, 0);
        GetComponent<Animator>().speed = 1;
    }
    public void LoadBattle()
    {
        GameObject go = Instantiate(BattleScene);
        go.GetComponent<BattleScene>().encounter = rPGable;

    }
    public void HideUI()
    {
        GetComponent<Animator>().speed = 0;
        transform.parent.gameObject.SetActive(false);
    }

}
