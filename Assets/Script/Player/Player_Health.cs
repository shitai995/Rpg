// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:29:40
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            Die();
    }

    protected override void Die()
    {
        base.Die();


       // player.ui.OpenDeathScreenUI();
        //GameManager.instance.SetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();

       
    }
}
