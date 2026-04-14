using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private PlayerCombat combat;
    private bool counteredSomebody;

    public PlayerCounterAttackState(Player player, StateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
        combat = player.GetComponent<PlayerCombat>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = combat.GetCounterRecoveryDuration();
        counteredSomebody = combat.CounterAttackPerformed();

        animator.SetBool("counterAttackPerformed", counteredSomebody);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.velocity.y);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && !counteredSomebody)
            stateMachine.ChangeState(player.idleState);
    }
}