using System;
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
    [SerializeField] bool isStartRoom = false;
    [NonSerialized] public CutsceneMaker cutsceneMaker;
    void Start()
    {
        OnLoad.AddListener(LoadRoomObjectsAuto);
        OnUnload.AddListener(UnloadRoomObjectsAuto);
        (isStartRoom ? OnLoad : OnUnload).Invoke();
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
    public void LoadRoomObjectsAuto()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(true);
        }
    }
    public void UnloadRoomObjectsAuto()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
    }


}
