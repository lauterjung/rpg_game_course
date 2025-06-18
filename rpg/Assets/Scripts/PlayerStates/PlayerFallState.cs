using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAiredState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.IsWallDetected)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }
}
