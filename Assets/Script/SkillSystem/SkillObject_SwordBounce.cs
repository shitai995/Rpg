// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-09 16:20:03
// 版本：V1.1
// 描述：弹跳飞剑技能实体，继承基础飞剑，实现多目标弹射效果
// ========================================================
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弹跳飞剑
/// </summary>
public class SkillObject_SwordBounce : SkillObject_Sword
{
    [SerializeField] private float bounceSpeed = 15;
    private int bounceCount;

    private Collider2D[] enemyTargets;
    private Transform nextTarget;
    private List<Transform> selectedBefore = new List<Transform>();

    /// <summary>
    /// 初始化弹跳飞剑
    /// </summary>
    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        anim.SetTrigger("spin");
        base.SetupSword(swordManager, direction);

        bounceSpeed = swordManager.bounceSpeed;
        bounceCount = swordManager.bounceCount;
    }

    protected override void Update()
    {
        HandleComeback();
        HandleBounce();
    }

    /// <summary>
    /// 处理目标弹射移动
    /// </summary>
    private void HandleBounce()
    {
        if (nextTarget == null) return;

        // 向目标位置移动
        transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed * Time.deltaTime);

        // 抵达目标后造成伤害并切换下一个目标
        if (Vector2.Distance(transform.position, nextTarget.position) < 0.75f)
        {
            DamageEnemiesInRadius(transform, 1);
            BounceToNextTarget();

            // 弹射次数用尽或无目标，开始返程
            if (bounceCount == 0 || nextTarget == null)
            {
                nextTarget = null;
                GetSwordBackToPlayer();
            }
        }
    }

    /// <summary>
    /// 切换至下一个弹射目标
    /// </summary>
    private void BounceToNextTarget()
    {
        nextTarget = GetNextTarget();
        bounceCount--;
    }

    /// <summary>
    /// 首次碰撞逻辑
    /// </summary>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 首次命中时检索范围内所有敌人并停住刚体
        if (enemyTargets == null)
        {
            enemyTargets = GetEnemiesAround(transform, 10);
            rb.simulated = false;
        }

        DamageEnemiesInRadius(transform, 1);

        // 无多余目标或弹射次数用完则返程，否则继续弹射
        if (enemyTargets.Length <= 1 || bounceCount == 0)
            GetSwordBackToPlayer();
        else
            nextTarget = GetNextTarget();
    }

    /// <summary>
    /// 随机获取下一个弹射目标
    /// </summary>
    private Transform GetNextTarget()
    {
        List<Transform> validTarget = GetValidTargets();
        int randomIndex = Random.Range(0, validTarget.Count);
        Transform target = validTarget[randomIndex];
        selectedBefore.Add(target);
        return target;
    }

    /// <summary>
    /// 筛选未重复选择的有效目标
    /// </summary>
    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets = new List<Transform>();
        List<Transform> aliveTargets = GetAliveTargets();

        // 排除已选中过的目标
        foreach (var enemy in aliveTargets)
        {
            if (enemy != null && !selectedBefore.Contains(enemy.transform))
                validTargets.Add(enemy.transform);
        }

        // 无新目标则清空记录，重新选择
        if (validTargets.Count > 0)
            return validTargets;
        else
        {
            selectedBefore.Clear();
            return aliveTargets;
        }
    }

    /// <summary>
    /// 筛选存活的敌人目标
    /// </summary>
    private List<Transform> GetAliveTargets()
    {
        List<Transform> aliveTargets = new List<Transform>();
        foreach (var enemy in enemyTargets)
        {
            if (enemy != null)
                aliveTargets.Add(enemy.transform);
        }
        return aliveTargets;
    }
}