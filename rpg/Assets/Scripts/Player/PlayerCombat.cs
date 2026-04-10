using UnityEngine;

public class PlayerCombat : EntityCombat
{
    [Header("Counter attack details")]
    [SerializeField] private float counterDuration;

    public bool CounterAttackPerformed()
    {
        bool hasCounteredSomebody = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable != null)
            {
                counterable.HandleCounter();
                hasCounteredSomebody = true;
            }
        }

        return hasCounteredSomebody;
    }

    public float GetCounterDuration()
    {
        return counterDuration;
    }
}
