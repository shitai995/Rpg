// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 20:14:14
// 版本：V1.1
// 描述：Buff类道具效果数据SO，用于配置并执行道具的Buff效果
// ========================================================

using System;
using UnityEngine;

/// <summary>
/// 在编辑器中创建Buff效果配置项
/// </summary>
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Buff effect", fileName = "Item effect data - Buff")]

/// <summary>
/// Buff类道具效果实现类
/// </summary>
public class ItemEffect_Buff : ItemEffect_DataSO
{
    [Header("Buff效果配置")]
    [Tooltip("需要施加的Buff数据组")]
    [SerializeField] private BuffEffectData[] buffsToApply;

    [Tooltip("Buff持续时间")]
    [SerializeField] private float duration;

    [Tooltip("Buff唯一来源ID，防止重复施加")]
    [SerializeField] private string source = Guid.NewGuid().ToString();


    /// <summary>
    /// 判断是否可使用该Buff效果（防重复施加）
    /// </summary>
    public override bool CanBeUsed(Player player)
    {
        // 检查是否可施加该来源Buff
        if (player.stats.CanApplyBuffOf(source))
        {
            this.player = player;
            return true;
        }
        else
        {
            Debug.Log("Same buff effect cannot be applied twice!");
            return false;
        }
    }

    /// <summary>
    /// 执行Buff效果：给玩家施加配置好的Buff
    /// </summary>
    public override void ExecuteEffect()
    {
        player.stats.ApplyBuff(buffsToApply, duration, source);
        player = null;
    }
}