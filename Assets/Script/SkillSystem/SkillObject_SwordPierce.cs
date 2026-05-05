// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-27 19:48:49
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class SkillObject_SwordPierce : SkillObject_Sword
{
    private int amountTopPierce;

    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);
        amountTopPierce = swordManager.amountToPierce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (amountTopPierce <= 0 || groundHit)
        {
            DamageEnemiesInRadius(transform, .3f);
            StopSword(collision);
            return;
        }

        amountTopPierce--;
        DamageEnemiesInRadius(transform, .3f);

    }
}
