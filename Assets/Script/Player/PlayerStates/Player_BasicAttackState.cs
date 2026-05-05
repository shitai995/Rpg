// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-30 22:09:01
// 版本：V1.1
// 描述：玩家基础攻击状态（三段连招）
// 支持三段连击、攻击方向跟随、连招排队、超时重置连招逻辑
// ========================================================

using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    #region 攻击相关计时器与状态标记
    private float attackVelocityTimer; // 攻击移动速度计时器（控制攻击位移持续时间）
    private float lastTimeAttacked;    // 上次攻击时间（用于连招超时判断）

    private bool comboAttackQueued;    // 连招排队标记（攻击中按下攻击键，排队触发下一段）
    private int attackDir;             // 攻击方向（跟随输入或当前面向）
    private int comboIndex = 1;        // 当前连招索引（1-3段）
    private int comboLimit = 3;        // 最大连招段数（默认3段）
    private const int FirstComboIndex = 1; // 初始连招索引（固定为1）
    #endregion

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        // 校验：连招段数与攻击速度数组长度是否匹配（避免数组越界）
        if (comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("攻击模组数不匹配！连招段数与attackVelocity数组长度不一致");
            comboLimit = player.attackVelocity.Length; // 强制同步为数组长度，防止报错
        }
    }

    public override void Enter()
    {
        base.Enter(); // 调用基类：激活攻击动画、重置triggerCalled标记

        comboAttackQueued = false; // 重置连招排队标记
        ResetComboIndexIfNeeded(); // 校验并重置连招索引（超时/超段数时重置为第1段）
        SyncAttackSpeed();
        // 确定攻击方向：有水平输入则跟随输入，无则沿用当前面向
        attackDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;

        // 通知动画控制器当前连招段数（播放对应攻击动画）
        anim.SetInteger("basicAttackIndex", comboIndex);
        // 应用当前段攻击的位移速度
        ApplyAtackVelocity();
    }

    public override void Update()
    {
        base.Update(); // 保留基类：计时器、冲刺检测等通用逻辑

        HandleAttackVelocity(); // 更新攻击位移计时器，到期后停止位移

        // 攻击同时格挡
        //if (input.Player.CounterAttack.WasPerformedThisFrame())
        //   stateMachine.ChangeState(player.countAttackState);

        // 检测攻击输入：攻击中按下攻击键，触发连招排队
        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        // 攻击动画关键帧触发后，处理状态退出（切换下一段连招或返回闲置）
        if (triggerCalled)
            HandleStartExit();
    }

    public override void Exit()
    {
        base.Exit(); // 关闭攻击动画布尔参数

        comboIndex++; // 连招索引+1（准备下一段攻击）
        lastTimeAttacked = Time.time; // 记录当前攻击时间（用于超时判断）
    }

    /// <summary>
    /// 处理攻击状态退出逻辑
    /// </summary>
    private void HandleStartExit()
    {
        if (comboAttackQueued)
        {
            // 有排队连招：延迟切换到下一段攻击（避免动画冲突）
            anim.SetBool(animBoolName, false);
            player.EnterAtackStateWithDelay();
        }
        else
        {
            // 无排队连招：返回闲置状态
            stateMachine.ChangeState(player.idleState);
        }
    }

    /// <summary>
    /// 连招排队：当前攻击中按下攻击键，标记排队（未到最大段数时生效）
    /// </summary>
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    /// <summary>
    /// 处理攻击位移：计时器到期后停止水平位移，保留竖直速度（如空中攻击下坠）
    /// </summary>
    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    /// <summary>
    /// 应用当前段攻击的位移速度（根据连招索引获取对应配置）
    /// </summary>
    private void ApplyAtackVelocity()
    {
        // 获取当前连招段的攻击位移速度（数组索引从0开始，故-1）
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration; // 重置位移计时器
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y); // 应用速度（含方向）
    }

    /// <summary>
    /// 连招索引重置判断：超最大段数 或 超时未攻击，重置为第1段
    /// </summary>
    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > lastTimeAttacked + player.comboResetTime)
            comboIndex = FirstComboIndex;

        if (comboIndex > comboLimit)
            comboIndex = FirstComboIndex;
    }
}