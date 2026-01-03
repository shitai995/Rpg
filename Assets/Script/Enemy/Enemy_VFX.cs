
// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 22:29:25
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("Counter Attack Window")]
    [SerializeField] private GameObject attackAlert;


    public void EnableAttackAlert(bool enable)
    {
        if (attackAlert == null)
            return;

        attackAlert.SetActive(enable);
    }
}
