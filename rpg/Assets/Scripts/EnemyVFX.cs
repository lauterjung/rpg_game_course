using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFX : EntityVFX
{
    [Header("Counter attack window")]
    [SerializeField] private GameObject attackAlert;

    public void EnableAttackAlert (bool enable)
    {
        attackAlert.SetActive(enable);
    }
}
