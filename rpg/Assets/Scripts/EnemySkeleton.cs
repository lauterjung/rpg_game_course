using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        IdleState = new EnemyIdleState(this, stateMachine, "idle");
        MoveState = new EnemyMoveState(this, stateMachine, "move");
        AttackState = new EnemyAttackState(this, stateMachine, "attack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
    }
}
