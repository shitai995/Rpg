// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-02 15:52:28
// 版本：V1.1
// 描述：实体属性核心计算类（整合基础属性、主属性、加成，计算最终战斗数值）
// 核心功能：元素伤害/抗性、物理伤害/暴击、护甲减伤/闪避等战斗数值的最终计算
// ========================================================


using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;


    public Stat_ResourceGroup resources; // 基础最大生命值
    public Stat_OffenseGroup offense; // 进攻属性分组
    public Stat_DefenseGroup defense; // 防御属性分组
    public Stat_MajorGroup major;   // 核心属性分组

    public AttackData GetAttackData(DamageScaleData scaleData)
    {
        return new AttackData(this, scaleData); 
    }
    /// <summary>
    /// 获取最终元素伤害值，输出本次触发的主元素类型
    /// </summary>
    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        // 获取三系基础元素伤害值
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        // 智力提供固定元素伤害加成
        float bonusElementalDamage = major.intelligence.GetValue();
        // 默认取火焰伤害为主属性
        float highestDamage = fireDamage;
        element = ElementType.Fire;
        // 对比替换最高伤害的元素类型
        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }
        // 非主属性的元素伤害，仅提供50%的伤害加成
        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * .5f;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * .5f;
        float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage * .5f;

        // 计算总附加元素伤害 + 智力加成 = 最终元素伤害
        float weakerElementsDamage = bonusFire + bonusIce + bonusLightning;
        float finalDamage = highestDamage + weakerElementsDamage + bonusElementalDamage;

        return finalDamage * scaleFactor;
    }

    /// <summary>
    /// 元素抵抗
    /// </summary>
    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0; ;
        // 智力提供全元素抗性加成
        float bonusResistance = major.intelligence.GetValue() * .5f;
        // 根据元素类型获取对应基础抗性
        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
        }
        // 计算总抗性，上限75%，转换为0~1的系数
        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalResistanec = Mathf.Clamp(resistance, 0, resistanceCap) / 100;

        return finalResistanec;

    }
    /// <summary>
    /// 计算物理最终伤害，输出是否暴击
    /// </summary>
    public float GetPhyiscalDamage(out bool isCrit, float scaleFactor = 1)
    {
        // 1. 计算基础总伤害
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        // 2. 计算最终暴击概率（并添加边界校验，避免异常值）
        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * .3f;
        float critChance = baseCritChance + bonusCritChance;

        // 3. 计算最终暴击倍率（并添加边界校验，避免暴击倍率低于1倍）
        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * .5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        // 4. 优化随机数判定，使用浮点型重载保证精度
        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;
        // 5. 计算并返回最终伤害
        return finalDamage * scaleFactor;
    }
    /// <summary>
    /// 计算护甲减伤比例（受破甲影响）
    /// </summary>
    public float GetArmorMitigation(float armorReduction)
    {
        // 1. 计算总护甲值（基础护甲 + 活力属性带来的护甲加成）
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        // 2. 计算破甲抵消后的有效护甲
        float reductionMutliplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMutliplier;

        // 3. 计算基础护甲减伤比例（经典护甲减伤公式：有效护甲÷（有效护甲+100））
        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = .85f;

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }
    /// <summary>
    /// 获取最终破甲倍率（转换为0~1的系数）
    /// </summary>
    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }
    /// <summary>
    /// 获取最终闪避率（上限85%）
    /// </summary>
    public float GetEvasion()
    {

        // 1. 计算总闪避率（基础闪避率 + 敏捷属性带来的闪避率加成）
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * .5f;

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 85f;
        // 限制敏捷范围
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }
    /// <summary>
    /// 获取最终最大生命值
    /// </summary>
    public float GetMaxHealth()
    {
        float baseMaxHealth = resources.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

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

            default:
                Debug.LogWarning($"StatType {type} not implemented yet.");
                return null;

        }

    }
    [ContextMenu("Update Default Stat Setup")]
    // 把配置文件里的maxHealth值，设为当前实体maxHealth属性的基础值
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }
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
