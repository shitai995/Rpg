// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 21:38:55
// 版本：V1.1
// 描述：技能物体基类（SkillObject_Base）
// 为所有技能实例化物体（如碎片、分身、子弹）提供通用核心功能：
// 敌人检测、范围伤害、最近目标查找、Gizmos可视化调试等
// ========================================================

using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject onHitVfx;
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;// 目标检测的中心点
    [SerializeField] protected float checkRadius = 1;// 检测半径

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;// 伤害倍率配置
    protected ElementType usedElement;// 当前技能使用的元素类型
    protected bool targetGotHit;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// 对指定范围内的所有敌人造成伤害
    /// 核心逻辑：检测敌人 → 调用IDamgable接口 → 应用元素状态效果
    /// </summary>
    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        // 遍历检测范围内的所有敌人
        foreach (var target in GetEnemiesAround(t, radius))
        {
            // 1. 检测目标是否实现可受击接口
            IDamgable damgable = target.GetComponent<IDamgable>();

            if (damgable == null)
                continue;
            // 2. 通过玩家属性计算最终攻击数据
            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            // 4. 提取攻击数据参数，调用受击接口造成伤害
            float physDamage = attackData.phyiscalDamage;
            float elemDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            targetGotHit = damgable.TakeDamage(physDamage, elemDamage, element, transform);
            // 5. 若有元素类型，应用对应的元素状态效果
            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
            {
                lastTarget = target.transform;
                Instantiate(onHitVfx, target.transform.position, Quaternion.identity);
            }
            // 6. 记录本次使用的元素类型
            usedElement = element;
        }
    }
    /// <summary>
    /// 查找10米范围内最近的敌人目标
    /// 用于技能物体的追踪、锁定逻辑（如碎片向最近敌人移动）
    /// </summary>
    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in GetEnemiesAround(transform, 10))
        {
            // 计算当前敌人与自身的距离
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            // 更新最近目标
            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }

        return target;
    }
    /// <summary>
    /// 检测指定范围内的所有敌人
    /// </summary>

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
