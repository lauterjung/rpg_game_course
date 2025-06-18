using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x == player.facingDirection && player.IsWallDetected)
        {
            return;
        }

        if (player.MoveInput.x != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
    }
}
