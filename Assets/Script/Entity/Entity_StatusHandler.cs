// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-15 22:41:45
// 版本：V1.1
// 描述：实体元素状态效果处理器（冰霜减速、火焰灼烧、雷电充能/雷击）
// ========================================================

using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;
    // 当前身上正存在的元素状态效果，默认无任何状态
    private ElementType currentEffect = ElementType.None;

    [Header("Shock efect details")]
    [SerializeField] private GameObject LightingStrikeVfx;// 雷电充能满后触发的雷击特效预制体
    [SerializeField] private float currentCharge;// 当前雷电充能值（累加至最大值触发雷击）
    [SerializeField] private float maximumCharge = 1;// 雷电充能最大值（达到该值触发雷击）
    private Coroutine shockCo;// 雷电状态协程引用
    private Coroutine burnCo;// 火焰状态协程引用
    private Coroutine chillCo;// 冰霜状态协程引用
    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
    }

    public void RemoveAllNegativeEffects()
    {
        if (shockCo != null) StopCoroutine(shockCo);
        if (burnCo != null) StopCoroutine(burnCo);
        if (chillCo != null) StopCoroutine(chillCo);
        shockCo = null; burnCo = null; chillCo = null;
        currentEffect = ElementType.None;
        entityVfx.StopAllVfx();
    }
    public void ApplyStatusEffect(ElementType element,ElementalEffectData effectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChillEffect(effectData.chillDuration, effectData.chillSlowMultiplier);

        if(element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(effectData.burnDuration,effectData.totalBurnDamage);

        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
    }
    /// <summary>
    /// // 施加雷电充能效果累加充能值，充能满触发雷击；未充满则启动雷电状态特效
    /// </summary>
    private void ApplyShockEffect(float duration, float damage, float charge)
    {
        // 获取雷电元素抗性，计算最终充能值（抗性越高，充能增加越少）
        float lightningResistance = entityStats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        // 累加当前充能值
        currentCharge += finalCharge;
        // 充能值达到最大值 → 触发雷击，停止雷电效果并返回
        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopShockEffect();
            return;
        }
        // 若雷电协程已在运行，先中断（防止协程叠加）
        if (shockCo != null)
            StopCoroutine(shockCo);
        // 启动雷电状态协程，播放特效
        shockCo = StartCoroutine(ShockEffectCo(duration));
    }

    private void StopShockEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllVfx();
    }
    /// <summary>
    /// 触发雷击效果实例化,雷击特效，扣除对应血量
    /// </summary>
    private void DoLightningStrike(float damage)
    {
        Instantiate(LightingStrikeVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(damage);
    }
    // 雷电状态协程
    private IEnumerator ShockEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopShockEffect();
    }
    /// <summary>
    /// 施加火焰灼烧效果
    /// </summary>
    private void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = entityStats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);

        burnCo = StartCoroutine(BurnEffectCo(duration, finalDamage));
    }
    /// <summary>
    /// 火焰灼烧效果
    /// </summary>
    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);
        // 每秒触发伤害的次数
        int ticksPerSecond = 2;
        // 总触发次数
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);
        // 每次触发的伤害值
        float damagePerTick = totalDamage / tickCount;
        // 每次触发的时间间隔（秒）
        float tickInterval = 1f / ticksPerSecond;
        // 循环触发灼烧伤害
        for (int i = 0; i < tickCount; i++)
        {
            if (entityHealth.isDead) break;
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }
    /// <summary>
    /// 施加冰霜减速效果
    /// </summary>
    private void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance);

        chillCo = StartCoroutine(ChillEffectCo(finalDuration, slowMultiplier));
    }
    /// <summary>
    /// 冰霜减速效果协程
    /// </summary>
    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }
    /// <summary>
    /// 判断指定的元素状态效果是否可以成功施加给当前实体
    /// </summary>
    public bool CanBeApplied(ElementType effect)
    {
        // 雷电状态：已有雷电状态时仍可施加（用于叠加充能）
        if (effect == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

        return currentEffect == ElementType.None;
    }

}
