// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-21 14:54:00
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class Player_SwordThrowState : PlayerState
{
    private Camera mainCamera;
    public Player_SwordThrowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        skillManager.swordThrow.EnableDots(true);

        if(mainCamera != Camera.main) 
            mainCamera = Camera.main;
    }
    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocity.y);
        player.HandleFlip(dirToMouse.x);
        skillManager.swordThrow.PredictTrajectory(dirToMouse);

        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("swordThrowPerformed", true);

            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.ConfirmTrajectory(dirToMouse);
        }

        if (input.Player.RangeAttack.WasReleasedThisFrame() || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("swordThrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 wordMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = wordMousePosition - playerPosition;

        return direction.normalized;
    }
}
