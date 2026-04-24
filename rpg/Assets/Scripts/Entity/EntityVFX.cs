using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    [Header("On taking damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On dealing damage VFX")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;

    [Header("Elemental colors")]
    [SerializeField] private Color chillVfxColor = Color.cyan;
    [SerializeField] private Color burnVfxColor = Color.red;
    [SerializeField] private Color electrifyVfxColor = Color.yellow;
    private Color originalHitVfxColor;

    [Header("Elemental effects")]
    [SerializeField] private float tickInterval = 0.2f;
    [SerializeField] private float lightColorMultiplier = 1.2f;
    [SerializeField] private float darkColorMultiplier = 0.8f;


    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                StartCoroutine(PlayStatusVfxCoroutine(duration, chillVfxColor));
                break;

            case ElementType.Fire:
                StartCoroutine(PlayStatusVfxCoroutine(duration, burnVfxColor));
                break;

            case ElementType.Lightning:
                StartCoroutine(PlayStatusVfxCoroutine(duration, electrifyVfxColor));
                break;
        }
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVfxCoroutine(float duration, Color color)
    {
        float elapsedTime = 0;
        bool toggle = false;

        Color lightColor = color * lightColorMultiplier;
        Color darkColor = color * darkColorMultiplier;

        while (elapsedTime < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);

            elapsedTime += tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVfx(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        if (!isCrit)
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

        if (entity.facingDirection == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    public void UpdateOnHitColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire:
                hitVfxColor = chillVfxColor;
                break;

            case ElementType.Ice:
                hitVfxColor = chillVfxColor;
                break;

            case ElementType.Lightning:
                hitVfxColor = chillVfxColor;
                break;

            default:
                hitVfxColor = originalHitVfxColor;
                break;
        }
    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
        {
            StopCoroutine(OnDamageVfxCoroutine());
        }

        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCoroutine());
    }

    private IEnumerator OnDamageVfxCoroutine()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);

        sr.material = originalMaterial;
    }
}