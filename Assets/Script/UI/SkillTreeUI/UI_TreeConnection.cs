// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-07 21:14:57
// 版本：V1.1
// 描述：技能树节点连接线组件
// ========================================================

using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;// 旋转支点
    [SerializeField] private RectTransform connectionLength;// 连接线长度控制组件
    [SerializeField] private RectTransform childNodeConnectionPoint;// 子节点连接点
    /// <summary>
    /// 设置连接线的方向、长度和偏移角度（核心接口）
    /// </summary>
    public void DirectConnection(NodeDirectionType direction, float length,float offset)
    {
        // 判断是否激活连接线：方向为None时隐藏
        bool shouldBeActive = direction != NodeDirectionType.None;
        // 最终长度：激活时为目标长度，未激活时为0
        float finalLength = shouldBeActive ? length : 0;
        // 获取方向对应的基础角度 + 偏移量
        float angle = GetDirectionAngle(direction);
        // 1. 设置连接线旋转角度
        rotationPoint.localRotation = Quaternion.Euler(0,0,angle + offset);
        // 2. 设置连接线长度
        connectionLength.sizeDelta = new Vector2(finalLength,connectionLength.sizeDelta.y);
    }
    // 获取连接线的Image组件
    public Image GetConnectionImage() => connectionLength.GetComponent<Image>();
    /// <summary>
    /// 获取子节点连接点在目标RectTransform中的局部坐标（用于锚定连接线）
    /// </summary>
    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        // 将连接点的世界坐标转换为目标Rect的局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rect.parent as RectTransform,
                childNodeConnectionPoint.position,
                null,
                out var localPosition
            );
        return localPosition;
    }
    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.DownLeft: return -135f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownRight: return -45f;
            default: return 0f;
        }
    }
}

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight,
}

