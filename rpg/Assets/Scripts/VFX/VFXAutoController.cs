using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1f;
    [Space]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;



    [Header("Random position")]

    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    [Header("Random rotation")]
    [SerializeField, Range(-360f, 0f)] private float zMinRotation = 0;
    [SerializeField, Range(0f, 360f)] private float zMaxRotation = 360;


    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (autoDestroy)
            Destroy(gameObject, destroyDelay);
    }

    private void ApplyRandomOffset()
    {
        if (!randomOffset)
            return;

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position = transform.position + new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotation()
    {
        if (!randomRotation)
            return;

        float zRotation = Random.Range(zMinRotation, zMaxRotation);

        transform.Rotate(0, 0, zRotation);
    }
}
