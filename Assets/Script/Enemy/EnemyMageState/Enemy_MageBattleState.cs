// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 20:40:51
// 版本：V1.1
// 描述：法师敌人战斗状态
// ========================================================

using UnityEngine;

/// <summary>法师敌人战斗状态</summary>
public class Enemy_MageBattleState : Enemy_BattleState
{
    private Enemy_Mage enemyMage;
    private float lastTimeUsedRetreat; // 上次后撤技能释放时间

    public Enemy_MageBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage; // 强转法师专属敌人类
        lastTimeUsedRetreat = float.NegativeInfinity;
    }

    public override void Enter()
    {
        base.Enter();

        // 判断是否需要后撤
        if (ShouldRetreat())
        {
            // 冷却完毕则释放后撤技能，否则重新判断后撤条件
            if (CanUseRetreatAbility())
                Retreat();
            else
                ShouldRetreat();
        }
    }

    // 切换后撤状态并记录释放时间
    private void Retreat()
    {
        lastTimeUsedRetreat = Time.time;
        stateMachine.ChangeState(enemyMage.mageRetreatState);
    }

    // 判断后撤技能是否冷却完成
    private bool CanUseRetreatAbility() => Time.time > lastTimeUsedRetreat + enemyMage.retreatCooldown;
}