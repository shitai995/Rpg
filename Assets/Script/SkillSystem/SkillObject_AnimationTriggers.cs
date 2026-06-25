// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-13 20:36:18
// 版本：V1.1
// 描述：技能动画事件触发器
// ========================================================

using UnityEngine;

/// <summary>
/// 时间回响技能动画回调
/// </summary>
public class SkillObject_AnimationTriggers : MonoBehaviour
{
    private SkillObject_TimeEcho timeEcho;

    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    /// <summary>
    /// 动画事件：执行攻击
    /// </summary>
    private void AttackTrigger()
    {
        timeEcho.PerformAttack();
    }

    /// <summary>
    /// 动画事件：判断并结束技能
    /// </summary>
    private void TryTerminate(int currentAttackIndex)
    {
        if (currentAttackIndex == timeEcho.maxAttacks)
            timeEcho.HandleDeath();
    }
}