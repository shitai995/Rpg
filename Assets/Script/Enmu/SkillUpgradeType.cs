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
    Shard_TeleportHpRewind,// 碎片传送+血量回溯



    // ----- Shard Tree -----
    SwordThrow,// You can throw sword to damage enemies from range
    SwordThrow_Spin,// Your sword will spin at one point and damage enemies. Like a chainsaw
    SwordThrow_Pierce,// Pierce sword will pierce N targets
    SwordThrow_Bounce, // Bounce sword will bounce between enemies


    // ------ Time Ecoh -------
    TimeEcho,  // Create a clone of a player. It can take damage from enemies.
    TimeEcho_SingleAttack, // Time Echo can perform a single attack
    TimeEcho_MultiAttack, // Time Echo can perform N attacks
    TimeEcho_ChanceToDuplicate, // Time Echo has a chance to create another time echo when attacks

    TimeEcho_HealWisp, // When time echo dies it creates a wips that flies towards the player to heal it.
                       // Heal is = to percantage of damage taken when died
    TimeEcho_CleanseWisp, // Wisp will now remove negative effects from player
    TimeEcho_CooldownWisp, // Wisp will reduce cooldown of all skills by N second. 

    // ------ Domain Expansion -------
    Domain_SlowingDown, // Create an area in which you slow down enemies by 90-100% . You can freely move and fight.
    Domain_EchoSpam, // You can no longer move, but you spam enemy with Time Echo ability
    Domain_ShardSpam // You can no longer move, but you spam enemy with Time Shard ability

}
