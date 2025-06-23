using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackState : PlayerState
{
    private bool hasTouchedGround;
    public PlayerJumpAttackState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        hasTouchedGround = false;
        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDirection, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected && !hasTouchedGround)
        {
            hasTouchedGround = true;
            animator.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.velocity.y);
        }

        if (triggerCalled && player.IsGroundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
