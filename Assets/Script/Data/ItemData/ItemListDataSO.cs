// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:22:07
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item list", fileName = "List of items - ")]

public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;
}
