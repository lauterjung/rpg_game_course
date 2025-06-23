using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiredState : PlayerState
{
    public PlayerAiredState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x != 0)
        {
            player.SetVelocity(player.MoveInput.x * player.moveSpeed * player.inAirMoveMultiplier, rb.velocity.y);
        }

        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.JumpAttackState);
        }
    }
}
