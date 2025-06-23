using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyIdleState IdleState;
    public EnemyMoveState MoveState;
    public EnemyAttackState AttackState;
    [Range(0, 2)]
    public float MoveAnimationSpeedMultiplier = 1f;

    [Header("Movement details")]
    public float idleTime = 2f;
    public float moveSpeed = 1.4f;
}
