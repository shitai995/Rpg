// ========================================================
// 作者：娇娇
// 版本：V1.1
// 描述：旋斩飞剑技能实体，达到最大距离停止飞行并持续范围攻击
// ========================================================
using UnityEngine;

/// <summary>
/// 旋转飞剑
/// </summary>
public class SkillObject_SwordSpin : SkillObject_Sword
{
    private int maxDistance;         // 最大飞行距离
    private float attacksPerSecond;  // 每秒攻击次数
    private float attackTimer;       // 攻击计时

    /// <summary>
    /// 初始化旋转飞剑
    /// </summary>
    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);
        anim?.SetTrigger("spin");

        maxDistance = swordManager.maxDistance;
        attacksPerSecond = swordManager.attacksPerSecond;

        // 到达最大旋转时长后开始返程
        Invoke(nameof(GetSwordBackToPlayer), swordManager.maxSpinDuration);
    }

    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeback();
    }

    /// <summary>
    /// 超出最大距离则停止物理运动
    /// </summary>
    private void HandleStopping()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance > maxDistance && rb.simulated)
            rb.simulated = false;
    }

    /// <summary>
    /// 定时执行范围伤害
    /// </summary>
    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1 / attacksPerSecond;
        }
    }

    /// <summary>
    /// 碰撞后停止飞行
    /// </summary>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}