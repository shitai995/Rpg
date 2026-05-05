// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-24 20:37:31
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Player_AnimationTriggers :Entity_AnimationTriggers
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }

    private void ThrowSword() => player.skillManager.swordThrow.ThrowSword();
}
