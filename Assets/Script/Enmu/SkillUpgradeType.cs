// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 14:57:04
// 版本：V1.1
// 描述：技能升级类型枚举
// ========================================================

using UnityEngine;

public enum SkillUpgradeType 
{
    None,

    // ------ Dash Tree -----
    Dash,// 基础冲刺解锁
    Dash_CloneOnStart,// 冲刺开始生成分身
    Dash_CloneOnStartAndArrival,// 冲刺开始和结束都生成分身
    Dash_ShardOnShart,// 冲刺开始生成碎片
    Dash_ShardOnStartAndArrival,// 冲刺开始和结束都生成碎片

    // ----- Shard Tree -----
    Shard,// 基础碎片解锁
    Shard_MoveToEnemy,// 碎片自动追踪最近敌
    Shard_Multicast,// 碎片充能连发
    Shard_Teleport,// 碎片传送
    Shard_TeleportHpRewind// 碎片传送+血量回溯

}
