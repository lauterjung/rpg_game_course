using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public StatMajorGroup major;
    public StatOffenseGroup offense;
    public StatDefenseGroup defense;

    public float strengthDamageMultiplier = 1;
    public float strenghtCritPowerMultiplier = 0.5f;
    public float strengthCritDamageMultiplier = 5;
    public float vitalityHealthMultiplier = 5;
    public float agilityCritChanceMultiplier = 0.5f;
    public float agilityEvasionMultiplier = 0.5f;
    public float evasionCap = 85f;

    public float GetPhysicalDamage(out bool isCritical)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue() * strengthDamageMultiplier;
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * agilityCritChanceMultiplier;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * strenghtCritPowerMultiplier;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        isCritical = Random.Range (0, 100) < critChance;

        float finalDamage = isCritical ? totalBaseDamage * critPower : totalBaseDamage;

        return finalDamage;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * vitalityHealthMultiplier;
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;
        
        return finalMaxHealth;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * agilityEvasionMultiplier;
        float totalEvasion = baseEvasion + bonusEvasion;

        float finalEvasion = Math.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }
}
