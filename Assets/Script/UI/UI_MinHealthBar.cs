
// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-28 18:34:44
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class UI_MinHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        entity.OnFlipped += HandleFlip;
    }


    private void OnDisable()
    {
        entity.OnFlipped -= HandleFlip;
    }
    private void HandleFlip() => transform.rotation = Quaternion.identity;
}




