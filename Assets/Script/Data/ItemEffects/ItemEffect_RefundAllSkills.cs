// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 15:10:15
// 版本：V1.1
// 描述：重置所有技能点（洗点）道具效果
// ========================================================

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Refund all skills", fileName = "Item effect data - Refund all skills")]

public class ItemEffect_RefundAllSkills : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.RefundAllskills();
    }
}