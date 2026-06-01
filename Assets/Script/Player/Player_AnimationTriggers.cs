// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-24 20:37:31
// 版本：V1.1
// 描述：玩家动画事件触发器
// ========================================================

using UnityEngine;

/// <summary>
/// 玩家动画回调，绑定动画帧事件
/// </summary>
public class Player_AnimationTriggers : Entity_AnimationTriggers
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }

    /// <summary>
    /// 动画事件：执行掷剑技能
    /// </summary>
    private void ThrowSword() => player.skillManager.swordThrow.ThrowSword();
}