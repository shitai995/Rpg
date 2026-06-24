// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 21:17:04
// 版本：V1.1
// 描述：任务总面板UI，管理所有任务格子、详情面板，实现存档读写接口
// ========================================================
using UnityEngine;

/// <summary>
/// 任务主面板，控制任务列表刷新、存档数据存储
/// </summary>
public class UI_Quest : MonoBehaviour, ISaveable
{
    // 游戏存档数据
    private GameData currentGameData;

    [SerializeField] private UI_ItemSlotParent inventorySlots; // 背包UI父物体
    [SerializeField] private UI_QuestPreviw questPreviw;       // 任务详情面板

    private UI_QuestSlot[] questSlots; // 所有任务格子
    public Player_QuestManager questManager { get; private set; } // 任务管理器

    private void Awake()
    {
        // 查找全部子物体任务格子（包含隐藏）
        questSlots = GetComponentsInChildren<UI_QuestSlot>(true);
        questManager = Player.instance.questManager;
    }

    /// <summary>
    /// 初始化任务列表，填充任务格子数据
    /// </summary>
    /// <param name="questsToSetup">待展示任务数组</param>
    public void SetupQuestUI(QuestDataSO[] questsToSetup)
    {
        // 先隐藏全部格子
        foreach (var slot in questSlots)
            slot.gameObject.SetActive(false);

        // 依次激活格子并填充任务数据
        for (int i = 0; i < questsToSetup.Length; i++)
        {
            questSlots[i].gameObject.SetActive(true);
            questSlots[i].SetupQuestSlot(questsToSetup[i]);
        }

        questPreviw.MakeQuestPreviwEmpty(); // 清空右侧详情面板
        inventorySlots.UpdateSlots(Player.instance.inventory.itemList); // 同步背包显示

        UpdateQuestList(); // 过滤不可领取任务
    }

    /// <summary>
    /// 更新任务列表，隐藏已接取的任务
    /// </summary>
    public void UpdateQuestList()
    {
        foreach (var slot in questSlots)
        {
            if (slot.questInSlot == null) continue;
            // 格子显示中且任务不可领取，则隐藏格子
            if (slot.gameObject.activeSelf && CanTakeQuest(slot.questInSlot) == false)
                slot.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 判断任务是否可领取
    /// </summary>
    /// <param name="questToCheck">目标任务配置</param>
    /// <returns>true=可领取</returns>
    private bool CanTakeQuest(QuestDataSO questToCheck)
    {
        // 判断任务是否已接取进行中
        bool questActive = questManager.QuestIsActive(questToCheck);

        if (currentGameData != null)
        {
            // 存档判断已完成任务逻辑
            bool questCompleted =
             currentGameData.completedQuests.TryGetValue(questToCheck.questSaveId, out bool isCompleted) && isCompleted;
            return questActive == false && questCompleted == false;
        }
        // 当前仅判断：未接取则可领取
        return questActive == false;
    }

    /// <summary>
    /// 外部获取任务详情面板组件
    /// </summary>
    public UI_QuestPreviw GetQuestPreviw() => questPreviw;

    // 存档接口：读取存档数据
    public void LoadData(GameData data)
    {
        currentGameData = data;
    }

    // 存档接口：写入存档（暂无存储逻辑）
    public void SaveData(ref GameData data)
    {

    }
}