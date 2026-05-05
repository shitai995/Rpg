// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-20 15:18:55
// 版本：V1.1
// 描述：敌人战斗状态类 - 处理追击玩家、后撤、攻击判断、战斗超时等核心逻辑
// ========================================================

using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;// 目标玩家的Transform（追击/攻击的对象）
    private Transform lastTarget;
    private float lastTimeWasInBattle;// 最后检测到玩家的时刻
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        // 首次进入战斗状态时，获取检测到的玩家对象
        player ??= enemy.GetPlayerReference();// 等同于  if(player == null    player = enemy.GetPlayerReference();


        // 如果距离玩家过近（小于最小后撤距离），执行后撤逻辑
        if (ShouldRetreat())
        {
            // 设置后撤速度（反向远离玩家）
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            // 翻转敌人朝向（始终面向玩家）
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();
        // 如果检测到玩家，更新“最后检测到玩家”的时间（重置战斗计时）
        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }
        // 战斗超时（长时间没检测到玩家）→ 切换回闲置状态
        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);
        // 玩家在攻击范围内且能检测到 → 切换到攻击状态
        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else
            // 不在攻击范围 → 向玩家方向移动（追击）
            enemy.SetVelocity(enemy.GetBattleMoveSpeed() * DirectionToPlayer(), rb.linearVelocity.y);
    }
    /// <summary>
    /// 更新敌人检测切换玩家或分身
    /// </summary>
    private void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;

        Transform newTarget = enemy.PlayerDetected().transform;

        if(newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;

        }

    }
    /// <summary>
    /// 更新战斗计时器：记录最后检测到玩家的时刻
    /// </summary>
    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    /// <summary>
    /// 判断战斗是否超时
    /// 逻辑：当前时间 > 最后检测到玩家的时间 + 战斗持续时长 → 超时
    /// </summary>
    /// <returns>超时返回true，未超时返回false</returns>
    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    /// <summary>
    /// 判断是否在攻击范围内
    /// </summary>
    /// <returns>玩家距离 < 攻击距离 返回true</returns>
    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    /// <summary>
    /// 判断是否需要后撤（距离玩家过近）
    /// </summary>
    /// <returns>玩家距离 < 最小后撤距离 返回true</returns>
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    /// <summary>
    /// 得到敌人与玩家距离
    /// </summary>
    /// <returns></returns>
    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    /// <summary>
    /// 得到玩家相对敌人方向
    /// </summary>
    /// <returns></returns>
    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
