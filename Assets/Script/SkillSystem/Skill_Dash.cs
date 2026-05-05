// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 13:42:02
// 版本：V1.1
// 描述：冲刺技能子类
// ========================================================

using UnityEngine;

public class Skill_Dash : Skill_Base
{
    /// <summary>
    /// 冲刺开始时触发的效果（由动画/逻辑层调用）
    /// 根据技能升级类型，判断是否生成分身/碎片
    /// </summary>
    public void OnStartEffect()
    {
        // 条件1：解锁「冲刺开始生成分身」 或 「冲刺开始和结束都生成分身」
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStart) || Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.Dash_ShardOnShart) || Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }
    /// <summary>
    /// 冲刺结束时触发的效果（由动画/逻辑层调用）
    /// 根据技能升级类型，判断是否生成分身/碎片
    /// </summary>
    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }
    // 生成碎片
    private void CreateShard()
    {
        skillManager.shard.CreateRawShard();
    }
    // 生成分身
    private void CreateClone()
    {
        skillManager.timeEcho.CreateTimeEcho();
    }
}
