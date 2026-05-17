// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-07 14:44:06
// 版本：V1.1
// 描述：技能数据配置类
// ========================================================

using UnityEngine;
using System;
[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")] 
public class SkillDataSO : ScriptableObject 
{
    

    [Header("技能描述")]
    public string displayName;// 技能显示名称
    [TextArea]
    public string description;// 技能描述文本
    public Sprite icon;// 技能图标

    [Header("解锁与升级配置")]
    public int cost;// 技能解锁/升级所需消耗
    public bool unlockedByDefault;// 是否默认解锁
    public SkillType skillType;// 技能类型
    public UpgradeData upgradeData;// 技能升级数据

}
/// <summary>
/// 技能升级数据类（可序列化）
/// 封装单级升级的核心参数，作为Skill_DataSO的子配置
/// </summary>
[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;// 升级类型
    public float cooldown;// 升级后的技能冷却时间
    public DamageScaleData damageScaleData;// 升级后的伤害/元素效果倍率配置
}