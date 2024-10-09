using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator2D;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene : MonoBehaviour
{
    public RPGable RPGableEnemy; //the enco
    Animator playerAnim;
    Animator enemyAnim;

    public enum PlayerAction { Attack, ConsumableItem, KeyItem, Defend, Run }
    PlayerAction currentPlayerAction;
    public int itemIndex; //can work for indexes of Consumable Items and Key Items.
    

    enum EnemyAction { Attack, Special }
    EnemyAction selectedEnemyAction;

    //stats are what is kept track of by the enemy and player.
    public class Stats
    {
        public int Health; //this is modified and if it reaches 0, the player/enemy dies.
        public int MaxHealth;
        public int Attack;
        public int Defense;
        public int Speed; //speed determines who goes first.

        //TODO: add a level table (indexed by number of demon keys obtained) instead of calculation.
        public Stats()
        {
            int totalDemonKeysCollected = SaveManager.GetSave().DemonKeys.Count((_) => true);
            MaxHealth = totalDemonKeysCollected * 10; //10 is completely arbitrary
            //get fraction of health to max health, then multiply that by the calculated max health to get our scaled current health.
            int gameMaxHealth = (SaveManager.GetSave().HeartContainersCollected.Count((_)=>true) + PlayerHealth.STARTING_HEART_CONTAINERS) * 2;
            float fraction = (float)PlayerHealth.currentHealth / gameMaxHealth;
            Health = Mathf.RoundToInt(fraction * MaxHealth); 
            Attack = totalDemonKeysCollected * 10;
            Defense = totalDemonKeysCollected * 10;
            Speed = totalDemonKeysCollected * 10;
        }

        //Enemy Stats match player's skill level.
        //TODO: ALLOW WEAKER ENEMYS UPON BACKTRACKING.
        public Stats(RPGable enemy)
        {
            int pendantsCollected = SaveManager.GetSave().Pendants.Count((_) => true);
            Health = pendantsCollected * 10 + enemy.HealthModifier;
            Attack = pendantsCollected * 10 + enemy.AttackModifier;
            Defense = pendantsCollected * 10 + enemy.DefenseModifier;
            Speed = pendantsCollected * 10 + enemy.SpeedModifier;
        }
    }
    public Stats player; //player tries to kill enemy.
    public Stats enemy; //enemy tries to kill player.
    [SerializeField] Animator decisionWindow; //decision window flys in and out.
    WaitForSeconds wait = new(0.5f); //dont have to make new shit all the time.

    void Start()
    {
        FindFirstObjectByType<CameraFocus>().gameObject.SetActive(false); //disable og camera. Look here. This saves having to render the game thats not on-screen at all.
        enemyAnim = transform.GetChild(1).GetComponent<Animator>();
        playerAnim = transform.GetChild(0).GetComponent<Animator>();
        player = new();
        enemy = new(RPGableEnemy);

    }

    /*  sequential script, broken up by waiting.
        This will execute over time.
    */
    public void OnPlayerSelectedAction(PlayerAction action)
    {
        currentPlayerAction = action;
        StartCoroutine(nameof(PlayerSelectedAction));
    }
    public IEnumerator PlayerSelectedAction()
    {
        //enemy selects move to do.
        selectedEnemyAction = Random.Range(0, 3) == 1 ? EnemyAction.Special : EnemyAction.Attack;

        //Hide Selector Animation.
        decisionWindow.Play("FlyOut");
        FindFirstObjectByType<RPGSelector>().enabled = false;

        yield return wait;

        //Begin Turn---------------------------------------------

        //Player Goes First or Enemy Goes First.
        if (player.Speed >= enemy.Speed)
        {
            yield return DoPlayerAction(currentPlayerAction);
            yield return new WaitForSeconds(0.5f);
            yield return DoEnemyAction();
        }
        else
        {
            yield return DoEnemyAction();
            yield return new WaitForSeconds(0.5f);
            yield return DoPlayerAction(currentPlayerAction);
        }
       yield return new WaitForSeconds(0.5f);

        //End Of Turn.--------------------------------------------
        decisionWindow.Play("FlyIn");

        yield return wait;

        FindFirstObjectByType<RPGSelector>().enabled = true;
        enemyAnim.Play("Blank");
        playerAnim.Play("Blank");

    }

    IEnumerator DoPlayerAction(PlayerAction action)
    {
        switch (action)
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
        player.Health -= enemy.Attack - (player.Defense - (SaveManager.GetSave().CurrentArmor * 10));
        print(player.Health);
        yield return wait;
    }

    void Attack()
    {
        enemyAnim.Play("Slash");
        enemy.Health -= (SaveManager.GetSave().CurrentSword + 1) * 10 + player.Attack - enemy.Defense; //calculate damage
        print(enemy.Health);

    }
    void Run()
    {
        ExitBattle();
    }
    void ExitBattle()
    {
        //by simply destroying this object, the camera automatically returns to the topdown camera.
        //This camera has a higher priority.
        Destroy(gameObject); 
    }

    void OnDestroy()
    {
        //allow gameplay to resume.
        FindFirstObjectByType<CameraFocus>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindFirstObjectByType<Canvas>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FreezeMmanager.UnfreezeAll<PauseFreezer>(); //this was set by the RPGBoy
    }

}
