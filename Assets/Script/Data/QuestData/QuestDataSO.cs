// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 15:27:32
// 版本：V1.1
// 描述：单个任务静态配置SO，存储任务基础信息、目标、奖励，自动生成唯一存档ID
// ========================================================
using UnityEditor;
using UnityEngine;

// 领奖NPC类型
public enum RewardType { Merchant, Blacksmith, None }
// 任务类型
public enum QuestType { Kill, Talk, Delivery }

[CreateAssetMenu(menuName = "RPG Setup/ Quest Data/ New Quest", fileName = "Quest - ")]
/// <summary>
/// 任务静态配置资源，编辑面板填写任务所有固定参数
/// </summary>
public class QuestDataSO : ScriptableObject
{
    [Header("存档唯一标识(自动生成)")]
    public string questSaveId;

    [Space]
    public QuestType questType;        // 任务分类：击杀/对话/交付物品
    public string questName;           // 任务名称
    [TextArea] public string description; // 任务剧情描述
    [TextArea] public string questGoal;   // 任务目标文字说明

    public string questTargetId;       // 目标标识：怪物/NPC/物品ID
    public int requiredAmount;        // 需要完成的目标数量
    public ItemDataSO itemToDeliver;  // 交付类任务专用，要上交的道具

    [Header("奖励设置")]
    public RewardType rewardType;     // 领取奖励对应的NPC类型
    public Inventory_Item[] rewardItems; // 任务奖励道具数组

#if UNITY_EDITOR
    // 资源变更/保存时自动用资源GUID赋值存档ID
    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        questSaveId = AssetDatabase.AssetPathToGUID(path);
    }
#endif
}