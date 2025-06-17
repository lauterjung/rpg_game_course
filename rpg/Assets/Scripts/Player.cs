using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: better player air movement
// TODO: compare states. When let go off wall still playing wall animation state
public class Player : MonoBehaviour
{
    public PlayerInputSet input { get; private set; }
    private StateMachine stateMachine;
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }

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

    private bool isFacingRight = true;
    public int facingDirection { get; private set; } = 1;
    public Vector2 MoveInput { get; private set; }

    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool IsGroundDetected { get; private set; }
    public bool IsWallDetected { get; private set; }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        stateMachine = new StateMachine();
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

    private void Start()
    {
        stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
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

    public void CallAnimationTrigger()
    {
        stateMachine.CurrentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && !isFacingRight || xVelocity < 0 && isFacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        facingDirection *= -1;
    }

    private void HandleCollisionDetection()
    {
        IsGroundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        IsWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
    }
}
