// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-09 01:09:33
// 版本：V1.1
// 描述：
// ========================================================

using System.Runtime.InteropServices;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundlayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;


        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth; ;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;   

        foreach(ParallaxLayer layer in backgroundlayers)
        {
            layer.Move(distanceToMove);// 背景跟随相机
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);// 循环背景
        }
    }
    /// <summary>
    /// 计算图像宽度
    /// </summary>
    private void InitializeLayers()
    {
        foreach (ParallaxLayer layer in backgroundlayers)
            layer.CalculateImageWidth();
    }
        
}
