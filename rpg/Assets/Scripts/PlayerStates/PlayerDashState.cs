using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private int dashDirection;
    private float originalGravityScale;

    public PlayerDashState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dashDirection = player.MoveInput.x != 0 ? (int)player.MoveInput.x : player.facingDirection;
        stateTimer = player.dashDuration;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.dashSoeed * dashDirection, 0);

        if (stateTimer < 0)
        {
            if (player.IsGroundDetected == false)
            {
                stateMachine.ChangeState(player.FallState);
            }

            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }

    private void CancelDashIfNeeded()
    {
        if (player.IsWallDetected)
        {
            if (player.IsGroundDetected == false)
            {
                stateMachine.ChangeState(player.FallState);
            }

            stateMachine.ChangeState(player.IdleState);
        }
    }
}
