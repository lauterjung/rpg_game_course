using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 3.5f;
    [SerializeField] float jumpForce = 8f;

    [Header("Collision details")]
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    [Header("Attack details")]
    [SerializeField] float attackRadius;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask whatIsEnemy;


    [Header("Other")]

    bool isGrounded;

    bool canMove = true;
    bool canJump = true;
    float xInput;
    bool facingRight = true;

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleFlip();
        HandleAnimations();
    }

    public void DamageEnemies()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsEnemy);

        foreach (Collider2D enemy in enemyColliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage();
        }
    }

    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryToAttack();
        }
    }

    private void HandleMovement()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void TryToAttack()
    {
        if (isGrounded)
        {
            animator.SetTrigger("attack");
        }
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleFlip()
    {
        if ((rb.velocity.x > 0 && !facingRight) || (rb.velocity.x < 0 && facingRight))
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
