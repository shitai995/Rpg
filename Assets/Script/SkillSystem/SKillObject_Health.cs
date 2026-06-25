// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-11 20:45:49
// 版本：V1.1
// 描述：技能实体生命值管理
// ========================================================

using UnityEngine;

/// <summary>
/// 时间回响技能实体血量逻辑
/// </summary>
public class SKillObject_Health : Entity_Health
{
    /// <summary>
    /// 实体死亡处理
    /// </summary>
    protected override void Die()
    {
        SkillObject_TimeEcho timeEcho = GetComponent<SkillObject_TimeEcho>();
        timeEcho.HandleDeath();
    }
}