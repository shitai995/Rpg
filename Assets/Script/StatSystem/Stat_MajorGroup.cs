// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-02 16:29:07
// 版本：V1.1
// 描述：核心主属性分组类（整合力量/敏捷/智力/活力，关联战斗数值加成）
// ========================================================

using System;
using UnityEngine;
[Serializable]
public class Stat_MajorGroup
{
    public Stat strength;   // 力量  +  伤害
    public Stat agility;    // 敏捷  +  暴击率
    public Stat intelligence; // 智力  +  
    public Stat vitality;   // 活力  +  生命值
}
