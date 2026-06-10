// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:26:28
// 版本：V1.1
// 描述：关卡管理器，进入关卡时自动播放对应分组背景音乐
// ========================================================

using UnityEngine;

/// <summary>
/// 关卡管理组件
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private string musicGroupName; // 关卡对应背景音乐分组名

    private void Start()
    {
        // 启动当前关卡专属BGM
        if (AudioManager.instance != null)
            AudioManager.instance.StartBGM(musicGroupName);
    }
}