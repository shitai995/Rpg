 // ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 22:31:03
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;

    [Header("移动配置")]
    public float idleTime = 2f;// 等待时间
    public float moveSpeed = 1.4f; // 移动速度
    [Range(0,2)]
    public float moveAnimSpeedMultiplier = 1;// 适配移动速度与动画速度

}
