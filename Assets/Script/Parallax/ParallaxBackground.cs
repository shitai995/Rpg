// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-09 01:09:33
// 版本：V1.1
// 描述：视差背景滚动脚本 - 实现多层背景随相机移动产生不同速度的滚动，且背景循环无缝衔接
// ========================================================

using System.Runtime.InteropServices;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX;// 上一帧相机的X坐标（用于计算相机移动距离）
    private float cameraHalfWidth;// 相机可视范围的半宽（用于判断背景是否需要循环）

    // 在Inspector面板中配置的视差背景层数组（可添加多个不同速度的背景层）
    [SerializeField] private ParallaxLayer[] backgroundlayers;

    private void Awake()
    {
        mainCamera = Camera.main;

        // 计算相机可视范围半宽（正交相机：尺寸 * 宽高比 = 半宽）
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;

        // 计算相机本次移动的距离（X轴）
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;

        // 计算相机可视范围的左右边界（X轴）
        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth; ;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;   

        foreach(ParallaxLayer layer in backgroundlayers)
        {
            layer.Move(distanceToMove);// 背景跟随相机
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);// 循环背景
        }
    }
    /// <summary>
    /// 初始化所有背景层：计算每个背景的实际宽度
    /// </summary>
    private void InitializeLayers()
    {
        foreach (ParallaxLayer layer in backgroundlayers)
            layer.CalculateImageWidth();
    }
        
}
