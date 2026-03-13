// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 13:46:27
// 版本：V1.1
// 描述：玩家技能管理器
// ========================================================

using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
    }
    /// <summary>
    ///  根据技能类型匹配对应的技能实例
    /// </summary>
    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;

            default:
                Debug.Log($"Skill type {type} is not implemented yet.");
                return null;
        }


    }
}
