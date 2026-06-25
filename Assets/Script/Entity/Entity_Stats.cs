// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-02 15:52:28
// 版本：V1.1
// 描述：实体属性核心计算类（整合基础属性、主属性、加成，计算最终战斗数值）
// 核心功能：元素伤害/抗性、物理伤害/暴击、护甲减伤/闪避等战斗数值的最终计算
// ========================================================

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Entity_Stats : MonoBehaviour
{
    // 默认属性配置表
    public StatSetupDataSO defaultStatSetup;

    // 属性分组
    public Stat_ResourceGroup resources;    // 生命/资源类
    public Stat_OffenseGroup offense;       // 攻击类
    public Stat_DefenseGroup defense;       // 防御类
    public Stat_MajorGroup major;           // 主属性（力/敏/智/体）

    protected virtual void Awake() { }

    public void AdiustStatSetup(Stat_ResourceGroup resourceGroup,Stat_OffenseGroup offenseGroup,Stat_DefenseGroup defenseGroup,float penalty,float increase)
    {

        offense.damage.SetBaseValue(offenseGroup.damage.GetValue() * increase);
        offense.attackSpeed.SetBaseValue(offenseGroup.attackSpeed.GetValue() * increase);
        offense.critChance.SetBaseValue(offenseGroup.critChance.GetValue() * increase);
        offense.critPower.SetBaseValue(offenseGroup.critPower.GetValue() * increase);
        offense.fireDamage.SetBaseValue(offenseGroup.fireDamage.GetValue() * increase); 
        offense.iceDamage.SetBaseValue(offenseGroup.iceDamage.GetValue() * increase);
        offense.lightningDamage.SetBaseValue(offenseGroup.lightningDamage.GetValue() * increase);

        defense.evasion.SetBaseValue(defenseGroup.evasion.GetValue() * increase);

        resources.maxHealth.SetBaseValue(resourceGroup.maxHealth.GetValue() * penalty);
        resources.healthRegen.SetBaseValue(resourceGroup.healthRegen.GetValue() * penalty);

        defense.armor.SetBaseValue(defenseGroup.armor.GetValue() * penalty);
        defense.lightningRes.SetBaseValue(defenseGroup.lightningRes.GetValue() * penalty);
        defense.fireRes.SetBaseValue(defenseGroup.fireRes.GetValue() * penalty);
        defense.iceRes.SetBaseValue(defenseGroup.iceRes.GetValue() * penalty);

    }

    // 获取攻击数据（伤害+类型）
    public AttackData GetAttackData(DamageScaleData scaleData)
    {
        return new AttackData(this, scaleData);
    }

    // 计算最终元素伤害（最高元素全额，其余50% + 智力加成）
    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        float fireDmg = offense.fireDamage.GetValue();
        float iceDmg = offense.iceDamage.GetValue();
        float lightningDmg = offense.lightningDamage.GetValue();
        float intBonus = major.intelligence.GetValue();

        // 找出最高伤害元素
        float highest = fireDmg;
        element = ElementType.Fire;

        if (iceDmg > highest) { highest = iceDmg; element = ElementType.Ice; }
        if (lightningDmg > highest) { highest = lightningDmg; element = ElementType.Lightning; }

        if (highest <= 0) { element = ElementType.None; return 0; }

        // 非最高元素只算50%
        float bonusFire = fireDmg == highest ? 0 : fireDmg * .5f;
        float bonusIce = iceDmg == highest ? 0 : iceDmg * .5f;
        float bonusLightning = lightningDmg == highest ? 0 : lightningDmg * .5f;

        float total = highest + bonusFire + bonusIce + bonusLightning + intBonus;
        return total * scaleFactor;
    }

    // 获取元素抗性（智力提供额外抗性，上限75%）
    public float GetElementalResistance(ElementType element)
    {
        float baseRes = 0;
        float intBonus = major.intelligence.GetValue() * .5f;

        switch (element)
        {
            case ElementType.Fire: baseRes = defense.fireRes.GetValue(); break;
            case ElementType.Ice: baseRes = defense.iceRes.GetValue(); break;
            case ElementType.Lightning: baseRes = defense.lightningRes.GetValue(); break;
        }

        float res = Mathf.Clamp(baseRes + intBonus, 0, 75);
        return res / 100;
    }

    // 计算物理伤害（含暴击判定）
    public float GetPhyiscalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float dmg = GetBaseDamage();
        float critChance = GetCritChance();
        float critPower = GetCritPower() / 100;

        isCrit = Random.Range(0, 100) < critChance;
        float final = isCrit ? dmg * critPower : dmg;
        return final * scaleFactor;
    }

    // 基础物理伤害 = 攻击力 + 力量
    public float GetBaseDamage() => offense.damage.GetValue() + major.strength.GetValue();
    // 暴击率 = 基础值 + 敏捷*0.3
    public float GetCritChance() => offense.critChance.GetValue() + major.agility.GetValue() * .3f;
    // 暴击伤害 = 基础值 + 力量*0.5
    public float GetCritPower() => offense.critPower.GetValue() + major.strength.GetValue() * .5f;

    // 计算护甲减伤（受破甲影响，上限85%）
    public float GetArmorMitigation(float armorReduction)
    {
        float armor = GetBaseArmor();
        float mult = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = armor * mult;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        return Mathf.Clamp(mitigation, 0, .85f);
    }

    // 护甲 = 基础值 + 活力
    public float GetBaseArmor() => defense.armor.GetValue() + major.vitality.GetValue();

    // 获取破甲百分比
    public float GetArmorReduction() => offense.armorReduction.GetValue() / 100;

    // 闪避率 = 基础值 + 敏捷*0.5，上限85%
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float agiBonus = major.agility.GetValue() * .5f;
        return Mathf.Clamp(baseEvasion + agiBonus, 0, 85);
    }

    // 最大生命 = 基础值 + 活力*5
    public float GetMaxHealth()
    {
        float baseHp = resources.maxHealth.GetValue();
        float vitBonus = major.vitality.GetValue() * 5;
        return baseHp + vitBonus;
    }

    // 根据类型获取对应属性
    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.IceResistance: return defense.iceRes;
            case StatType.FireResistance: return defense.fireRes;
            case StatType.LightningResistance: return defense.lightningRes;

            default: Debug.LogWarning($"StatType {type} 未实现"); return null;
        }
    }

    // 应用默认属性配置
    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null) return;

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
    }
}