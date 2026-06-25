// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 22:57:11
// 版本：V1.1
// 描述：法师施法状态
// ========================================================

using UnityEngine;

/// <summary>法师释放法术状态</summary>
public class Enemy_MageSpellCastState : EnemyState
{
    private Enemy_Mage enemyMage; // 法师敌人专属引用

    public Enemy_MageSpellCastState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage; // 强转法师实体
    }

    public override void Enter()
    {
        base.Enter();
        enemyMage.SetVelocity(0, 0); // 施法时静止不动
        enemyMage.SetSpellCastPerformed(false); // 重置施法完成标记
    }

    public override void Update()
    {
        base.Update();
        // 标记施法完成，切换对应动画参数
        if (enemyMage.spellCastPerformed)
            anim.SetBool("spellCast_performed", true);
        // 动画触发结束信号，切回战斗状态
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("spellCast_performed", false); // 退出状态重置动画参数
    }
}