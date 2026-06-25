// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-25 12:12:41
// 版本：V1.1
// 描述：保存游戏接口
// ========================================================

using UnityEngine;

public interface ISaveable
{
    public void LoadData(GameData data);
    public void SaveData(ref GameData data);
}
