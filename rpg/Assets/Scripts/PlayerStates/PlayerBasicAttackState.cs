using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttackState : PlayerState
{
    private float lastTimeAttacked;
    private float attackVelocityTimer;

    private bool comboAttackQueued;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private int attackDirection;
    private const int FirstComboIndex = 1;



    public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();

        attackDirection = player.MoveInput.x != 0 ? (int)player.MoveInput.x : player.facingDirection;

        animator.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        // detect and deal damage

        if (triggerCalled)
        {
            HandleStateExit();
        }
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            animator.SetBool(animationBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
    }

    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > lastTimeAttacked + player.comboResetTime || comboIndex > comboLimit)
        {
            comboIndex = FirstComboIndex;
        }
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.velocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDirection, attackVelocity.y);
    }
}
