// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 22:05:14
// 版本：V1.1
// 描述：敌人动画触发器类
// ========================================================

using UnityEngine;

public class Enemy_AnimtorTriggers : Entity_AnimationTriggers
{
    private Enemy enemy;
    private Enemy_VFX enemy_Vfx;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemy_Vfx = GetComponentInParent<Enemy_VFX>();
    }
    private void EnableCounterWindow()
    {
        enemy_Vfx.EnableAttackAlert(true);
        enemy.EnableCounterWindow(true);
    }

    private void DisableCounterWindow()
    {
        enemy_Vfx.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);
    }
}
