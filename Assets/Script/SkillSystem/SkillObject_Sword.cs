// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-27 14:30:54
// 版本：V1.1
// 描述：掷剑技能物体逻辑，控制飞剑飞行、回弹与碰撞伤害
// ========================================================

using UnityEngine;

/// <summary>
/// 飞剑技能实体
/// </summary>
public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordManager;

    protected Transform playerTransform;
    protected bool shouldComeback;        // 是否开始返程
    protected float comebackSpeed = 20;   // 返程速度
    protected float maxAllowedDistance = 25; // 最大飞行距离

    protected virtual void Update()
    {
        // 朝向与运动方向保持一致
        transform.right = rb.linearVelocity;
        HandleComeback();
    }

    /// <summary>
    /// 初始化飞剑参数
    /// </summary>
    public virtual void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        rb.linearVelocity = direction;
        this.swordManager = swordManager;

        playerTransform = swordManager.transform.root;
        playerStats = swordManager.player.stats;
        damageScaleData = swordManager.damageScaleData;
    }

    /// <summary>
    /// 触发飞剑返程
    /// </summary>
    public void GetSwordBackToPlayer() => shouldComeback = true;

    /// <summary>
    /// 处理飞剑返程逻辑
    /// </summary>
    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        // 超出最大距离强制返程
        if (distance > maxAllowedDistance)
            GetSwordBackToPlayer();

        if (!shouldComeback) return;

        // 向玩家位置移动
        transform.position = Vector2.MoveTowards(transform.position,
            playerTransform.position, comebackSpeed * Time.deltaTime);

        // 靠近玩家后销毁
        if (distance < 0.5f)
            Destroy(gameObject);
    }

    /// <summary>
    /// 碰撞检测，命中目标后停止并造成范围伤害
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemiesInRadius(transform, 1);
    }

    /// <summary>
    /// 停止飞剑运动并依附到碰撞物体上
    /// </summary>
    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}