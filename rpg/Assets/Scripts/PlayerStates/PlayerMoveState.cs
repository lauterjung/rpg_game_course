using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x == 0 || player.IsWallDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        player.SetVelocity(player.MoveInput.x * player.moveSpeed, rb.velocity.y);
    }
}
