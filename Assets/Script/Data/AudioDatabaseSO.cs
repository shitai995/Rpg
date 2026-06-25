// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:28:08
// 版本：V1.1
// 描述：音频资源总配置表
// ========================================================

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频数据库，统一管理所有音效、音乐资源
/// </summary>
[CreateAssetMenu(menuName = "Audio/Audio Database")]
public class AudioDatabaseSO : ScriptableObject
{
    [Tooltip("玩家相关音效")]
    public List<AudioClipData> player;
    [Tooltip("UI界面音效")]
    public List<AudioClipData> uiAudio;

    [Header("背景音乐列表")]
    public List<AudioClipData> mainMenuMusic;
    public List<AudioClipData> levelMusic;

    // 音频字典，通过名称快速查找
    private Dictionary<string, AudioClipData> clipCollection;

    /// <summary>
    /// 初始化音频字典
    /// </summary>
    private void OnEnable()
    {
        clipCollection = new Dictionary<string, AudioClipData>();

        AddToCollection(player);
        AddToCollection(uiAudio);
        AddToCollection(mainMenuMusic);
        AddToCollection(levelMusic);
    }

    /// <summary>
    /// 根据名称获取音频数据
    /// </summary>
    public AudioClipData Get(string groupName)
    {
        return clipCollection.TryGetValue(groupName, out var data) ? data : null;
    }

    /// <summary>
    /// 将音频列表加入字典
    /// </summary>
    private void AddToCollection(List<AudioClipData> listToAdd)
    {
        foreach (var data in listToAdd)
        {
            if (data != null && !clipCollection.ContainsKey(data.audioName))
            {
                clipCollection.Add(data.audioName, data);
            }
        }
    }
}

/// <summary>
/// 单组音频数据
/// </summary>
[System.Serializable]
public class AudioClipData
{
    public string audioName;          // 音频标识名
    public List<AudioClip> clips;     // 音频片段集合
    [Range(0f, 1f)] public float maxVolume = 1f; // 最大音量

    /// <summary>
    /// 随机获取一个音频片段
    /// </summary>
    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Count == 0)
            return null;

        return clips[Random.Range(0, clips.Count)];
    }
}