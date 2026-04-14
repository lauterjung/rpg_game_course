using UnityEngine;

public class PlayerCombat : EntityCombat
{
    [Header("Counter attack details")]
    [SerializeField] private float counterRecovery = 0.1f;

    public bool CounterAttackPerformed()
    {
        bool performedCounter = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable == null)
                continue;

            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                performedCounter = true;
            }
        }

        return performedCounter;
    }

    public float GetCounterRecoveryDuration()
    {
        return counterRecovery;
    }
}
