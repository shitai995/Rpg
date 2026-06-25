// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 21:34:27
// 版本：V1.1
// 描述：玩家任务管理器，管理进行中/已完成任务、进度更新、NPC领奖、存档读写
// ========================================================
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家任务核心逻辑，统一处理任务接取、进度、交付、领奖、存档
/// </summary>
public class Player_QuestManager : MonoBehaviour, ISaveable
{
    [Header("任务运行数据集合")]
    public List<QuestData> activeQuests;    // 当前正在进行的任务
    public List<QuestData> completedQuests;  // 已完成任务记录

    private Entity_DropManager dropManager;  // 物品掉落管理器，生成任务奖励
    private Inventory_Player inventory;     // 玩家背包，交付任务扣除道具

    [Header("QUEST DATABASE")]
    [SerializeField] private QuestDatabaseSO questDatabase; // 全局任务配置库

    private void Awake()
    {
        // 获取自身挂载的掉落、背包组件
        dropManager = GetComponent<Entity_DropManager>();
        inventory = GetComponent<Inventory_Player>();
    }

    /// <summary>
    /// 和NPC交互领奖，自动处理交付类任务上交道具，发放对应类型任务奖励
    /// </summary>
    /// <param name="npcType">当前交互NPC的奖励类型</param>
    public void TryGetRewardFrom(RewardType npcType)
    {
        List<QuestData> finishList = new List<QuestData>();

        foreach (var quest in activeQuests)
        {
            // 交付任务：背包道具充足则扣除道具、补满任务进度
            if (quest.questDataSO.questType == QuestType.Delivery)
            {
                var requiredItem = quest.questDataSO.itemToDeliver;
                var requiredAmount = quest.questDataSO.requiredAmount;

                if (inventory.HasItemAmount(requiredItem, requiredAmount))
                {
                    inventory.RemoveItemAmount(requiredItem, requiredAmount);
                    quest.AddQuestProgress(requiredAmount);
                }
            }

            // 进度达标且匹配当前NPC领奖类型，加入完成列表
            if (quest.CanGetReward() && quest.questDataSO.rewardType == npcType)
                finishList.Add(quest);
        }

        // 批量发奖励并标记任务完成
        foreach (var quest in finishList)
        {
            GiveQuestReward(quest.questDataSO);
            CompleteQuest(quest);
        }
    }

    /// <summary>
    /// 生成任务奖励道具，逐个实例化掉落物
    /// </summary>
    private void GiveQuestReward(QuestDataSO questDataSO)
    {
        foreach (var item in questDataSO.rewardItems)
        {
            if (item == null || item.itemData == null) continue;
            // 按堆叠数量生成对应个数掉落
            for (int i = 0; i < item.stackSize; i++)
            {
                dropManager.CreateItemDrop(item.itemData);
            }
        }
    }
    public bool HasCompletedQuest()
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            QuestData quest = activeQuests[i];

            if (quest.questDataSO.questType == QuestType.Delivery)
            {
                var requiredItem = quest.questDataSO.itemToDeliver;
                var requiredAmount = quest.questDataSO.requiredAmount;

                if (inventory.HasItemAmount(requiredItem, requiredAmount))
                    return true;
            }

            if (quest.CanGetReward())
                return true;
        }

        return false;
    }
    /// <summary>
    /// 任务目标进度增加（击杀怪物、采集物品时调用）
    /// </summary>
    /// <param name="questTargetId">任务目标唯一标识</param>
    /// <param name="amount">进度增量，默认+1</param>
    public void AddProgress(string questTargetId, int amount = 1)
    {
        List<QuestData> autoFinishList = new List<QuestData>();

        foreach (var quest in activeQuests)
        {
            // 目标ID不匹配直接跳过
            if (quest.questDataSO.questTargetId != questTargetId)
                continue;
            // 未完成才累加进度
            if (!quest.CanGetReward())
                quest.AddQuestProgress(amount);
            // 无需找NPC领奖、进度已满，自动领奖完成
            if (quest.questDataSO.rewardType == RewardType.None && quest.CanGetReward())
                autoFinishList.Add(quest);
        }

        foreach (var quest in autoFinishList)
        {
            GiveQuestReward(quest.questDataSO);
            CompleteQuest(quest);
        }
    }

    /// <summary>
    /// 获取指定任务当前完成进度数值
    /// </summary>
    public int GetQuestProgress(QuestData questToCheck)
    {
        QuestData targetQuest = activeQuests.Find(q => q == questToCheck);
        return targetQuest != null ? targetQuest.currentAmount : 0;
    }

    /// <summary>
    /// 接取新任务，加入进行中任务列表
    /// </summary>
    public void AcceptQuest(QuestDataSO questDataSO)
    {
        activeQuests.Add(new QuestData(questDataSO));
    }

    /// <summary>
    /// 完成任务：移出进行中列表，存入已完成列表
    /// </summary>
    public void CompleteQuest(QuestData questData)
    {
        completedQuests.Add(questData);
        activeQuests.Remove(questData);
    }

    /// <summary>
    /// 判断该任务是否已经接取、正在进行
    /// </summary>
    public bool QuestIsActive(QuestDataSO questToCheck)
    {
        if (questToCheck == null)
            return false;
        return activeQuests.Find(q => q.questDataSO == questToCheck) != null;
    }

    #region ISaveable 存档接口
    /// <summary>
    /// 读取存档，恢复所有进行中任务与进度
    /// </summary>
    public void LoadData(GameData data)
    {
        activeQuests.Clear();
        // 遍历存档任务数据，通过ID从任务库还原任务配置
        foreach (var entry in data.activeQuests)
        {
            string questSaveId = entry.Key;
            int progress = entry.Value;

            QuestDataSO questDataSO = questDatabase.GetQuestById(questSaveId);
            if (questDataSO == null)
            {
                Debug.Log(questSaveId + " was not found in the database!");
                continue;
            }

            QuestData questToLoad = new QuestData(questDataSO);
            questToLoad.currentAmount = progress;
            activeQuests.Add(questToLoad);
        }
    }

    /// <summary>
    /// 保存任务进度、已完成任务标记到存档
    /// </summary>
    public void SaveData(ref GameData data)
    {
        data.activeQuests.Clear();
        // 存储进行中任务ID+当前进度
        foreach (var quest in activeQuests)
        {
            data.activeQuests.Add(quest.questDataSO.questSaveId, quest.currentAmount);
        }
        // 标记已完成任务
        foreach (var quest in completedQuests)
        {
            data.completedQuests.Add(quest.questDataSO.questSaveId, true);
        }
    }
    #endregion
}