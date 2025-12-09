// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-09 01:15:59
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;


[System.Serializable]
public class ParallaxLayer
{

    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;//视觉差数
    [SerializeField] private float imageWidthOffset = 10;// 背景偏移补偿
 
    private float imageFullWidth;
    private float imageHalfWidth;


    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }
    public void Move(float distanceToMove)
    {
        background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
        
    }

    public void LoopBackground(float cameraLeftEdge,float cameraRightEdge)
    {
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset;

        if (imageRightEdge < cameraLeftEdge)
            background.position += Vector3.right * imageFullWidth;
        else if (imageLeftEdge > cameraRightEdge)
            background.position += Vector3.left * -imageFullWidth;
    }
}
