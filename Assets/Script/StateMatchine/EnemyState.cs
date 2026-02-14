// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 22:32:37
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy,StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
        stats = enemy.stats;
    }

    public override void Update()
    {
        base.Update();

    }

    /// <summary>
    /// 更新动画参数：同步敌人移动、速度等状态到动画组件
    /// （核心：让动画和实际移动速度匹配）
    /// </summary>
    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        // 计算战斗状态下的动画速度倍率（战斗移动速度 / 基础移动速度）
        float battleAnimSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;
        // 给动画组件传参：战斗动画速度倍率（让动画速度匹配战斗移速）
        anim.SetFloat("battleAnimSpeedMultiplier", battleAnimSpeedMultiplier);
        // 给动画组件传参：基础移动动画速度倍率（自定义调整动画播放速度）
        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
        // 给动画组件传参：刚体X轴速度（用于播放左右移动动画）
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }
}
