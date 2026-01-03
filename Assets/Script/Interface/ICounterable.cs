// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 21:48:21
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public interface ICounterable
{
    public bool CanBeCountered {  get; }
    public void HandleCounter();
}
