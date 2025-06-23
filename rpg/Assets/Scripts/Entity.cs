using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }

    private bool isFacingRight = true;
    public int facingDirection { get; private set; } = 1;


    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool IsGroundDetected { get; private set; }
    public bool IsWallDetected { get; private set; }


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start() { }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
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
        IsGroundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondaryWallCheck != null)
        {
            IsWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        }
        else
        {
            IsWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
        
        if (secondaryWallCheck != null)
        {
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
        }
    }
}
