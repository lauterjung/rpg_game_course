using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationTriggers : MonoBehaviour
{
    Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void CurrentStateTrigger()
    {
        entity.CallAnimationTrigger();
    }
}
