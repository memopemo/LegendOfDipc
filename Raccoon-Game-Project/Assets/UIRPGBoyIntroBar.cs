using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*  this *WAS* complete poopy butt code, but only because i didnt want to use the animator.
    I think unity's animator is only fine for my uses when im not doing anything procedural with the animation itself.
    And also for like one-time hits, rather than building a whole library of animations for up/down/left/right.
    My frustrations with unity's animator is that it has always felt inefficient/clunky to use.
    Its hella more tailored to moving/rotating positional children (like a bone structure) than sprite animations.
    Its made more in mind for humanoid models/3D rigged models than anything.
    ALSO: its a fucking pain in the absolute ***ASS*** to iterate on anything other than the animation itself.
        Want to move a child somewhere else in the tree? Fuck you. Its missing.
        Want to "NOT" have to make three "AnimEnter" "AnimIdle" "AnimExit" animations and instead have a "Start loop here" and "Loop Point here" and allow a simple intro for the loop? Impossible.
        Want to apply animations to two different objects with differing names? Fuck you. Missing.
        Want to rename an object? Fuck you. Missing.
        Want to "NOT" use strings to play animations? Fuck you. Make the most batshit insane web of transitions that you have to test *every* transition for to make sure nothing fucked up.
    ALSO ALSO: STOP. MAKING. ME. USE. STRINGS. TO. CALL. ANIMATIONS. THIS SUCKS. 
    tldr; i have a love/hate relationship with unity, which is fair. but unity for the love of god make a Sprite Animator thats easy to use.
*/  
public class UIRPGBoyIntroBar : MonoBehaviour
{
    
    [SerializeField] Sprite emptyEnemySprite;
    [SerializeField] Image enemyImage;
    [SerializeField] GameObject BattleScene;
    RPGable rPGable; //intra-class reference.
    public void Start()
    {
        //this is the only way i can get it to stop without making a whole animation/graph for stopping/starting.
        GetComponent<Animator>().speed = 0;
    }
    
    public void Play(RPGable foundRPGable)
    {
        rPGable = foundRPGable;
        enemyImage.sprite = foundRPGable ? foundRPGable.UIShockedSprite : emptyEnemySprite; //if found, show enemy's shocked sprite.
        GetComponent<Animator>().Play(foundRPGable ? "success" : "fail", -1, 0); //"oh, theres nothing here" or "YOU!!"
        GetComponent<Animator>().speed = 1;
    }

    //Called by animation.
    public void LoadBattle()
    {
        GameObject go = Instantiate(BattleScene);
        go.GetComponent<BattleScene>().RPGableEnemy = rPGable;

    }
    
    //Called by animation. We cant hide the UI when the battle is loaded because then the white will fade out too soon.
    public void HideUI()
    {
        GetComponent<Animator>().speed = 0;
        transform.parent.gameObject.SetActive(false); //disable canvas.
    }

}
