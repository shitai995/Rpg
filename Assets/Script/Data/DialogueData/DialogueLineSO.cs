// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-23 21:53:43
// 版本：V1.1
// 描述：单段对话台词配置SO，存储说话人、台词、分支选项、对话触发行为
// ========================================================
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Line Data", fileName = "Line - ")]
/// <summary>
/// 对话单节点资源，一段完整台词/对话分支
/// </summary>
public class DialogueLineSO : ScriptableObject
{
    [Header("对话基础信息")]
    public string dialogueGroupName;    // 对话分组名，用于区分剧情片段
    public DialogueSpeakerSO speaker;   // 说话人配置（头像、名称）

    [Header("台词文本")]
    [TextArea] public string[] textLine;// 多句台词数组

    [Header("玩家分支选项")]
    [TextArea] public string playerChoiceAnswer; // 玩家选择显示文字
    public DialogueLineSO[] choiceLines;         // 选择后跳转的后续对话节点

    [Header("对话结束触发行为")]
    [TextArea] public string actionLine;   // 行为参数文本
    public DialogueActionType actionType;  // 行为类型（接任务、传送、开门等）

    /// <summary>
    /// 获取第一段台词
    /// </summary>
    public string GetFirstLine() => textLine[0];

    /// <summary>
    /// 随机返回一条台词（用于重复对话）
    /// </summary>
    public string GetRandomLine()
    {
        return textLine[Random.Range(0, textLine.Length)];
    }
}