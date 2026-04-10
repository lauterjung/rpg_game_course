using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    private Collider2D collider;
    public EnemyDeadState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(enemy, stateMachine, animationBoolName)
    {
        collider = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        animator.enabled = false;
        collider.enabled = false;

        rb.gravityScale = 12;
        rb.velocity = new Vector2(rb.velocity.x, 15);

        stateMachine.SwitchOffStateMachine();
    }
}
