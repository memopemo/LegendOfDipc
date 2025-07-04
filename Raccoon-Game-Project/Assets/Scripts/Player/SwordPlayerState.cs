using UnityEngine;

public class SwordPlayerState : IPlayerState
{
    Sword.SwingType typeOfSwing;
    Sword.Type typeOfSword;

    int tripleHitCount = 0; //Number of successful triple-hit inputs.
    float swordTimerSecs = 0; //Time to compare against inputs.

    // TODO: Fine tune these variables to get a good feel.
    const float minNextInputSecs = 0.1f;
    const float maxNext3SwipeInputSecs = 0.3f; //must sort of rapid press.
    const float maxNextInputSecs = 0.4f;
    const float maxTotalSwordAnimation = 0.5f;

    const float nudgeSword = 2f;

    const int SWORD_ANIM_OFFSET = 4;
    const int SWORD_SOUND_OFFSET = 6;

    SwordHitBox swordHitBox;

    public void OnEnter(PlayerStateManager manager)
    {
        int swordID = SaveManager.GetSave().CurrentSword;
        if (swordID == -1)
        {
            manager.SwitchState(new DefaultPlayerState());
            return;
        }
        /* Determine type of sword we are using. */
        typeOfSword = (Sword.Type)(SaveManager.GetSave().CurrentSword % 3);

        int anim = SWORD_ANIM_OFFSET;

        if (typeOfSword == 0)
        {
            anim += (int)Sword.SwingType.Stab;
        }
        //both Swing and 3Hit start with the same animation, SWING_FORE = 0, so no need to do anything.
        manager.animator.SetAnimation(anim);
        //NudgeInDirectionToLookCool(manager);
        manager.swordHitBoxObject.SetActive(true);
        swordHitBox = manager.swordHitBoxObject.GetComponent<SwordHitBox>();
        manager.noiseMaker.Play(SWORD_SOUND_OFFSET+(typeOfSword == 0 ? 2 : 0));
        manager.UpdateColors();
    }

    public void OnLeave(PlayerStateManager manager)
    {
        manager.swordHitBoxObject.SetActive(false);
        manager.UpdateColors(true);
    }

    public void OnUpdate(PlayerStateManager manager)
    {
        swordTimerSecs += Time.deltaTime;
        // Check if player waited too long / cant bc of triple swipe.
        if (swordTimerSecs > maxTotalSwordAnimation / (DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Astir) ? 3 : 1))
        {
            manager.SwitchState(new DefaultPlayerState());
        }

        // Enforce triple hit
        // (after triple hit is complete,
        // player is forced to wait until they go back to default state to triple hit again)
        if (!Buttons.IsButtonDown(Buttons.Sword) || tripleHitCount == 2) return;
        if (typeOfSword == Sword.Type.TripleHit)
        {
            if (swordTimerSecs < minNextInputSecs || swordTimerSecs > maxNext3SwipeInputSecs) return;
            tripleHitCount++;
            manager.animator.SetAnimation(SWORD_ANIM_OFFSET + tripleHitCount);
            swordTimerSecs = 0;
            NudgeInDirectionToLookCool(manager);
            swordHitBox.TripleSwipeAgain((Sword.SwingType)tripleHitCount);
            manager.noiseMaker.Play(SWORD_SOUND_OFFSET+tripleHitCount);
        }
        else
        {
            if (swordTimerSecs < maxNextInputSecs) return;
            manager.animator.RestartAnimation();
            swordTimerSecs = 0;
            NudgeInDirectionToLookCool(manager);
        }

    }

    // nudges the player a little bit forward to give some weight to swinging.
    public void NudgeInDirectionToLookCool(PlayerStateManager manager)
    {
        // if (manager.rigidBody.linearVelocity == Vector2.zero) return; //prevent nudging if the player started swinging from a standstill.
        // manager.rigidBody.linearVelocity += (Vector2)manager.directionedObject.direction * nudgeSword;
    }
}
