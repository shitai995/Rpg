// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-23 21:54:00
// 版本：V1.1
// 描述：对话发言者配置SO，存储NPC/角色名称与对话头像
// ========================================================
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Speaker Data", fileName = "Speaker - ")]
/// <summary>
/// 对话发言者资源，存放对话显示用名字与头像
/// </summary>
public class DialogueSpeakerSO : ScriptableObject
{
    [Header("发言者名称")]
    public string speakerName;
    [Header("对话头像")]
    public Sprite speakerPortrait;
}