// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 20:31:08
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Chest : MonoBehaviour, IDamgable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponentInChildren<Entity_VFX>();

    [SerializeField] private Vector2 knockback;
    public bool TakeDamage(float damage, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
        anim.SetBool("chestOpen", true);  
        rb.linearVelocity = knockback;
        rb.angularVelocity = Random.Range(-200f, 200f);

        return true;
    }

}
