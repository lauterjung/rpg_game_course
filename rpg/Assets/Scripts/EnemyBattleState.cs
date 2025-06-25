using System;
using UnityEngine;

public class EnemyBattleState : EnemyState
{
    private Transform Player;
    private float lastTimeWasInBattle;

    public EnemyBattleState(Enemy enemy, StateMachine stateMachine, string animationBoolName) : base(enemy, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (Player == null)
        {
            Player = enemy.PlayerDetected().transform;
        }

        if (ShouldRetreat())
        {
            rb.velocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
        {
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.IdleState);
        }

        if (IsWithinAttackRange() && enemy.PlayerDetected())
        {
            stateMachine.ChangeState(enemy.AttackState);
        }
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.velocity.y);
        }
    }

    private void UpdateBattleTimer()
    {
        lastTimeWasInBattle = Time.time;
    }

    private bool BattleTimeIsOver()
    {
        return Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    }

    private bool IsWithinAttackRange()
    {
        return DistanceToPlayer() < enemy.attackDistance;
    }

    private bool ShouldRetreat()
    {
        return DistanceToPlayer() < enemy.minRetreatDistance;
    }

    private float DistanceToPlayer()
    {
        if (Player == null)
        {
            return float.MaxValue;
        }

        return MathF.Abs(Player.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (Player == null)
        {
            return 0;
        }

        return Player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
