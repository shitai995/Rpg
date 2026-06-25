// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-24 23:37:40
// 版本：V1.1
// 描述：史莱姆死亡状态
// ========================================================

using UnityEngine;

/// <summary>史莱姆死亡状态</summary>
public class Enemy_SlimeDeadState : Enemy_DeadState
{
    private Enemy_Slime enemySlime; // 史莱姆敌人专属引用

    public Enemy_SlimeDeadState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        enemySlime = enemy as Enemy_Slime; // 强转为史莱姆实体
    }

    public override void Enter()
    {
        base.Enter();
        enemySlime.CreateSlimeOnDeath(); // 死亡时生成小史莱姆分身
    }
}