using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator animator => GetComponentInChildren<Animator>();
    private EntityVFX vfx => GetComponentInChildren<EntityVFX>();

    [Header("Open Details")]
    [SerializeField] private Vector2 knockback = new Vector2(0, 3);

    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        vfx?.PlayOnDamageVfx();
        animator.SetBool("chestOpen", true);
        
        rb.velocity = knockback;
        rb.angularVelocity = Random.Range(-200f, 200f);

        // Drop items

        return true;
    }
}
