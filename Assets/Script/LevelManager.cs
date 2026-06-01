// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:26:28
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string musicGroupName;

    private void Start()
    {
        AudioManager.instance.StartBGM(musicGroupName);
    }
}
