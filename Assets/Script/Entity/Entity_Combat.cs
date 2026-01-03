// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-22 00:01:09
// 版本：V1.1
// 描述：实体战斗核心类
// ========================================================

using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;


    [Header("目标检测参数")]
    [SerializeField] private Transform targetCheck;// 检测圆心位置（挂载空物体精准控制检测点）
    [SerializeField] private float targetCheckRadius = 1;// 检测半径（单位：世界坐标）
    [SerializeField] private LayerMask whatIsTarget;// 目标层级掩码（仅检测指定层级的物体，避免攻击友方/场景）


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
            IDamgable damable = target.GetComponent<IDamgable>();

            if (damable == null)
                continue;

            float damage = stats.GetPhyiscalDamage(out bool isCrit);
            bool targetGotHit =  damable.TakeDamage(damage, transform);

            if (targetGotHit)
                vfx.CreateOnHitVFX(target.transform,isCrit);
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
