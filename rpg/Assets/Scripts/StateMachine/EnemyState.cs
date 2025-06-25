using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(stateMachine, animationBoolName)
    {
        this.enemy = enemy;
        this.animator = enemy.animator;
        this.rb = enemy.rb;
    }

    public override void UpdateAnumationParameters()
    {
        base.UpdateAnumationParameters();

        float battleAnimationSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;

        animator.SetFloat("battleAnimationSpeedMultiplier", battleAnimationSpeedMultiplier);
        animator.SetFloat("moveAnimationSpeedMultiplier", enemy.MoveAnimationSpeedMultiplier);
        animator.SetFloat("xVelocity", rb.velocity.x);    }
}
