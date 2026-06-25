// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 20:56:10
// 版本：V1.1
// 描述：任务列表单个格子组件，展示任务名、奖励图标，点击刷新右侧任务详情面板
// ========================================================

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// 任务栏单个格子UI
/// </summary>
public class UI_QuestSlot : MonoBehaviour
{
    [Header("任务名称文本")]
    [SerializeField] private TextMeshProUGUI questName;
    [Header("奖励快速预览图标槽位数组")]
    [SerializeField] private Image[] rewardQuickPreviwSlots;

    /// <summary>
    /// 当前格子绑定的任务数据
    /// </summary>
    public QuestDataSO questInSlot { get; private set; }
    // 任务详情弹窗控制器
    private UI_QuestPreviw questPreviw;

    /// <summary>
    /// 初始化格子，填充任务名称、奖励图标
    /// </summary>
    /// <param name="questDataSO">任务配置数据</param>
    public void SetupQuestSlot(QuestDataSO questDataSO)
    {
        // 从父级根UI获取全局任务详情面板
        questPreviw = transform.root.GetComponentInChildren<UI_Quest>().GetQuestPreviw();

        // 绑定任务数据+显示任务名
        questInSlot = questDataSO;
        questName.text = questDataSO.questName;

        // 先隐藏所有奖励图标
        foreach (var previwIcon in rewardQuickPreviwSlots)
        {
            previwIcon.gameObject.SetActive(false);
        }

        // 循环填充奖励图标与数量文字
        for (int i = 0; i < questInSlot.rewardItems.Length; i++)
        {
            // 奖励数据为空跳过
            if (questDataSO.rewardItems[i] == null || questDataSO.rewardItems[i].itemData == null) continue;

            Image slot = rewardQuickPreviwSlots[i];
            slot.gameObject.SetActive(true);
            slot.sprite = questDataSO.rewardItems[i].itemData.itemIcon;
            // 赋值奖励堆叠数量
            slot.GetComponentInChildren<TextMeshProUGUI>().text = questDataSO.rewardItems[i].stackSize.ToString();
        }
    }

    /// <summary>
    /// 刷新右侧任务详情面板（格子点击事件调用）
    /// </summary>
    public void UpdateQuestPreviw()
    {
        questPreviw.SetupQuestPreviw(questInSlot);
    }
}  