using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera MainCamera;
    private float LastCameraPositionX;
    private float CameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        MainCamera = Camera.main;
        CameraHalfWidth = MainCamera.orthographicSize * MainCamera.aspect;
        InitializeLayers();
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = MainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - LastCameraPositionX;
        LastCameraPositionX = currentCameraPositionX;

        float cameraLeftEdge = currentCameraPositionX - CameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + CameraHalfWidth;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void InitializeLayers()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
            layer.CalculateImageWidth();
    }
}
