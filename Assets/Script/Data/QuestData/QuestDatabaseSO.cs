// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-22 15:28:18
// 版本：V1.1
// 描述：全局任务数据库SO，存储全部任务配置，提供ID查询、编辑器一键收集任务
// ========================================================
using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "RPG Setup/ Quest Data/ Quest Database", fileName = "QuestDatabase")]
/// <summary>
/// 所有QuestDataSO总仓库，存档加载时通过ID匹配任务
/// </summary>
public class QuestDatabaseSO : ScriptableObject
{
    [Header("全部任务配置集合")]
    public QuestDataSO[] allQuests;

    /// <summary>
    /// 根据存档唯一ID查找对应任务SO
    /// </summary>
    public QuestDataSO GetQuestById(string id)
    {
        return allQuests.FirstOrDefault(q => q != null && q.questSaveId == id);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all QuestDataSO")]
    /// <summary>
    /// 编辑器右键工具：自动项目内全部QuestDataSO填充进数组
    /// </summary>
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:QuestDataSO");

        allQuests = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<QuestDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(q => q != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}