using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform Background;
    [SerializeField] private float ParallaxMultiplier;
    [SerializeField] private float ImageWidthOffset = 10f;

    private float ImageFullWidth;
    private float ImageHalfWidth;

    public void CalculateImageWidth()
    {
        ImageFullWidth = Background.GetComponent<SpriteRenderer>().bounds.size.x;
        ImageHalfWidth = ImageFullWidth / 2;
    }

    public void Move(float distanceToMove)
    {
        Background.position += Vector3.right * (distanceToMove * ParallaxMultiplier);
    }

    public void LoopBackground(float cameraLefteEdge, float cameraRightEdge)
    {
        float imageRightEdge = (Background.position.x + ImageHalfWidth) - ImageWidthOffset;
        float imageLeftEdge = (Background.position.x - ImageHalfWidth) + ImageWidthOffset;

        if (imageRightEdge < cameraLefteEdge)
            Background.position += Vector3.right * ImageFullWidth;
        else if (imageLeftEdge > cameraRightEdge)
            Background.position += Vector3.right * -ImageFullWidth;
    }
}
