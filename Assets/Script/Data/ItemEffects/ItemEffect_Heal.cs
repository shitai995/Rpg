// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 14:31:42
// 版本：V1.1
// 描述：治疗类道具效果实现
// ========================================================

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/ Heal effect", fileName = "Item effect data - heal")]

public class ItemEffect_Heal : ItemEffect_DataSO
{
    [Tooltip("治疗百分比（基于最大生命值）")]
    [SerializeField] private float healPercent = .35f;

    /// <summary>
    /// 执行治疗效果
    /// </summary>
    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();
        // 计算治疗量：最大生命值 * 治疗百分比
        float healAmount = player.stats.GetMaxHealth() * healPercent;

        // 恢复玩家生命值
        player.health.IncreaseHealth(healAmount);
    }
}