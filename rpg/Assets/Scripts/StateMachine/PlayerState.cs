using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;

    public PlayerState(Player player, StateMachine stateMachine, string animationBoolName) : base(stateMachine, animationBoolName)
    {
        this.player = player;
        
        animator = player.animator;
        rb = player.rb;
        input = player.input;
        entityStats = player.entityStats;

    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private bool CanDash()
    {
        if (stateMachine.currentState == player.dashState || player.IsWallDetected)
        {
            return false;
        }

        return true;
    }
}
