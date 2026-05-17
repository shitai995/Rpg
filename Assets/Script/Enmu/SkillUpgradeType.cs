// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 14:57:04
// 版本：V1.1
// 描述：技能升级类型枚举（技能树解锁/强化分支）
// ========================================================

using UnityEngine;

/// <summary>
/// 技能升级枚举：定义所有技能树解锁项与强化效果
/// </summary>
public enum SkillUpgradeType
{
    None, // 无升级

    // ====== 冲刺技能树 ======
    Dash, // 解锁基础冲刺
    Dash_CloneOnStart, // 冲刺开始时生成分身
    Dash_CloneOnStartAndArrival, // 冲刺开始与结束时生成分身
    Dash_ShardOnShart, // 冲刺开始时生成碎片
    Dash_ShardOnStartAndArrival, // 冲刺开始与结束时生成碎片

    // ====== 碎片技能树 ======
    Shard, // 解锁基础碎片技能
    Shard_MoveToEnemy, // 碎片自动追踪最近敌人
    Shard_Multicast, // 碎片可充能连发
    Shard_Teleport, // 碎片触发传送
    Shard_TeleportHpRewind, // 碎片传送 + 血量回溯

    // ====== 飞剑技能树 ======
    SwordThrow, // 解锁飞剑投掷（远程伤害）
    SwordThrow_Spin, // 飞剑定点旋转持续切割敌人
    SwordThrow_Pierce, // 飞剑可穿透多个目标
    SwordThrow_Bounce, // 飞剑在敌人之间弹跳

    // ====== 时间残影技能树 ======
    TimeEcho, // 生成时间残影（可承伤）
    TimeEcho_SingleAttack, // 残影可进行一次攻击
    TimeEcho_MultiAttack, // 残影可进行多次攻击
    TimeEcho_ChanceToDuplicate, // 残影攻击时有概率复制新残影

    TimeEcho_HealWisp, // 残影死亡生成治疗精灵，为玩家回血
    TimeEcho_CleanseWisp, // 治疗精灵额外清除玩家负面效果
    TimeEcho_CooldownWisp, // 治疗精灵减少所有技能冷却

    // ====== 领域展开大招 ======
    Domain_SlowingDown, // 创造领域，大幅减速敌人，玩家自由战斗
    Domain_EchoSpam, // 玩家无法移动，持续释放时间残影
    Domain_ShardSpam // 玩家无法移动，持续释放碎片技能
}