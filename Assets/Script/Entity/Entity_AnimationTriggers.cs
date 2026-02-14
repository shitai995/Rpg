// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-30 22:21:49
// 版本：V1.1
// 描述：动画触发回调类（绑定到Animator所在对象）
// 作用：接收动画事件，通知玩家状态机标记触发完成，避免重复攻击等重复行为
// ========================================================

using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;
    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }

    /// <summary>
    /// 动画事件回调方法（在Animator中手动绑定到对应动画帧）
    /// 触发时通知Player，标记当前状态的触发已执行（避免重复攻击）
    /// </summary>
    private void CurrentStateTriger()
    {
        // 调用Player的触发标记方法，间接通知当前状态机状态已触发
        entity.CurrentStateAnimationTrigger();
    }
    private void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}