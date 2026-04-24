using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    public StatResourceGroup resources;
    public StatMajorGroup major;
    public StatOffenseGroup offense;
    public StatDefenseGroup defense;

    // TODO: all percent or all 0-1
    public float strengthDamageMultiplier = 1f;
    public float strenghtCritPowerMultiplier = 0.5f;
    public float strengthCritDamageMultiplier = 5f;
    public float vitalityHealthMultiplier = 5f;
    public float vitalityArmorMultiplier = 1f;
    public float agilityCritChanceMultiplier = 0.5f;
    public float agilityEvasionMultiplier = 0.5f;
    public float inteligenceElementalDamageMultiplier = 1f;
    public float inteligenceElementalResistanceMultiplier = 0.5f;
    public float elementalDamageContribution = 0.5f;
    public float evasionCap = 85f;
    public float armorMitigationFactor = 100f;
    public float armorMitigationCap = 0.85f;
    public float elementalResistanceCap = 75f;

    public float GetPhysicalDamage(out bool isCritical, float scaleFactor = 1f)
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

        isCritical = Random.Range(0, 100) < critChance;

        float finalDamage = isCritical ? totalBaseDamage * critPower : totalBaseDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1f)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();

        float bonusElementalDamage = major.inteligence.GetValue() * inteligenceElementalDamageMultiplier;
        float highestDamage = GetHighestElementalDamage(fireDamage, iceDamage, lightningDamage, out element);

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        var weakElementalDamageBonus = CalculateWeakerElementsBonus(highestDamage, fireDamage, iceDamage, lightningDamage);

        float finalDamage = highestDamage + bonusElementalDamage + weakElementalDamageBonus;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.inteligence.GetValue() * inteligenceElementalResistanceMultiplier;

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;

            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;

            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float finalResistance = Mathf.Clamp(resistance, 0, elementalResistanceCap) / 100;

        return finalResistance;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue() * vitalityArmorMultiplier;
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + armorMitigationFactor);
        float finalMitigation = Math.Clamp(mitigation, 0, armorMitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = resources.maxHealth.GetValue();
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

    private float GetHighestElementalDamage(float fireDamage, float iceDamage, float lightningDamage, out ElementType element)
    {
        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        return highestDamage;
    }

    private float CalculateWeakerElementsBonus(float highestDamage, float fireDamage, float iceDamage, float lightningDamage)
    {
        // TODO: tied damage
        float bonusFire = fireDamage == highestDamage ? 0 : fireDamage * elementalDamageContribution;
        float bonusIce = iceDamage == highestDamage ? 0 : iceDamage * elementalDamageContribution;
        float bonusLightning = lightningDamage == highestDamage ? 0 : lightningDamage * elementalDamageContribution;

        return bonusFire + bonusIce + bonusLightning;
    }

}
