using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity entity;
    private EntityVFX entityVfx;
    private EntityStats stats;
    private ElementType currentEffetct = ElementType.None;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    public bool CanBeApplied()
    {
        return currentEffetct == ElementType.None;
    }

    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);

        StartCoroutine(ChilledEffectCoroutine(reducedDuration, slowMultiplier));
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
