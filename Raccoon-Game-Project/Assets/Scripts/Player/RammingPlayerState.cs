using UnityEngine;

// Dash forward and move a little bit until you crash into something.
public class RammingPlayerState : IPlayerState
{
    const float WINDUP_SECS = 1.28f;
    float windupTimeAndSpeed = -WINDUP_SECS; //- is windup, + is speed.
    const int WINDUP_ANIM = 30;
    const int DASH_ANIM = 31;
    const int WINDUP_SOUND = 2;
    const int DASH_SOUND = 3;
    const int DASH_STEP_SOUND = 4;
    const float MAX_DASH_SPEED = CommonPlayerState.DEFAULT_SPEED * 4;

    private const int RAM_DAMAGE = 5;
    private const int DASH_PARTICLE = 0;
    DamagesEnemy damagesEnemy;
    int framesImpactingCount;
    const int FRAMES_IMPACTING = 4;

    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(WINDUP_ANIM);
        damagesEnemy = manager.gameObject.AddComponent<DamagesEnemy>();
        damagesEnemy.amount = RAM_DAMAGE;
        damagesEnemy.isBuffed = true;
        manager.noiseMaker.Play(WINDUP_SOUND);
    }
    public void OnLeave(PlayerStateManager manager)
    {
        Object.Destroy(damagesEnemy);
        manager.noiseMaker.StopInteruptableClip(false);

    }
    public void OnUpdate(PlayerStateManager manager)
    {
        if (!Buttons.IsButtonHeld(Buttons.KeyItem) && isWindingUp())
        {
            manager.SwitchState(new DefaultPlayerState());
            manager.noiseMaker.StopInteruptableClip(false);
            return;
        }
        else if (!isWindingUp() && Buttons.IsButtonDown(Buttons.KeyItem))
        {
            manager.SwitchState(new DefaultPlayerState());
            return;
        }

        //play dash start sound and looping step sound.
        if (isWindingUp() && windupTimeAndSpeed + Time.deltaTime > 0) //play only on the switch from windup to running.
        {
            Object.Instantiate(manager.poofParticle, manager.transform.position, Quaternion.identity); //create poof particle
            manager.noiseMaker.Play(DASH_SOUND); //zoom!
            manager.noiseMaker.Play(DASH_STEP_SOUND);
        }

        windupTimeAndSpeed += Time.deltaTime;
        //freeze if we are impacting.
        if (isFreezeImpacting())
        {
            //movement, but no direction
            manager.rigidBody.linearVelocity = (Vector2)manager.directionedObject.direction
                * (!isWindingUp() ? MAX_DASH_SPEED : windupTimeAndSpeed + manager.additionalSpeed);
        }
        else
        {
            framesImpactingCount--;
            manager.rigidBody.linearVelocity = Vector2.zero;
        }

        // allow strafing perpendicular
        if (manager.directionedObject.direction.x != 0) //if x direction is active...
        {
            manager.rigidBody.linearVelocity += new Vector2(0, manager.rawInput.y); //then use input y.
        }
        else //use input x.
        {
            manager.rigidBody.linearVelocity += new Vector2(manager.rawInput.x, 0);
        }

        manager.animator.SetAnimation(isWindingUp() ? WINDUP_ANIM : DASH_ANIM); //animation

        if (isWindingUp()) return; //skip next sections if we are still winding up. 

        if (Time.frameCount % 5 == 0)
        {
            GameObject particle = manager.GetComponent<ParticleMaker>().CreateParticle(DASH_PARTICLE);
            particle.transform.SetPositionAndRotation(
                particle.transform.position + Vector3.down * 0.5f,
                Quaternion.Euler(0, 0, Rotation.DirectionToAngle((Vector2)manager.directionedObject.direction) - 90));
        }

        if (Time.frameCount % 2 == 0) return; //Don't ram into shit until we are actually running. 
                                              // Also only do this every other fame.


        //check for hitting something.
        /* Our Box Cast is SPECIFIC:
         * We want to be able to graze solid objects and not stop, yet we also want to *actually slam into them if we are directly hitting them.*
         * There would be a potential case where we are running against a wall where our circle collider does not slide us off the edge, yet it doesnt detect anything.
         * Using a small box collider instead of a point protects us from this. (edge wont work since it could skip over the wall's edge collider.)
         */
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)manager.transform.position + Vector2.down * 0.2f, Vector2.one * 0.4f, 0, (Vector2)manager.directionedObject.direction, 1f);
        
        Physics2D.queriesHitTriggers = false;
        foreach (var hit in hits)
        {
            //Important
            if (hit.collider.gameObject == manager.gameObject) continue; //ignore player
            if (hit.collider.gameObject.TryGetComponent(out Hittable hittable))
            {
                hittable.OnHit(damagesEnemy);
                if (!hittable.TryGetComponent(out Knockbackable _))
                {
                    manager.SwitchState(new BonkPlayerState());
                }
            }
            else if (hit.collider.gameObject.TryGetComponent(out Rammable rammable))
            {
                rammable.OnRamInto();
                //if its collidable but not solid (aka breakable)
                if (!hit.collider.isTrigger)
                {
                    framesImpactingCount = FRAMES_IMPACTING; //slow us down a peg.
                }
                if (rammable.isSolid)
                {
                    manager.SwitchState(new BonkPlayerState());
                    return;
                }
            }
            else if (!hit.collider.isTrigger) //we have hit a brick wall.
            {
                //todo: add a Slam into wall effect.
                manager.SwitchState(new BonkPlayerState());
                return;
            }
        }
        manager.waterCheck.Evaluate<Water>((_) => manager.SwitchState(new SwimPlayerState())); //No dashing over water!!! 

    }

    bool isWindingUp() => windupTimeAndSpeed < 0;
    bool isFreezeImpacting() => framesImpactingCount >= 0;
}
