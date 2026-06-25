// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-23 21:53:52
// 版本：V1.1
// 描述：NPC任务绑定数据，存储NPC领奖类型与关联所有任务
// ========================================================
using System;
/// <summary>
/// NPC绑定的任务配置数据，序列化存储NPC奖励类型+对应任务列表
/// </summary>
[Serializable]
public class DialogueNpcData
{
    public RewardType npcRewardType;    // NPC对应的领奖类型，任务管理器领奖匹配用
    public QuestDataSO[] quests;        // 该NPC可接取的全部任务

    /// <summary>
    /// 构造函数，快速初始化NPC任务数据
    /// </summary>
    public DialogueNpcData(RewardType npcRewardType, QuestDataSO[] quests)
    {

        this.npcRewardType = npcRewardType;
        this.quests = quests;
    }
}