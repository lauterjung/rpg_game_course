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
        this.animator = player.animator;
        this.rb = player.rb;
        this.input = player.input;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void UpdateAnumationParameters()
    {
        base.UpdateAnumationParameters();

        animator.SetFloat("yVelocity", rb.velocity.y);
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
