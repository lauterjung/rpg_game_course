using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private bool needsRecalculation = true;
    private float finalValue;

    public float GetValue()
    {
        if (needsRecalculation)
        {
            finalValue = GetFinalValue();
            needsRecalculation = false;
        }

        return finalValue;
    }

    public void AddModifier(float value, string source)
    {
        StatModifier modifierToAdd = new StatModifier(value, source);
        modifiers.Add(modifierToAdd);
        needsRecalculation = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(x => x.source == source);
        needsRecalculation = true;
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.value;
        }

        return finalValue;
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}