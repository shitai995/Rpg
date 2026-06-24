// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-23 15:15:02   
// 版本：V1.1
// 描述：进行中任务列表单项格子，展示任务名、奖励缩略图，点击打开详情面板
// ========================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 进行中任务列表Item
/// </summary>
public class UI_ActiveQuestSlot : MonoBehaviour
{
    // 当前格子绑定的运行时任务数据
    private QuestData questInSlot;
    // 进行中任务详情面板
    private UI_ActiveQuestPreviw questPreviw;

    [SerializeField] private TextMeshProUGUI questName;          // 任务名称文本
    [SerializeField] private Image[] questRewardPreviw;          // 奖励预览图标数组

    /// <summary>
    /// 初始化格子，填充任务名称、奖励图标
    /// </summary>
    /// <param name="questToSetup">运行时任务数据</param>
    public void SetupActiveQuestSlot(QuestData questToSetup)
    {
        // 从根UI获取详情面板
        questPreviw = transform.root.GetComponentInChildren<UI_ActiveQuestPreviw>();
        questInSlot = questToSetup;
        questName.text = questToSetup.questDataSO.questName;

        Inventory_Item[] reward = questToSetup.questDataSO.rewardItems;
        // 先隐藏所有奖励图标
        foreach (var previwIcon in questRewardPreviw)
            previwIcon.gameObject.SetActive(false);

        // 填充奖励图标与数量文字
        for (int i = 0; i < reward.Length; i++)
        {
            if (reward[i] == null) continue;
            Image previw = questRewardPreviw[i];
            previw.gameObject.SetActive(true);
            previw.sprite = reward[i].itemData.itemIcon;
            previw.GetComponentInChildren<TextMeshProUGUI>().text = reward[i].stackSize.ToString();
        }
    }

    /// <summary>
    /// 格子点击按钮事件：刷新右侧进行中任务详情
    /// </summary>
    public void SetupPreviwBTN()
    {
        questPreviw.SetupQuestPreviw(questInSlot);
    }
}