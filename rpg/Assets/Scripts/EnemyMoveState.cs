using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(enemy, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.IsGroundDetected == false || enemy.IsWallDetected)
        {
            enemy.Flip();
        }
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, rb.velocity.y);

        if (enemy.IsGroundDetected == false || enemy.IsWallDetected)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}