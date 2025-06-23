using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleState : EnemyState
{
    public EnemyBattleState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(enemy, stateMachine, animationBoolName)
    {
    }
}
