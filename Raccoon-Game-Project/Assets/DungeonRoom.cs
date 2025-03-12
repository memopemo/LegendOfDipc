using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CutsceneMaker))]
public class DungeonRoom : MonoBehaviour
{
    public UnityEvent OnLoad; //called before room transition starts (loading enemies, gameobjects, lighting, etc)
    public UnityEvent OnUnload; //called after room transition to new room (unloading enemies, etc)
    public UnityEvent OnKillAllEnemies;
    public int enemiesRemaining;
    public CutsceneMaker cutsceneMaker;
    void Start()
    {
        cutsceneMaker = GetComponent<CutsceneMaker>();
    }
    public void StartPotentialCutscene()
    {
        GetComponent<CutsceneMaker>().StartCutscene();
    }
    public void EnemyKilled()
    {
        if (enemiesRemaining < 0) return;
        enemiesRemaining -= 1;
        if (enemiesRemaining == 0) OnKillAllEnemies.Invoke();
    }
    public Collider2D Bounds() => GetComponent<Collider2D>();


}
