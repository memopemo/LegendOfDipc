using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator2D;
using UnityEngine;
using UnityEngine.Events;


public class DungeonDoor : MonoBehaviour
{
    public enum LockType { Switch, Key, BossKey }
    public enum State { Closed, Open }
    public LockType type;
    public State state;
    BoxCollider2D parentThruDoor;
    BoxCollider2D collider;
    [System.NonSerialized] public SimpleAnimator2D simpleAnimator2D;
    public void Start()
    {

        parentThruDoor = transform.parent.GetComponent<BoxCollider2D>();
        collider = GetComponent<BoxCollider2D>();
        simpleAnimator2D = GetComponent<SimpleAnimator2D>();
        int dungeonNumber = GameObjectParser.GetIndexFromString(gameObject.scene.name);
        Dungeon dungeon = SaveManager.GetSave().dungeons[dungeonNumber];
        if (state == State.Closed && (type == LockType.Key || type == LockType.BossKey) && dungeon.DoorsUnlocked[GameObjectParser.GetIndexFromName(gameObject)])
        {
            state = State.Open;
        }

    }
    public void Update()
    {
        if (parentThruDoor && state == State.Open)
        {
            parentThruDoor.enabled = state == State.Open;
        }
        collider.isTrigger = state == State.Open;
        // type + is
        simpleAnimator2D.SetAnimation((int)type * 2 + (int)state);
    }
    public void Interact()
    {
        int dungeonNumber = GameObjectParser.GetIndexFromString(gameObject.scene.name);
        Dungeon dungeon = SaveManager.GetSave().dungeons[dungeonNumber];
        switch (type)
        {
            case LockType.Key:
                if (dungeon.SkeletonKeyObtained)
                {
                    Open();
                    dungeon.DoorsUnlocked[GameObjectParser.GetIndexFromName(gameObject)] = true;
                    break;
                }
                for (int i = 0; i < dungeon.KeyObtained.Length; i++)
                {
                    if (dungeon.KeyObtained[i])
                    {
                        dungeon.KeyObtained[i] = false;
                        dungeon.DoorsUnlocked[GameObjectParser.GetIndexFromName(gameObject)] = true;
                        Open();
                        break;
                    }
                }
                break;
            case LockType.BossKey:
                if (dungeon.BossKeyObtained)
                {
                    dungeon.DoorsUnlocked[GameObjectParser.GetIndexFromName(gameObject)] = true;
                    Open();
                }
                break;
        }
    }
    public void DelayedOpen()
    {
        if (IsInvoking()) return;
        Invoke(nameof(Open), 0.5f);
    }
    public void Open()
    {
        state = State.Open;
        GetComponent<NoiseMaker>().Play(type == LockType.Switch ? 0 : 1);
    }
    public void Close(int lockType)
    {
        if (!IsInvoking())
        {
            GetComponent<NoiseMaker>().Play(2 + ((LockType)lockType == LockType.Switch ? 0 : 1));
        }
        CancelInvoke();
        state = State.Closed;
        type = (LockType)lockType;

    }
    public void Close(LockType lockType)
    {
        if (!IsInvoking())
        {
            GetComponent<NoiseMaker>().Play(2 + (lockType == LockType.Switch ? 0 : 1));
        }
        CancelInvoke();
        state = State.Closed;
        type = lockType;
    }
}
