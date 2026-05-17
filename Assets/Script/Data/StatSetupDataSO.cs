// ========================================================
// 作者：娇娇 
// 创建时间：2026-02-05 22:54:18
// 版本：V1.1
// 描述：实体属性配置脚本对象（SO），用于预设不同角色/怪物的基础属性值
// 用途：可在编辑器中创建多个实例，配置玩家/小怪/BOSS的差异化属性
// ========================================================

using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "RPG Setup/Default stat setup",fileName = "Default Stat Setup")]
public class StatSetupDataSO : ScriptableObject
{
    [Header("基础资源属性")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("进攻属性 - 物理伤害")]
    public float attackSpeed = 1;// 攻击速度
    public float damage = 10; // 基础物理伤害值
    public float critChance;// 暴击概率
    public float critPower = 150;// 暴击倍率
    public float armorReduction;// 护甲穿透/减免值

    [Header("进攻属性 - 元素伤害")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("防御属性 - 物理伤害")]
    public float armor;// 物理护甲值
    public float evasion;// 闪避概率

    [Header("防御属性 - 元素伤害")]
    public float fireResistance;// 火焰抗性
    public float iceResistance;// 冰霜抗性
    public float lightningResistance;// 雷电抗性

    [Header("核心主属性")]
    public float strength;// 力量
    public float agility;// 敏捷
    public float intelligence;// 智力
    public float vitality;// 活力


}
