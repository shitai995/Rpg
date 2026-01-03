// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-22 00:27:54
// 版本：V1.1
// 描述：实体血量类
// ========================================================
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IDamgable
{
    private Slider healthBar;
    private Entity_VFX entityVfx;// 实体特效组件（用于播放受击特效）
    private Entity entity;
    private Entity_Stats stats;


    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;// 死亡状态标记（保护级便于子类重写）


    [Header("普通受击击退参数")]
    [SerializeField] private float knockbackDuration = .2f;// 普通击退持续时间（秒）
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);// 普通击退力度（X水平，Y垂直）

    [Header("重伤受击击退参数")]
    [SerializeField] private float heavyDamageThreshold = .3f;// 重伤判定阈值（伤害/当前血量 > 该值则判定为重伤）
    [SerializeField] private float heavyKnockDuration = .5f;// 重伤击退持续时间（秒）
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);// 重伤击退力度（X水平，Y垂直）
    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();

        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    /// <summary>
    /// 处理实体受击逻辑
    /// </summary>
    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }


        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = stats.GetArmorMitigation(armorReduction);
        float finalDamage = damage * (1 - mitigation);
        // 1. 计算击退力度和方向（根据是否为重伤 + 伤害来源方向）
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        // 2. 计算击退持续时间（根据是否为重伤）
        float duration = CalculateDuration(finalDamage);


        // 4. 执行击退逻辑（空条件运算符：兼容无Entity组件的情况）
        entity?.ReciveKnockback(knockback, duration);
        // 3. 播放受击特效（空条件运算符：避免组件为空时空引用报错）
        entityVfx?.PlayOnDamageVfx();
        // 5. 扣减生命值
        ReduceHp(finalDamage);
        Debug.Log("Damage taken: " + finalDamage);

        return true;
    }


    private bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();

    /// <summary>
    /// 扣减生命值并判断是否死亡
    /// </summary>  
    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if(currentHp <= 0)
            Die();
        
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHp / stats.GetMaxHealth();
    }

    /// <summary>
    /// 计算击退力度和方向
    /// </summary>
    private Vector2 CalculateKnockback(float damage,Transform damageDealer)
    {
        // 计算击退方向：
        // 实体X坐标 > 伤害来源X坐标 → 向右击退（1）；否则向左击退（-1）
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        // 根据是否为重伤，选择对应的击退力度模板
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        // 给水平击退力度乘以方向，垂直力度保持不变
        knockback.x = knockback.x * direction;

        return knockback;
    }
    /// <summary>
    /// 计算击退持续时间
    /// </summary>
    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockDuration : knockbackDuration;
    }
    /// <summary>
    /// 判断是否为重伤（表达式体方法简化写法）
    /// 判定规则：单次伤害 / 当前血量 > 重伤阈值 → 判定为重伤
    /// </summary>
    private bool IsHeavyDamage(float damage) => damage / stats.GetMaxHealth() > heavyDamageThreshold;
}
