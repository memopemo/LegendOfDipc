using UnityEngine;
public class BonkPlayerState : IPlayerState
{
    float jumpSecs;
    const float MAX_JUMP_TIME = 0.5f; 
    const int BONK_ANIM = 32;
    private const int JUMP_HEIGHT = 1;
    private const int BONK_PARTICLE = 1;
    private const int BONK_SOUND = 1;

    public void OnEnter(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(BONK_ANIM);
        manager.GetComponent<ParticleMaker>().CreateParticle(BONK_PARTICLE).transform.rotation = Quaternion.Euler(0,0,Rotation.DirectionToAngle((Vector2)manager.directionedObject.direction)-90);  ;
        manager.noiseMaker.Play(BONK_SOUND);
    }

    public void OnLeave(PlayerStateManager manager){}

    public void OnUpdate(PlayerStateManager manager)
    {
        jumpSecs += Time.deltaTime;

        manager.rigidBody.linearVelocity = -manager.directionedObject.direction*5; //move back slightly

        manager.height.height = ((float)(-4 * (float)JUMP_HEIGHT / (MAX_JUMP_TIME * MAX_JUMP_TIME)) * jumpSecs * jumpSecs) + ((float)(4 * (float)JUMP_HEIGHT / MAX_JUMP_TIME) * jumpSecs);

        //Check if done
        if (jumpSecs > MAX_JUMP_TIME) 
        {
            //Leave
            manager.SwitchState(new DefaultPlayerState());
            manager.height.height = 0;
        }
    }
}