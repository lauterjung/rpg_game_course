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

        counteredSomebody = false;
        animator.SetBool("counterAttackPerformed", false);
        stateTimer = combat.GetCounterDuration();
    }

    public override void Update()
    {
        base.Update();

        if (combat.CounterAttackPerformed())
        {
            counteredSomebody = true;
            animator.SetBool("counterAttackPerformed", true);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer < 0 && !counteredSomebody)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}