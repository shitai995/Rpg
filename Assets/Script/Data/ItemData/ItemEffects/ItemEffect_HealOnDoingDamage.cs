// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 20:42:36
// 版本：V1.1
// 描述：造成物理伤害时按比例回血效果
// ========================================================

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal on doing damage", fileName = "Item effect data - Heal on doing phys damage")]

public class ItemEffect_HealOnDoingDamage : ItemEffect_DataSO
{
    [Tooltip("造成伤害时的治疗比例（吸血率）")]
    [SerializeField] private float percentHealedOnAttack = .2f;

    /// <summary>
    /// 绑定玩家并注册伤害事件
    /// </summary>
    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        // 监听物理伤害事件，触发吸血
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    /// <summary>
    /// 解绑玩家并注销事件
    /// </summary>
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }

    /// <summary>
    /// 伤害触发治疗：根据伤害值恢复血量
    /// </summary>
    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * percentHealedOnAttack);
    }
}