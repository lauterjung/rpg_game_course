using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : EntityState
{
    public PlayerWallSlideState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        HandleWallSlide();

        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.WallJumpState);
        }

        if (player.IsWallDetected == false)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (player.IsGroundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
            player.Flip();
        }
    }

    private void HandleWallSlide()
    {
        if (player.MoveInput.y < 0)
        {
            player.SetVelocity(player.MoveInput.x, rb.velocity.y);
        }
        else
        {
            player.SetVelocity(player.MoveInput.x, rb.velocity.y * player.wallSlideMoveMultiplier);
        }
    }
}
