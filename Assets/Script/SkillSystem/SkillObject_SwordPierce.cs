// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-27 19:48:49
// 版本：V1.1
// 描述：穿刺飞剑技能实体，支持穿透多个目标，命中地面或穿透次数用尽后停驻
// ========================================================

using UnityEngine;

/// <summary>
/// 穿刺飞剑
/// </summary>
public class SkillObject_SwordPierce : SkillObject_Sword
{
    private int amountTopPierce; // 剩余可穿透次数

    /// <summary>
    /// 初始化穿刺飞剑参数
    /// </summary>
    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);
        amountTopPierce = swordManager.amountToPierce;
    }

    /// <summary>
    /// 碰撞逻辑，实现穿透判定与伤害
    /// </summary>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool isGround = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        // 穿透次数耗尽 或 命中地面，飞剑停止并造成伤害
        if (amountTopPierce <= 0 || isGround)
        {
            DamageEnemiesInRadius(transform, 0.3f);
            StopSword(collision);
            return;
        }

        // 消耗一次穿透次数并造成伤害，继续飞行
        amountTopPierce--;
        DamageEnemiesInRadius(transform, 0.3f);
    }
}