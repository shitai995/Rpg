// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 21:24:22
// 版本：V1.1
// 描述：敌人僵直状态
// ========================================================

using UnityEngine;

/// <summary>
/// 敌人受击僵直状态
/// </summary>
public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX vfx;

    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        vfx = enemy.GetComponent<Enemy_VFX>();
    }

    /// <summary>
    /// 进入僵直状态
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        vfx.EnableAttackAlert(false);    // 关闭预警特效
        enemy.EnableCounterWindow(false);// 关闭可反击判定

        stateTimer = enemy.stunnedDuration;
        // 施加僵直冲量
        rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDir, enemy.stunnedVelocity.y);
    }

    /// <summary>
    /// 状态帧更新
    /// </summary>
    public override void Update()
    {
        base.Update();
        // 僵直结束，切回待机状态
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}