// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 22:31:03
// 版本：V1.1
// 描述：
// ========================================================

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;

    [Header("战斗配置")]
    public float battleMoveSpeed = 3; // 攻击移动速度
    public float attackDistance = 2; // 攻击力距离
    public float battleTimeDuration = 5; // 战斗状态持续时长（超时退出战斗）
    public float minRetreatDistance = 1; // 攻击后最小后撤距离（防止贴脸）
    public Vector2 retreatVelocity; // 后撤时的速度向量（控制后撤方向/力度）

    [Header("移动配置")]
    public float idleTime = 2f;// 等待时间
    public float moveSpeed = 1.4f; // 移动速度
    [Range(0,2)]
    public float moveAnimSpeedMultiplier = 1;// 适配移动速度与动画速度


    [Header("检测玩家")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;// 玩家检测的射线起点（挂载点）
    [SerializeField] private float playerCheckDistance = 10;// 玩家检测距离



    /// <summary>
    /// 射线检测
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D PlayerDetected()
    {
        // 发射2D射线：从检测点出发，沿敌人面向方向，检测指定距离内的玩家/地面图层
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);
        // 过滤：无碰撞体 或 碰撞体不是玩家图层 → 返回默认值（未检测到玩家）
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;
        // 检测到玩家 → 返回检测结果
        return hit;
    }

    /// <summary>
    /// 绘制Gizmos辅助线（编辑器可视化检测范围）
    /// 作用：在Scene视图显示检测射线，方便调试距离参数
    /// </summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 检测玩家
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance),playerCheck.position.y));

        // 攻击距离检测
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));
        // 攻击后撤距离检测
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minRetreatDistance), playerCheck.position.y));
    }
}
