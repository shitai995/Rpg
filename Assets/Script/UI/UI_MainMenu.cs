// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:30:44
// 版本：V1.1
// 描述：主菜单界面逻辑，初始化音量、过渡动画、背景音乐及按钮事件
// ========================================================

using UnityEngine;

/// <summary>
/// 游戏主菜单控制器
/// </summary>
public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        // 加载音量配置
        transform.root.GetComponentInChildren<UI_Options>(true).LoadUpVolume();
        // 执行画面淡入
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();
        // 播放主菜单背景音乐
        //AudioManager.instance.StartBGM("playlist_mainMenu");
    }

    /// <summary>
    /// 开始游戏按钮
    /// </summary>
    public void PlayBTN()
    {
        //AudioManager.instance.PlayGlobalSFX("button_click");
        // GameManager.instance.ContinuePlay();
    }

    /// <summary>
    /// 退出游戏按钮
    /// </summary>
    public void QuitGameBTN()
    {
        Application.Quit();
    }
}