// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:30:44
// 版本：V1.1
// 描述：死亡界面UI，提供返回营地、复活至存档点、返回主菜单功能
// ========================================================

using UnityEngine;

/// <summary>
/// 角色死亡界面
/// </summary>
public class UI_DeathScreen : MonoBehaviour
{
    /// <summary>
    /// 返回营地按钮
    /// </summary>
    public void GoToCampBTN()
    {
        // GameManager.instance.ChangeScene("Level_0", RespawnType.NonSpecific);
    }

    /// <summary>
    /// 从存档点重新开始
    /// </summary>
    public void GoToCheckpointBTN()
    {
        // GameManager.instance.RestartScene();
    }

    /// <summary>
    /// 返回主菜单按钮
    /// </summary>
    public void GoToMainMenuBTN()
    {
        // GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
    }
}