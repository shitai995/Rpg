// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-09 01:15:59
// 版本：V1.1
// 描述：视差背景层逻辑类 - 单个背景层的移动、宽度计算、循环衔接
// ========================================================

using UnityEngine;


[System.Serializable]
public class ParallaxLayer
{

    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;// 视差倍数（控制背景移动速度：0=不动，<1=比相机慢，>1=比相机快）
    [SerializeField] private float imageWidthOffset = 10;// 背景偏移补偿
 
    private float imageFullWidth;
    private float imageHalfWidth;

    /// <summary>
    /// 计算背景图片的实际显示宽度
    /// （从SpriteRenderer获取真实宽度，为循环逻辑提供依据）
    /// </summary>
    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    /// <summary>
    /// 背景层移动逻辑
    /// </summary>
    /// <param name="distanceToMove">相机X轴移动的距离</param>
    public void Move(float distanceToMove)
    {
        // 按视差倍数横向移动背景
        background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
    }

    /// <summary>
    /// 背景循环衔接逻辑
    /// 当背景移出相机可视范围时，将其移动到另一侧，实现无缝循环
    /// </summary>
    public void LoopBackground(float cameraLeftEdge,float cameraRightEdge)
    {
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset;

        // 背景完全移出相机左侧 → 向右移动一个图片宽度（循环到右侧）
        if (imageRightEdge < cameraLeftEdge)
            background.position += Vector3.right * imageFullWidth;
        else if (imageLeftEdge > cameraRightEdge)
            background.position += Vector3.left * -imageFullWidth;
    }
}
