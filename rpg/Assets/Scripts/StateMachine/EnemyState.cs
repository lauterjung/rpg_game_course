using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(stateMachine, animationBoolName)
    {
        this.enemy = enemy;
        
        animator = enemy.animator;
        rb = enemy.rb;
        entityStats = enemy.entityStats;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        float battleAnimationSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;

        animator.SetFloat("battleAnimationSpeedMultiplier", battleAnimationSpeedMultiplier);
        animator.SetFloat("moveAnimationSpeedMultiplier", enemy.moveAnimationSpeedMultiplier);
        animator.SetFloat("xVelocity", rb.velocity.x);    }
}
