// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-24 14:43:01
// 版本：V1.1
// 描述：史莱姆敌人主体类，死亡分裂小史莱姆、支持玩家反击眩晕
// ========================================================

using UnityEngine;

/// <summary>史莱姆敌人主逻辑，实现可反击接口</summary>
public class Enemy_Slime : Enemy, ICounterable
{
    // 是否能被玩家反击眩晕
    public bool CanBeCountered => canBeStunned;
    // 史莱姆专属死亡状态
    public Enemy_SlimeDeadState slimeDeadState { get; set; }

    [Header("史莱姆专属配置")]
    [SerializeField] private GameObject slimeToCreatePrefab; // 分裂小史莱姆预制体
    [SerializeField] private int amountOfSlimesToCreate = 2;  // 死亡分裂数量
    [SerializeField] private bool hasRecoveryAnimation = true;// 是否带眩晕恢复动画

    protected override void Awake()
    {
        base.Awake();
        // 初始化通用敌人状态
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
        // 初始化史莱姆专属死亡状态
        slimeDeadState = new Enemy_SlimeDeadState(this, stateMachine, "idle");

        anim.SetBool("hasStunRecovery", hasRecoveryAnimation);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); // 初始进入闲置状态
    }

    /// <summary>重写死亡逻辑，切换史莱姆专属死亡状态</summary>
    public override void EntityDeath()
    {
        stateMachine.ChangeState(slimeDeadState);
    }

    /// <summary>被玩家反击触发，进入眩晕状态</summary>
    public void HandleCounter()
    {
        if (!CanBeCountered)
            return;
        stateMachine.ChangeState(stunnedState);
    }

    /// <summary>死亡时生成分裂小史莱姆，调整属性并自动进入战斗</summary>
    public void CreateSlimeOnDeath()
    {
        if (slimeToCreatePrefab == null)
            return;

        for (int i = 0; i < amountOfSlimesToCreate; i++)
        {
            GameObject newSlime = Instantiate(slimeToCreatePrefab, transform.position, Quaternion.identity);
            Enemy_Slime slimeScript = newSlime.GetComponent<Enemy_Slime>();

            // 缩小小史莱姆攻防属性
            slimeScript.stats.AdiustStatSetup(stats.resources, stats.offense, stats.defense, 0.6f, 1.2f);
            slimeScript.ApplyRespawnCelocity();
            slimeScript.StartBattleStateCheck(player);
        }
    }

    /// <summary>给分裂史莱姆赋予随机弹跳初速度</summary>
    public void ApplyRespawnCelocity()
    {
        Vector2 velocity = new Vector2(stunnedVelocity.x * Random.Range(-1f, 1f), stunnedVelocity.y * Random.Range(1f, 2f));
        SetVelocity(velocity.x, velocity.y);
    }

    /// <summary>生成后立刻尝试进入战斗，定时重试</summary>
    public void StartBattleStateCheck(Transform player)
    {
        TryEnterBattleState(player);
        InvokeRepeating(nameof(ReEnterBattleState), 0, 0.3f);
    }

    /// <summary>定时检测，未进入战斗则强制切战斗状态，进入后停止轮询</summary>
    private void ReEnterBattleState()
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            CancelInvoke(nameof(ReEnterBattleState));
            return;
        }
        stateMachine.ChangeState(battleState);
    }
}