
// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-07 15:50:55
// 版本：V1.1
// 描述：通用提示框基类
// ========================================================

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;// 提示框的RectTransform组件
    [SerializeField] private Vector2 offset = new Vector2(300,20);// 提示框相对于目标的偏移量

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    /// <summary>
    /// 显示/隐藏提示框
    /// </summary>
    public virtual void ShowToolTip(bool show,RectTransform targetRect)
    {
        // 场景销毁时 UI 对象可能已被销毁，避免 MissingReferenceException
        if (rect == null) return;

        if(show == false)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdatePosition(targetRect);
    }
    /// <summary>
    /// 更新提示框位置（核心自适应逻辑）
    /// 保证提示框始终在屏幕内，且根据目标位置自动调整左右/上下偏移
    /// </summary>
    private void UpdatePosition(RectTransform targetRect)
    {
        // 1. 获取屏幕边界参考值
        float screenCenterX = Screen.width / 2f;// 屏幕水平中心
        float screenTop = Screen.height;// 屏幕顶部Y坐标
        float screenBottom = 0;// 屏幕底部Y坐标

        // 2. 获取目标UI的世界坐标（作为提示框的基础位置）
        Vector2 targetPosition = targetRect.position;

        // 3. 水平位置自适应：目标在屏幕右侧 → 提示框左移；左侧 → 右移
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;
        // 4. 垂直边界校验：计算提示框上下边界是否超出屏幕
        float veritcalHalf = rect.sizeDelta.y / 2f;
        float topy = targetPosition.y + veritcalHalf;
        float bottomy = targetPosition.y - veritcalHalf;
        // 5. 顶部超出屏幕 → 提示框下移， 底部超出屏幕 → 提示框上移
        if (topy > screenTop)
            targetPosition.y = screenTop - veritcalHalf - offset.y;
        else if(bottomy < screenBottom)
            targetPosition.y = screenBottom + veritcalHalf + offset.y;

        // 7. 应用最终位置到提示框

        rect.position = targetPosition;
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }
}
