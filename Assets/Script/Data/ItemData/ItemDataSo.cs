// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 11:54:44
// 版本：V1.1
// 描述：所有道具的基础数据类（材料/消耗品/装备通用）
// ========================================================

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item", fileName = "Material data - ")]

public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("道具效果")]
    [Tooltip("道具使用时执行的效果")]
    public ItemEffect_DataSO itemEffect;
}