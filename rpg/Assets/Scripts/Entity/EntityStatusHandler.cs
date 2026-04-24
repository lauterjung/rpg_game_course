using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity entity;
    private EntityVFX entityVfx;
    private EntityStats entityStats;
    private EntityHealth entityHealth;
    private ElementType currentEffetct = ElementType.None;

    [Header("Electrify effect details")]
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1f;
    private Coroutine electrifyCoroutine;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<EntityVFX>();
        entityStats = GetComponent<EntityStats>();
        entityHealth = GetComponent<EntityHealth>();
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffetct == ElementType.Lightning)
        {
            return true;
        }

        return currentEffetct == ElementType.None;
    }

    public void ApplyElectrifyEffect(float duration, float lightningDamage, float charge)
    {
        float lightningResistance = entityStats.GetElementalResistance(ElementType.Lightning);

        float finalCharge = charge * (1 - lightningResistance);
        currentCharge = currentCharge + finalCharge;

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(lightningDamage);
            StopElectrifyEffect();

            return;
        }

        if (electrifyCoroutine != null)
        {
            StopCoroutine(electrifyCoroutine);
        }

        electrifyCoroutine = StartCoroutine(ElectrifyEffectCoroutine(duration));
    }

    private IEnumerator ElectrifyEffectCoroutine(float duration)
    {
        currentEffetct = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);

        StopElectrifyEffect();
    }

    private void StopElectrifyEffect()
    {
        currentEffetct = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllVfx();
    }

    private void DoLightningStrike(float lightningDamage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(lightningDamage);
    }

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = entityStats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnedEffectCoroutine(duration, finalDamage));
    }

    private IEnumerator BurnedEffectCoroutine(float duration, float totalDamage)
    {
        currentEffetct = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);
        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffetct = ElementType.None;
    }

    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance);

        StartCoroutine(ChilledEffectCoroutine(finalDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCoroutine(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffetct = ElementType.Ice;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffetct = ElementType.None;
    }
}
