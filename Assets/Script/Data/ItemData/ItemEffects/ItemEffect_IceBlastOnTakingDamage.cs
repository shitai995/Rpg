// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 20:43:21
// 版本：V1.1
// 描述：低血量受击时触发冰霜爆炸效果
// ========================================================

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice blast", fileName = "Item effect data - Ice blast on taking damage")]

public class ItemEffect_IceBlastOnTakingDamage : ItemEffect_DataSO
{
    [Tooltip("冰元素效果数据（减速/冻结等）")]
    [SerializeField] private ElementalEffectData effectData;

    [Tooltip("冰霜爆炸冰属性伤害值")]
    [SerializeField] private float iceDamage;

    [Tooltip("敌人层级（用于范围检测）")]
    [SerializeField] private LayerMask whatIsEnemy;

    [Space]
    [Tooltip("触发效果的血量百分比阈值")]
    [SerializeField] private float healthPercentTrigger = .25f;

    [Tooltip("效果冷却时间")]
    [SerializeField] private float cooldown;

    private float lastTimeUsed = -999;

    [Header("Vfx Objects")]
    [Tooltip("冰霜爆炸特效")]
    [SerializeField] private GameObject iceBlastVfx;

    [Tooltip("命中敌人特效")]
    [SerializeField] private GameObject onHitVfx;

    /// <summary>
    /// 执行效果：受击时检查血量与冷却，触发冰爆
    /// </summary>
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

        // 满足冷却+血量阈值时触发冰爆
        if (noCooldown && reachedThreshold)
        {
            player.vfx.CreateEffectOf(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    /// <summary>
    /// 范围冰伤检测：对周围敌人造成冰伤+冰元素效果
    /// </summary>
    private void DamageEnemiesWithIce()
    {
        // 圆形范围检测敌人
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            IDamgable damagable = target.GetComponent<IDamgable>();
            if (damagable == null) continue;

            // 造成冰属性伤害
            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);

            // 施加冰元素状态效果（减速/冻结）
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            // 命中后播放击中特效
            if (targetGotHit)
                player.vfx.CreateEffectOf(onHitVfx, target.transform);
        }
    }

    /// <summary>
    /// 绑定玩家，注册受击监听事件
    /// </summary>
    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }

    /// <summary>
    /// 解绑玩家，注销受击监听（防内存泄漏）
    /// </summary>
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}