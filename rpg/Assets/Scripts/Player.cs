using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet input;
    private StateMachine stateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public Vector2 MoveInput { get; private set; }

    void Awake()
    {
        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        IdleState = new PlayerIdleState(this, stateMachine, "idleState");
        MoveState = new PlayerMoveState(this, stateMachine, "moveState");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void OnDisable() {
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        stateMachine.UpdateActiveState();
    }
}
