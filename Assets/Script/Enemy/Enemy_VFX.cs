// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 22:29:25
// 版本：V1.1
// 描述：敌人特效管理
// ========================================================

using UnityEngine;

/// <summary>
/// 敌人特效组件
/// </summary>
public class Enemy_VFX : Entity_VFX
{
    [Header("反击预警")]
    [SerializeField] private GameObject attackAlert;

    /// <summary>
    /// 开关攻击预警特效
    /// </summary>
    public void EnableAttackAlert(bool enable)
    {
        if (attackAlert == null) return;
        attackAlert.SetActive(enable);
    }
}