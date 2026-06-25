// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 20:28:36
// 版本：V1.1
// 描述：法师后撤闪现状态
// ========================================================

using UnityEngine;

/// <summary>法师后撤技能状态</summary>
public class Enemy_MageRetreatState : EnemyState
{
    private Enemy_Mage enemyMage;
    private Vector3 startPosition; // 后撤起始坐标
    private Transform player;      // 玩家目标

    public Enemy_MageRetreatState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage; // 强转为法师实体
    }

    public override void Enter()
    {
        base.Enter();

        // 缓存玩家对象
        if (player == null)
            player = enemy.GetPlayerReference();

        startPosition = enemy.transform.position;
        // 朝远离玩家方向高速后撤
        rb.linearVelocity = new Vector2(enemyMage.retreatSpeed * -DirectionToPlayer(), 0);
        enemy.HandleFlip(DirectionToPlayer());

        // 切换不可被选中层级，开启残影特效
        enemy.gameObject.layer = LayerMask.NameToLayer("Untargetable");
        enemy.vfx.DoImageEchoEffect(1f);
    }

    public override void Update()
    {
        base.Update();
        // 判断是否后撤到最大距离
        bool rechedMaxDistance = Vector2.Distance(enemy.transform.position, startPosition) > enemyMage.retreatMaxDistance;

        // 后撤距离达标/可终止后撤时，切施法状态
        if (rechedMaxDistance || enemyMage.CanMoveBackwards())
            stateMachine.ChangeState(enemyMage.mageSpellCastState);
    }

    public override void Exit()
    {
        base.Exit();
        // 关闭残影，恢复敌人可选中层级
        enemy.vfx.StopImageEchoEffect();
        enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    // 获取玩家相对自身的左右方向
    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}