// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 23:39:06
// 版本：V1.1
// 描述：敌人移动状态类 - 处理敌人常规移动、碰壁/无地面判断、状态切换逻辑
// ========================================================

using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 无地面（即将掉落） 或 检测到墙体 → 翻转敌人朝向（避免继续向危险方向移动）
        if (enemy.groundDetected == false || enemy.wallDetected)   
            enemy.Flip();
    }
    public override void Update()
    {
        base.Update();
        // 设置移动速度：向当前面向方向以常规移动速度移动（保持Y轴速度不变）
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);
        // 无地面（即将掉落） 或 检测到墙体 → 停止移动，切换到闲置状态
        if (enemy.groundDetected == false || enemy.wallDetected)
            stateMachine.ChangeState(enemy.idleState);
    }
}
