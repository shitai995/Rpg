// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-11 18:23:19
// 版本：V1.1
// 描述：时间残影技能（分身、攻击、治疗精灵、冷却缩减）
// ========================================================

using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    [Header("攻击强化")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = .3f;

    [Header("治疗精灵强化")]
    [SerializeField] private float damagePercentHealed = .3f;
    [SerializeField] private float cooldownReducedInSeconds;

    // 获取治疗百分比
    public float GetPercentOfDamageHealed()
    {
        if (!ShouldBeWisp()) return 0;
        return damagePercentHealed;
    }

    // 获取冷却缩减
    public float GetCooldownReduceInSeconds()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_CooldownWisp) return 0;
        return cooldownReducedInSeconds;
    }

    // 是否能清除负面效果
    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp;
    }

    // 是否生成治疗/净化/减CD精灵
    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_HealWisp
            || upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp
            || upgradeType == SkillUpgradeType.TimeEcho_CooldownWisp;
    }

    // 获取分身复制概率
    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_ChanceToDuplicate) return 0;
        return duplicateChance;
    }

    // 获取最大攻击次数
    public int GetMaxAttacks()
    {
        if (upgradeType == SkillUpgradeType.TimeEcho_SingleAttack || upgradeType == SkillUpgradeType.TimeEcho_ChanceToDuplicate)
            return 1;

        if (upgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
            return maxAttacks;

        return 0;
    }

    // 获取残影持续时间
    public float GetEchoDuration()
    {
        return timeEchoDuration;
    }

    // 尝试使用技能
    public override void TryUseSkill()
    {
        if (CanUseSkill() == false) 
            return;
        CreateTimeEcho();
        SetSkillOnCooldown();
    }

    // 创建时间残影
    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}