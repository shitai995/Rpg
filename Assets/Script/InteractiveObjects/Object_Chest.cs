// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 20:31:08
// 版本：V1.1
// 描述：宝箱交互类（实现IDamagable接口，受击后打开并触发物理效果）
// ========================================================

using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamgable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponentInChildren<Entity_VFX>();

    private Entity_DropManager dropManager => GetComponent<Entity_DropManager>();

    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;
    [SerializeField] private bool canDropItems = true;
    public bool TakeDamage(float damage,float elementalDamage,ElementType element, Transform damageDealer)
    {
        if (canDropItems == false)
            return false;

        canDropItems = false;
        dropManager?.DropItems();
        // 1. 播放宝箱受击特效（如闪红、震动等）
        fx.PlayOnDamageVfx();
        // 2. 触发宝箱开启动画
        anim.SetBool("chestOpen", true);
        // 3. 给刚体添加击退速度（实现受击后位移效果）
        rb.linearVelocity = knockback;
        // 4. 给刚体添加随机旋转速度（-200~200度/秒，模拟受击旋转效果）
        rb.angularVelocity = Random.Range(-200f, 200f);

        return true;
    }

}
