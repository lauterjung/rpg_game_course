using System.Collections;
using UnityEngine;

// TODO: better player air movement
public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerBasicAttackState BasicAttackState { get; private set; }
    public PlayerJumpAttackState JumpAttackState { get; private set; }


    [Header("Attack details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1f;
    private Coroutine queuedAttackCoroutine;


    [Header("Movement details")]
    public float moveSpeed = 8f;
    public float jumpForce = 5f;
    public Vector2 wallJumpForce = new(8f, 12f);
    [Range(0, 1)]
    public float inAirMoveMultiplier = 0.7f;
    [Range(0, 1)]
    public float wallSlideMoveMultiplier = 0.7f;
    [Space]
    public float dashDuration = 0.25f;
    public float dashSoeed = 20f;
    public Vector2 MoveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();


        IdleState = new PlayerIdleState(this, stateMachine, "idle");
        MoveState = new PlayerMoveState(this, stateMachine, "move");
        JumpState = new PlayerJumpState(this, stateMachine, "jumpFall");
        FallState = new PlayerFallState(this, stateMachine, "jumpFall");
        WallSlideState = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, stateMachine, "jumpFall");
        DashState = new PlayerDashState(this, stateMachine, "dash");
        BasicAttackState = new PlayerBasicAttackState(this, stateMachine, "basicAttack");
        JumpAttackState = new PlayerJumpAttackState(this, stateMachine, "jumpAttack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCoroutine != null)
        {
            StopCoroutine(queuedAttackCoroutine);
        }

        queuedAttackCoroutine = StartCoroutine(EnterAttackStateWithDelayCoroutine());
    }

    private IEnumerator EnterAttackStateWithDelayCoroutine()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(BasicAttackState);
    }
}
