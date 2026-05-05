// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-11 20:45:49
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class SKillObject_Health : Entity_Health
{
    protected override void Die()
    {
        SkillObject_TimeEcho timeEcho = GetComponent<SkillObject_TimeEcho>();
        timeEcho.HandleDeath();
    }
}
