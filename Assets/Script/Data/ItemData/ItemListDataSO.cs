// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:22:07
// 版本：V1.1
// 描述：物品总列表配置表
// ========================================================

using System.Linq;
using UnityEngine;
using UnityEditor;
/// <summary>
/// 全局物品数据列表
/// </summary>
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item list", fileName = "List of items - ")]
public class ItemListDataSO : ScriptableObject
{
    [Tooltip("所有物品数据集合")]
    public ItemDataSO[] itemList;

    /// <summary>
    /// 根据ID查找物品数据
    /// </summary>
    /// <param name="saveId">物品唯一ID</param>
    /// <returns>对应物品数据</returns>
    public ItemDataSO GetItemData(string saveId)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveId == saveId);
    }

#if UNITY_EDITOR
    /// <summary>
    /// 右键菜单：自动收集所有物品配置
    /// </summary>
    [ContextMenu("Auto-fill with all ItemDataSO")]
    public void CollectItemsData()
    {
        // 查找所有ItemDataSO资源
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");

        // 加载并赋值到列表
        itemList = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        // 标记变更并保存
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}