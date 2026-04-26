using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    public StatSetupSO defaultStatSetup;

    public StatResourceGroup resources;
    public StatOffenseGroup offense;
    public StatDefenseGroup defense;
    public StatMajorGroup major;

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

        float bonusElementalDamage = major.intelligence.GetValue() * inteligenceElementalDamageMultiplier;
        float highestDamage = GetHighestElementalDamage(fireDamage, iceDamage, lightningDamage, out element);

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        var weakElementalDamageBonus = CalculateWeakerElementsBonus(element, fireDamage, iceDamage, lightningDamage);

        float finalDamage = highestDamage + bonusElementalDamage + weakElementalDamageBonus;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * inteligenceElementalResistanceMultiplier;

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

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorReduction;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.FireDamage: return offense.fireDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;
            case StatType.IceResistance: return defense.iceRes;
            case StatType.FireResistance: return defense.fireRes;
            case StatType.LightningResistance: return defense.lightningRes;

            default:
                Debug.LogWarning($"Stat type {type} not implemented yet.");
                return null;
        }
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned.");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
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

    private float CalculateWeakerElementsBonus(ElementType element, float fireDamage, float iceDamage, float lightningDamage)
    {
        float bonusIce = element == ElementType.Ice ? 0 : iceDamage * elementalDamageContribution;
        float bonusFire = element == ElementType.Fire ? 0 : fireDamage * elementalDamageContribution;
        float bonusLightning = element == ElementType.Lightning ? 0 : lightningDamage * elementalDamageContribution;

        return bonusFire + bonusIce + bonusLightning;
    }
}
