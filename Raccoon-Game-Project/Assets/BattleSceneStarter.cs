using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneStarter : MonoBehaviour
{
    Animator animator;
    [SerializeField] Sprite emptyEnemySprite;
    public RPGable rPGable;
    public void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        Image enemyImage = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        enemyImage.sprite = rPGable ? rPGable.UIShockedSprite : emptyEnemySprite;
        print(enemyImage.sprite);
        animator.Play(rPGable ? "success" : "fail");
        //Tell the main ui to show in like 2 seconds.
        Destroy(gameObject, 3);
    }
    
}
