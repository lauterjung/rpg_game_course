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

        if (rb.velocity.y < 0 && player.IsGroundDetected == false)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (input.Player.Jump.WasPressedThisFrame() && player.IsGroundDetected)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        
        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.BasicAttackState);
        }
    }
}
