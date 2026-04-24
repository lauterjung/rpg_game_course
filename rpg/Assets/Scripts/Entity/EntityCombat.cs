using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    private EntityStats stats;
    private EntityVFX vfx;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("Status effect details")]
    [SerializeField] private float defaultStatusDuration = 3;
    [SerializeField] private float chillSlowMultiplier = 0.2f;
    [SerializeField] private float electrifyChargeBuildUp = 0.4f;
    [Space]
    [SerializeField] private float fireScale = 0.4f;
    [SerializeField] private float iceScale = 0.4f;
    [SerializeField] private float lightningScale = 2.5f;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        vfx = GetComponent<EntityVFX>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;

            float damage = stats.GetPhysicalDamage(out bool isCrit);
            float elementalDamage = stats.GetElementalDamage(out ElementType element, 0.6f);
            bool targetWasHit = damageable.TakeDamage(damage, elementalDamage, element, transform);

            if (targetWasHit)
            {
                vfx.UpdateOnHitColor(element);
                vfx.CreateOnHitVfx(target.transform, isCrit);

                if (element != ElementType.None)
                    ApplyStatusEffect(target.transform, element);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1f)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

        if (statusHandler == null)
            return;

        if (!statusHandler.CanBeApplied(element))
            return;

        switch (element)
        {
            case ElementType.Ice:
                scaleFactor = iceScale;
                statusHandler.ApplyChillEffect(defaultStatusDuration, chillSlowMultiplier * scaleFactor);
                break;

            case ElementType.Fire:
                scaleFactor = fireScale;
                float fireDamage = stats.offense.fireDamage.GetValue() * scaleFactor;
                statusHandler.ApplyBurnEffect(defaultStatusDuration, fireDamage);
                break;

            case ElementType.Lightning:
                scaleFactor = lightningScale;
                float lightningDamage = stats.offense.lightningDamage.GetValue() * scaleFactor;
                statusHandler.ApplyElectrifyEffect(defaultStatusDuration, lightningDamage, electrifyChargeBuildUp);
                break;
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
