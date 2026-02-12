// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-22 23:32:59
// 版本：V1.1
// 描述：敌人血量管理类
// ========================================================

using UnityEngine;

public class Enemy_Health : Entity_Health
{


    private Enemy enemy => GetComponent<Enemy>();

    /// <summary>
    /// 重写基类的受击方法，扩展敌人专属逻辑
    /// 执行流程：判断伤害来源是否为玩家 → 触发战斗状态 → 执行基类通用受击逻辑（扣血、击退、特效）
    /// </summary>
    public override bool TakeDamage(float damage,float elementalDamage,ElementType element, Transform damageDealer)
    {
        // 此处顺序：先触发战斗状态，再扣血（符合“被玩家攻击→进入战斗→受击扣血”的逻辑）
        bool wasHit =  base.TakeDamage(damage,elementalDamage, element, damageDealer);

        if (!wasHit)
            return false;

        // 1. 伤害来源判定：仅当伤害来源物体挂载了Player组件时，触发敌人战斗状态
        if (damageDealer.GetComponent<Player>() != null)
            // 调用敌人核心逻辑，尝试进入战斗状态（传递伤害来源，用于敌人转向/追击玩家）
            enemy.TryEnterBattleState(damageDealer);
        return true;
    }
}
