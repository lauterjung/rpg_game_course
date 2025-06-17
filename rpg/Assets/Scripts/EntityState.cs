using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected StateMachine stateMachine;
    protected string animationBoolName;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(Player player, StateMachine stateMachine, string animationBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animationBoolName = animationBoolName;
        this.animator = player.animator;
        this.rb = player.rb;
        this.input = player.input;
    }

    protected EntityState(StateMachine stateMachine, string stateName)
    {
        this.stateMachine = stateMachine;
        this.animationBoolName = stateName;
    }

    public virtual void Enter()
    {
        animator.SetBool(animationBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public virtual void Exit()
    {
        animator.SetBool(animationBoolName, false);
    }

    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }

    private bool CanDash()
    {
        if (stateMachine.CurrentState == player.DashState || player.IsWallDetected)
        {
            return false;
        }

        return true;
    }
}
