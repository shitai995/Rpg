// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-23 15:14:30
// 版本：V1.1
// 描述：进行中任务详情面板，展示任务名、描述、实时进度、奖励
// ========================================================
using TMPro;
using UnityEngine;

/// <summary>
/// 进行中任务右侧详情UI面板
/// </summary>
public class UI_ActiveQuestPreviw : MonoBehaviour
{
    private Player_QuestManager questManager;

    [Header("文本组件")]
    [SerializeField] private TextMeshProUGUI questName;    // 任务名称
    [SerializeField] private TextMeshProUGUI description;   // 任务剧情描述
    [SerializeField] private TextMeshProUGUI progress;      // 进度文本（当前/需求）
    [Header("奖励格子数组")]
    [SerializeField] private UI_QuestRewardSlot[] questRewardSlots;

    /// <summary>
    /// 填充当前选中进行中任务的全部详情
    /// </summary>
    /// <param name="questData">任务运行时进度数据</param>
    public void SetupQuestPreviw(QuestData questData)
    {
        questManager = Player.instance.questManager;
        QuestDataSO questSO = questData.questDataSO;

        // 基础文字赋值
        questName.text = questSO.name;
        description.text = questSO.description;
        // 拼接进度文字：目标 + 当前进度/总需求数量
        progress.text = $"{questSO.questGoal} {questManager.GetQuestProgress(questData)}/{questSO.requiredAmount}";

        // 先隐藏所有奖励格子
        foreach (var slot in questRewardSlots)
            slot.gameObject.SetActive(false);

        // 渲染奖励图标
        for (int i = 0; i < questSO.rewardItems.Length; i++)
        {
            questRewardSlots[i].gameObject.SetActive(true);
            questRewardSlots[i].UpdateSlot(questSO.rewardItems[i]);
        }
    }
}