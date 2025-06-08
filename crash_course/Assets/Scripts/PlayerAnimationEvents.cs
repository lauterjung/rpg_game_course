using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void DamageEnemies()
    {
        player.DamageEnemies();
    }

    void DisableMovementAndJump()
    {
        player.EnableMovementAndJump(false);
    }

    void EnableMovementAndJump()
    {
        player.EnableMovementAndJump(true);
    }
}