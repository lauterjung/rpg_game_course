using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : EntityState
{
    public PlayerGroundedState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (input.Player.Jump.WasPressedThisFrame() && player.IsGrounded)
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }
}
