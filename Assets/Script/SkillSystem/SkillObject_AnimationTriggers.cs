// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-13 20:36:18
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class SkillObject_AnimationTriggers : MonoBehaviour
{
    private SkillObject_TimeEcho timeEcho;

    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    private void AttackTrigger()
    {
        timeEcho.PerformAttack();
    }

    private void TryTerminate(int currentAttackIndex)
    {
        if(currentAttackIndex == timeEcho.maxAttacks) 
            timeEcho.HandleDeath();
    }
}
