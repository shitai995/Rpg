// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 13:46:25
// 版本：V1.1
// 描述：精灵弓箭手敌人战斗状态逻辑
// ========================================================

using UnityEngine;

/// <summary>精灵弓箭手战斗状态</summary>
public class Enemy_ArcherElfBattleState : Enemy_BattleState
{
    private bool canFlip;          // 是否允许转向玩家
    private bool reachedDeadEnd;   // 是否走到地形死角

    public Enemy_ArcherElfBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        reachedDeadEnd = false; // 进入战斗重置死角标记
    }

    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();

        // 无路可走标记：无地面/撞墙
        if (!enemy.groundDetected || enemy.wallDetected)
            reachedDeadEnd = true;

        // 发现玩家，刷新目标与战斗持续计时
        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        // 战斗超时切回闲置
        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        // 满足攻击冷却
        if (CanAttack())
        {
            // 丢失玩家且允许转向时翻转朝向
            if (!enemy.PlayerDetected() && canFlip)
            {
                enemy.HandleFlip(DirectionToPlayer());
                canFlip = false;
            }
            enemy.SetVelocity(0, rb.linearVelocity.y); // 原地站立

            // 玩家在攻击范围且可见，切攻击状态
            if (WithinAttackRange() && enemy.PlayerDetected())
            {
                canFlip = true;
                lastTimeAttacked = Time.time;
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        // 未到攻击冷却，执行拉扯走位
        else
        {
            // 没卡地形且离玩家过近，向后拉开距离
            bool shouldWalkAway = !reachedDeadEnd && DistanceToPlayer() < enemy.attackDistance * 0.85f;

            if (shouldWalkAway)
            {
                // 反向远离玩家移动
                enemy.SetVelocity(enemy.GetBattleMoveSpeed() * -1 * DirectionToPlayer(), rb.linearVelocity.y);
            }
            else
            {
                enemy.SetVelocity(0, rb.linearVelocity.y); // 原地停留
                // 丢失玩家自动转向目标
                if (!enemy.PlayerDetected())
                    enemy.HandleFlip(DirectionToPlayer());
            }
        }
    }
}