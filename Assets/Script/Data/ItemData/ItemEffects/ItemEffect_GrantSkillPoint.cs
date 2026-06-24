// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 20:41:54
// 版本：V1.1
// 描述：增加技能点道具效果实现类
// ========================================================

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Grand skill point", fileName = "Item effect data - Grant Skill Point")]

public class ItemEffect_GrantSkillPoint : ItemEffect_DataSO
{
    [Tooltip("增加的技能点数量")]
    [SerializeField] private int pointsToAdd;

    /// <summary>
    /// 执行效果：给玩家添加技能点
    /// </summary>
    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        // 调用技能树UI添加技能点
        ui.skillTreeUI.AddSkillPoints(pointsToAdd);
    }
}