
// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-28 18:34:44
// 版本：V1.1
// 描述：最小血条UI组件
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
        if (entity == null)
            return;

        entity.OnFlipped += HandleFlip;
    }


    private void OnDisable()
    {
        if (entity == null)
            return;

        entity.OnFlipped -= HandleFlip;
    }
    // 翻转实体的缩放（左右翻转）
    private void HandleFlip() => transform.rotation = Quaternion.identity;
}




