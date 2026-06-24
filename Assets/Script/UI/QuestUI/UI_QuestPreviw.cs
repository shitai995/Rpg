// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 21:07:13
// 版本：V1.1
// 描述：任务详情预览面板，展示选中任务完整信息，提供接取任务按钮逻辑
// ========================================================
using TMPro;
using UnityEngine;

/// <summary>
/// 任务右侧详情面板UI
/// </summary>
public class UI_QuestPreviw : MonoBehaviour
{
    [Header("任务基础文本")]
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [Header("奖励格子数组")]
    [SerializeField] private UI_QuestRewardSlot[] questReward;
    [Header("面板附加控件(按钮/分割线等)")]
    [SerializeField] private GameObject[] additionalObjects;

    private UI_Quest questUI;       // 父级总任务面板
    private QuestDataSO previwQuest;// 当前预览的任务配置

    /// <summary>
    /// 填充任务详情面板数据
    /// </summary>
    /// <param name="questDataSO">选中的任务配置</param>
    public void SetupQuestPreviw(QuestDataSO questDataSO)
    {
        questUI = transform.root.GetComponentInChildren<UI_Quest>();
        previwQuest = questDataSO;

        EnableAdditonalObjects(true);
        EnableQuestRewardObjects(false);

        // 赋值任务文字信息
        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.description;
        questGoal.text = $"{questDataSO.questGoal} {questDataSO.requiredAmount}";

        // 遍历生成奖励图标
        for (int i = 0; i < questDataSO.rewardItems.Length; i++)
        {
            Inventory_Item rewardItem = new Inventory_Item(questDataSO.rewardItems[i].itemData);
            rewardItem.stackSize = questDataSO.rewardItems[i].stackSize;

            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(rewardItem);
        }
    }

    /// <summary>
    /// 接取任务按钮点击事件
    /// </summary>
    public void AcceptQuestBTN()
    {
        MakeQuestPreviwEmpty();
        // 调用任务管理器接取任务，刷新任务列表
        questUI.questManager.AcceptQuest(previwQuest);
        questUI.UpdateQuestList();
    }

    /// <summary>
    /// 清空详情面板、隐藏所有控件
    /// </summary>
    public void MakeQuestPreviwEmpty()
    {
        questName.text = "";
        questDescription.text = "";
        EnableAdditonalObjects(false);
        EnableQuestRewardObjects(false);
    }

    /// <summary>
    /// 显示/隐藏附加UI物体
    /// </summary>
    private void EnableAdditonalObjects(bool enable)
    {
        foreach (var obj in additionalObjects)
            obj.SetActive(enable);
    }

    /// <summary>
    /// 隐藏所有奖励格子
    /// </summary>
    private void EnableQuestRewardObjects(bool enable)
    {
        foreach (var obj in questReward)
            obj.gameObject.SetActive(enable);
    }
}