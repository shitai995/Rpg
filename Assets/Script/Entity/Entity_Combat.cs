// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-22 00:01:09
// 版本：V1.1
// 描述：实体战斗核心类
// ========================================================

using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;


    [Header("目标检测参数")]
    [SerializeField] private Transform targetCheck;// 检测圆心位置（挂载空物体精准控制检测点）
    [SerializeField] private float targetCheckRadius = 1;// 检测半径（单位：世界坐标）
    [SerializeField] private LayerMask whatIsTarget;// 目标层级掩码（仅检测指定层级的物体，避免攻击友方/场景）

    [Header("Status effect details")]
    [SerializeField] private float defaultDuration = 3;// 元素状态默认持续时间
    [SerializeField] private float chillSlowMultPlier = .2f;// 元素状态默认持续时间
    [SerializeField] private float electrifyChargeBuildUp = .4f;// 雷电充能叠加值
    [Space]
    [SerializeField] private float fireScale = .8f;// 火焰伤害伤害倍率
    [SerializeField] private float lightningScale = 2.5f;// 雷电伤害倍率
    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();   
        stats = GetComponent<Entity_Stats>();
    }



    /// <summary>
    /// 执行攻击逻辑（对外公开的调用接口）
    /// 流程：检测范围内目标 → 遍历目标 → 对有血量组件的目标扣血
    /// </summary>
    public void PerformAttack()
    {
        // 1. 获取检测范围内所有符合条件的碰撞体
        foreach (var target in GetDetectedColliders())
        {
            // 获取目标身上的受击接口，判定是否可被攻击造成伤害
            IDamgable damegable = target.GetComponent<IDamgable>();
            // 目标无受击接口，跳过该目标
            if (damegable == null)
                continue;
            // 2. 获取元素伤害值+对应元素类型、物理伤害值+暴击判定
            float elementalDamage = stats.GetElementalDamage(out ElementType element,.6f);

            float damage = stats.GetPhyiscalDamage(out bool isCrit, 1.5f); 
            // 3. 给目标施加伤害，返回是否成功命中(是否格挡/闪避/无敌等)
            bool targetGotHit = damegable.TakeDamage(damage, elementalDamage, element, transform);


            // 4. 有元素类型时，为目标施加对应元素状态效果
            if (element != ElementType.None)
                ApplyStatusEffect(target.transform, element);
            // 5. 目标成功受击时，更新特效颜色+生成受击特效
            if (targetGotHit)
            {
                vfx.UpdateOnHitColor(element);
                vfx.CreateOnHitVFX(target.transform, isCrit);
            }
        }
    }
    /// <summary>
    /// 为目标施加对应元素的异常状态效果
    /// 目前实现：冰霜减速效果，后续可扩展火焰灼烧/雷电麻痹
    /// </summary>
    public void ApplyStatusEffect(Transform target,ElementType element,float scaleFactor = 1)
    {
        // 获取目标身上的状态处理器脚本
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        // 目标无状态处理器，直接返回
        if (statusHandler == null)
            return;


        // 判定为冰霜元素 且 目标可被施加状态时 → 施加冰霜减速效果
        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
            statusHandler.ApplyChillEffect(defaultDuration,chillSlowMultPlier);

        if(element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            scaleFactor = fireScale;
            float fireDamage = stats.offense.fireDamage.GetValue() * scaleFactor;
            statusHandler.ApplyBurnEffect(defaultDuration,fireDamage);
        }

        if(element == ElementType.Lightning && statusHandler.CanBeApplied(ElementType.Lightning))
        {
            scaleFactor = lightningScale;
            float lightningDamage = stats.offense.lightningDamage.GetValue() * scaleFactor;
            statusHandler.ApplyElectrifyEffect(defaultDuration, lightningDamage, electrifyChargeBuildUp);
        }
    }
    /// <summary>
    /// 检测指定范围内的目标（2D圆形范围）
    /// 私有方法：封装检测逻辑，便于后续修改检测方式（如矩形、射线）
    /// </summary>
    protected Collider2D[] GetDetectedColliders()
    {
        // 2D物理圆形范围检测：返回所有在范围内且匹配层级掩码的碰撞体
        // 参数：检测圆心、半径、目标层级
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
