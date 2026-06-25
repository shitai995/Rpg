// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-23 15:14:08
// 版本：V1.1
// 描述：进行中任务总面板，打开时自动刷新所有正在进行的任务格子
// ========================================================
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 进行中任务列表主面板
/// </summary>
public class UI_ActiveQuest : MonoBehaviour
{
    private Player_QuestManager questManager;
    private UI_ActiveQuestSlot[] questSlots; // 所有任务格子预制体

    private void Awake()
    {
        // 获取玩家任务管理器
        questManager = Player.instance.questManager;
        // 查找全部子物体任务格子（包含隐藏）
        questSlots = GetComponentsInChildren<UI_ActiveQuestSlot>(true);
    }

    // 面板激活时刷新任务列表
    private void OnEnable()
    {
        List<QuestData> activeQuestList = questManager.activeQuests;

        // 先隐藏全部格子
        foreach (var slot in questSlots)
            slot.gameObject.SetActive(false);

        // 根据进行中任务数量激活并填充格子数据
        for (int i = 0; i < activeQuestList.Count; i++)
        {
            questSlots[i].gameObject.SetActive(true);
            questSlots[i].SetupActiveQuestSlot(activeQuestList[i]);
        }

        // 存在任务则默认选中第一条，自动展示详情
        if (activeQuestList.Count > 0)
            questSlots[0].SetupPreviwBTN();
    }
}