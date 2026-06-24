// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-22 00:27:54
// 版本：V1.1
// 描述：实体血量类
// ========================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IDamgable
{
    public event Action OnTakingDamage;
    public event Action OnHealthUpdate;

    private Slider healthBar;
    private Entity entity;
    private Entity_VFX entityVfx;// 实体特效组件（用于播放受击特效）
    private Entity_Stats entityStats;
    private Entity_DropManager dropManager;

    private bool minHealthBarActive;
    [SerializeField] protected float currentHealth;
    [Header("生命回复")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;
    public float lastDamageTaken { get; private set; }
    public bool isDead { get; private set; }// 死亡状态标记（保护级便于子类重写）
    protected bool canTakeDamage = true;

    [Header("普通受击击退参数")]
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);// 普通击退力度（X水平，Y垂直）
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);// 重伤击退力度（X水平，Y垂直）
    [SerializeField] private float knockbackDuration = .2f;// 普通击退持续时间（秒）
    [SerializeField] private float heavyKnockDuration = .5f;// 重伤击退持续时间（秒）

    [Header("重伤受击击退参数")]
    [SerializeField] private float heavyDamageThreshold = .3f;// 重伤判定阈值（伤害/当前血量 > 该值则判定为重伤）
    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();
        dropManager = GetComponent<Entity_DropManager>();
        SetupHealth();
    }
    protected virtual void Start()
    {

    }
    /// <summary>
    /// 初始化血量与血条
    /// </summary>
    private void SetupHealth()
    {
        if (entityStats == null)
            return;
        currentHealth = entityStats.GetMaxHealth();
        OnHealthUpdate += UpdateHealthBar;

        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    /// <summary>
    /// 处理实体受击逻辑
    /// </summary>
    public virtual bool TakeDamage(float damage,float elementalDamage,ElementType element, Transform damageDealer)
    {
        if (isDead || canTakeDamage == false)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} 闪避成功!");
            return false;
        }

        // 获取攻击者的属性脚本，用于计算攻击者的破甲值；攻击者无属性脚本则破甲值为0
        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;
        // 计算物理护甲减伤比例 → 计算实际受到的物理伤害 (护甲越高，物理伤害越低)
        float mitigation = entityStats != null ? entityStats.GetArmorMitigation(armorReduction) : 0;
        // 计算对应元素抗性比例 → 计算实际受到的元素伤害 (抗性越高，元素伤害越低)
        float resistance = entityStats != null ? entityStats.GetElementalResistance(element) : 0;

        float physicalDamageTaken = damage * (1 - mitigation);
        float elementalDamageTaken = elementalDamage * (1 - resistance);
        // 触发被攻击的击退效果
        TakeKnockback(damageDealer, physicalDamageTaken);
        // 结算总伤害，扣除生命值
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);
        
        lastDamageTaken = physicalDamageTaken + elementalDamageTaken;

        OnTakingDamage?.Invoke();
        return true;
    }
    /// <summary>
    /// 设置是否可受伤
    /// </summary>
    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;
    /// <summary>
    /// 判断是否闪避成功
    /// </summary>
    private bool AttackEvaded()
    {
        if (entityStats == null)
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < entityStats.GetEvasion();
    }
    /// <summary>
    /// 定时生命回复
    /// </summary>
    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
            return;

        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }
    /// <summary>
    /// 治疗：增加血量（不超过上限）
    /// </summary>
    public void IncreaseHealth(float healAmount)
    {
        if (isDead) 
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        OnHealthUpdate?.Invoke();
        
    }
    /// <summary>
    /// 扣减生命值并判断是否死亡
    /// </summary>  
    public void ReduceHealth(float damage)
    {
        // 3. 播放受击特效（空条件运算符：避免组件为空时空引用报错）
        currentHealth -= damage;

        entityVfx?.PlayOnDamageVfx();
        OnHealthUpdate?.Invoke();

        if(currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        entity.EntityDeath();
        dropManager?.DropItems();
    }
    /// <summary>
    /// 获取当前血量百分比
    /// </summary>
    public float GetHealthPercent() => currentHealth / entityStats.GetMaxHealth();
    /// <summary>
    /// 设置血量百分比
    /// </summary>
    public void SetHealthToPercent(float percent)
    {
        currentHealth = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        OnHealthUpdate?.Invoke();
    }
    public float GetCurrentHealth() => currentHealth;
    private void UpdateHealthBar()
    {
        if (healthBar == null || healthBar.transform.parent.gameObject.activeSelf == false)
            return;

        healthBar.value = currentHealth / entityStats.GetMaxHealth();
    }
    public void EnableHealthBar(bool enable) => healthBar?.transform.parent.gameObject.SetActive(enable);
    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        // 1. 计算击退力度和方向（根据是否为重伤 + 伤害来源方向）
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        // 2. 计算击退持续时间（根据是否为重伤）
        float duration = CalculateDuration(finalDamage);


        // 4. 执行击退逻辑（空条件运算符：兼容无Entity组件的情况）
        entity?.ReciveKnockback(knockback, duration);
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
    private bool IsHeavyDamage(float damage)
    {
        if (entityStats == null)
            return false;
        else
            return damage / entityStats.GetMaxHealth() > heavyDamageThreshold;
    }
}
