using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animator2D;

public class DungeonThruDoor : MonoBehaviour
{
    [SerializeField] DungeonRoom dom; //right, up, positive
    [SerializeField] DungeonRoom sub; // left, down, negative
    DungeonDoor domDoor;
    DungeonDoor subDoor;
    public enum Orientation { Horizontal, Vertical }
    [SerializeField] Orientation orientation;

    void Start()
    {
        domDoor = transform.Find("Dom").GetComponent<DungeonDoor>();
        subDoor = transform.Find("Sub").GetComponent<DungeonDoor>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //determine (with 100% certainty) which direction the player went into this trigger in.
        //this is to prevent any bugs with 
        if (!collision.TryGetComponent(out PlayerStateManager player)) return;

        CameraFocus camera = FindFirstObjectByType<CameraFocus>();
        float dir = orientation == Orientation.Horizontal ? player.rigidBody.linearVelocityX : player.rigidBody.linearVelocityY;

        DungeonRoom entering = dir > 0 ? dom : sub; //direction is negative so that means we are facing left/down
        DungeonRoom leaving = dir < 0 ? dom : sub;

        if (player.currentPlayerState is RammingPlayerState)
        {
            entering.OnLoad.Invoke();
            leaving.OnUnload.Invoke();
            entering.StartPotentialCutscene();
            camera.ChangeBounds(entering.Bounds());
        }
        else
        {
            StartCoroutine(RoomTransition(entering, leaving, player, camera));
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 171f / 255f, 209f / 255f);
        Gizmos.DrawLine(transform.position, sub.transform.position);
        Gizmos.color = new Color(56 / 255f, 79 / 255f, 1f);
        Gizmos.DrawLine(transform.position, dom.transform.position);
    }
    public IEnumerator RoomTransition(DungeonRoom entering, DungeonRoom leaving, PlayerStateManager player, CameraFocus camera)
    {
        //Load room before entering
        entering.OnLoad.Invoke();
        GetComponent<NoiseMaker>().Play(0);
        //run anim
        player.SetAnimation(1);
        float time = 0;
        if (entering == dom && domDoor.state == DungeonDoor.State.Closed)
        {
            domDoor.state = DungeonDoor.State.Open; //silent open
            entering.cutsceneMaker.AddTemporaryCutsceneAction(0.5f);
            entering.cutsceneMaker.AddTemporaryCutsceneAction(CloseDomDoor, 0.5f);
        }
        else if (entering == sub && subDoor.state == DungeonDoor.State.Closed)
        {
            subDoor.state = DungeonDoor.State.Open; //silent open
            entering.cutsceneMaker.AddTemporaryCutsceneAction(0.5f);
            entering.cutsceneMaker.AddTemporaryCutsceneAction(CloseSubDoor, 0.5f);
        }


        FreezeManager.FreezeAll<PauseFreezer>(); //cutscene freezer doesnt work here
        Inventory inventory = FindFirstObjectByType<Inventory>();
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        inventory.enabled = pauseMenu.enabled = false;

        var exitDirectionVector = orientation switch
        {
            Orientation.Vertical when entering == sub => Vector3.down,
            Orientation.Vertical when entering == dom => Vector3.up,
            Orientation.Horizontal when entering == sub => Vector3.left,
            Orientation.Horizontal when entering == dom => Vector3.right,
            _ => Vector3.zero,
        };
        while (time < 0.5f)
        {
            player.transform.position += 3.5f * Time.deltaTime * exitDirectionVector;
            time += Time.deltaTime;
            yield return null;
        }
        camera.ChangeBounds(entering.Bounds());
        while (time < 1)
        {
            player.transform.position += 10.5f * Time.deltaTime * exitDirectionVector;
            time += Time.deltaTime;
            yield return null;
        }
        leaving.OnUnload.Invoke();
        entering.StartPotentialCutscene();

        FreezeManager.UnfreezeAll<PauseFreezer>();
        inventory.enabled = pauseMenu.enabled = true;

    }
    public void CloseSubDoor()
    {
        subDoor.Close(subDoor.type);
    }
    public void CloseDomDoor()
    {
        domDoor.Close(domDoor.type);
    }


}
