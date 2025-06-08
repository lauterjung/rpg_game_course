using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public string enemyName;

    public void TakeDamage() { }
    protected void Move() { }
    protected virtual void Attack() { }
}
