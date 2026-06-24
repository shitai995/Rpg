// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 20:47:00
// 版本：V1.1
// 描述：任务奖励格子，继承背包道具格子，鼠标悬浮弹出道具提示，屏蔽点击逻辑
// ========================================================
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 任务奖励专用道具格子
/// </summary>
public class UI_QuestRewardSlot : UI_ItemSlot
{
    // 重写点击，无交互逻辑
    public override void OnPointerDown(PointerEventData eventData)
    {

    }

    /// <summary>
    /// 鼠标移入显示道具悬浮提示
    /// </summary>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;
        // 弹出道具信息Tooltip
        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot, false, false, false);
    }
}