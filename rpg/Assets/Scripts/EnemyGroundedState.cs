using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedState : EnemyState
{
    public EnemyGroundedState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(enemy, stateMachine, animationBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() == true)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
}
