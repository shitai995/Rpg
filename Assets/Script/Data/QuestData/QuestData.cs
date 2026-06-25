// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 21:35:46
// 版本：V1.1
// 描述：单条任务运行时数据，存储实时进度，判断是否可领奖
// ========================================================
using System;
using UnityEngine;
[Serializable]
/// <summary>
/// 任务运行时数据，记录单个任务当前进度
/// </summary>
public class QuestData
{
    // 任务静态配置资源
    public QuestDataSO questDataSO;
    // 当前完成进度数值
    public int currentAmount;
    // 是否满足领奖条件缓存标记
    public bool canGetReward;

    /// <summary>
    /// 增加任务进度，自动更新领奖状态
    /// </summary>
    /// <param name="amount">增加量，默认1</param>
    public void AddQuestProgress(int amount = 1)
    {
        currentAmount += amount;
        canGetReward = CanGetReward();
    }

    /// <summary>
    /// 判断进度是否达标、可以领取奖励
    /// </summary>
    public bool CanGetReward() => currentAmount >= questDataSO.requiredAmount;

    /// <summary>
    /// 构造函数，绑定任务配置SO，初始化进度为0
    /// </summary>
    public QuestData(QuestDataSO questSO)
    {
        questDataSO = questSO;
        currentAmount = 0;
        canGetReward = false;
    }
}