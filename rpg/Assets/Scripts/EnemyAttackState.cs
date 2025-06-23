using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(enemy, stateMachine, animationBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
