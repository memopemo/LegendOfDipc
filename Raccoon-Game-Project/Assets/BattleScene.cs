using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator2D;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene : MonoBehaviour
{
    public RPGable encounter;
    Animator playerAnim;
    Animator enemyAnim;

    public enum PlayerAction {Attack, ConsumableItem, KeyItem, Defend, Run}
    int itemIndex;
    PlayerAction selectedPlayerAction;

    enum EnemyAction {Attack, Special}
    EnemyAction selectedEnemyAction;
    Stats player;
    Stats enemy;
    

    class RPGEnemy 
    {
        public int health;
        public Status.Effect effectActive;
        public int effectTurnsLeft;
    }
    class Stats
    {
        public int Health;
        public int Attack;
        public int Defense;
        public int Speed;
        public Stats()
        {
            int pendantsCollected = SaveManager.GetSave().Pendants.Count((_)=>true);
            Health = pendantsCollected*10;
            Attack = pendantsCollected*10;
            Defense = pendantsCollected*10;
            Speed = pendantsCollected*10;
        }
        public Stats(RPGable enemy)
        {
            int pendantsCollected = SaveManager.GetSave().Pendants.Count((_)=>true);
            Health = pendantsCollected*10 + enemy.HealthModifier;
            Attack = pendantsCollected*10 + enemy.AttackModifier;
            Defense = pendantsCollected*10 + enemy.DefenseModifier;
            Speed = pendantsCollected*10 + enemy.SpeedModifier;
        }
    }
    RPGEnemy rPGEnemy = new();
    [SerializeField] Animator decisionWindow;
    WaitForSeconds wait = new(0.5f);

    void Start()
    {
        FindFirstObjectByType<CameraFocus>().gameObject.SetActive(false); //disable og camera. Look here.
        enemyAnim = transform.GetChild(1).GetComponent<Animator>();
        playerAnim = transform.GetChild(0).GetComponent<Animator>();
        player = new();
        enemy = new(encounter);
        rPGEnemy.health = enemy.Health;

    }
    public void OnPlayerSelectedAction(PlayerAction playerAction)
    {
        selectedPlayerAction = playerAction;
        selectedEnemyAction = Random.Range(0,3) == 1 ? EnemyAction.Special: EnemyAction.Attack;
        FindFirstObjectByType<RPGSelector>().enabled = false;
        StartCoroutine(nameof(PlayTurn));
    }

    //sequential script, broken up by waiting.
    IEnumerator PlayTurn()
    {
        //Hide Selector Animation.
        decisionWindow.Play("FlyOut");

        yield return wait;

        //Player Goes First or Enemy Goes First.
        if(player.Speed >= enemy.Speed)
        {
            yield return DoPlayerAction();
            yield return DoEnemyAction();
        }
        else
        {
            yield return DoEnemyAction();
            yield return DoPlayerAction();
        }
        yield return wait;

        //End Turn.
        decisionWindow.Play("FlyIn");
        yield return wait;
        FindFirstObjectByType<RPGSelector>().enabled = true;
        enemyAnim.Play("Blank");
        playerAnim.Play("Blank");

        
    }
    IEnumerator DoPlayerAction()
    {
        switch (selectedPlayerAction)
        {
            case PlayerAction.Run:
                Run();
                break;
            default:
                Attack();
            break;
        }
        yield return wait;
        
    }
    
    IEnumerator DoEnemyAction()
    {
        print("Enemy's turn!");
        playerAnim.Play("Slash");
        yield return wait;
    }
    
    void Attack()
    {
        rPGEnemy.health -= (SaveManager.GetSave().CurrentSword+1) * 10 + player.Attack - enemy.Defense;
        print(rPGEnemy.health);
        enemyAnim.Play("Slash");
    }
    void Run()
    {
        ExitBattle();
    }
    void ExitBattle()
    {
        Destroy(gameObject);
        
    }
    
    void OnDestroy()
    {
        FindFirstObjectByType<CameraFocus>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindFirstObjectByType<Canvas>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FreezeMmanager.UnfreezeAll<PauseFreezer>();
    }
    
}
