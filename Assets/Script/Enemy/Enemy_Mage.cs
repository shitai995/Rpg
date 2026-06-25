// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 18:42:29
// 版本：V1.1
// 描述：法师敌人主体类，包含后撤、多段施法、反制眩晕、身后地形检测逻辑
// ========================================================

using System.Collections;
using UnityEngine;

/// <summary>法师敌人主逻辑，实现可被反击接口</summary>
public class Enemy_Mage : Enemy, ICounterable
{
    // 是否可被反击眩晕
    public bool CanBeCountered => canBeStunned;
    // 法师专属状态只读引用
    public Enemy_MageRetreatState mageRetreatState { get; private set; }
    public Enemy_MageBattleState mageBattleState { get; private set; }
    public Enemy_MageSpellCastState mageSpellCastState { get; private set; }

    [Header("法师专属配置")]
    [SerializeField] private GameObject spellPrefab;       // 法术弹丸预制体
    [SerializeField] private Transform spellStartPosition; // 施法生成点
    [SerializeField] private int amountToCast = 3;         // 连续施法数量
    [SerializeField] private float spellCastCooldown = 0.3f;// 单次施法间隔
    public bool spellCastPerformed { get; private set; }   // 施法流程完成标记

    [Space]
    public float retreatCooldown = 5;      // 后撤技能冷却
    public float retreatMaxDistance = 8;   // 后撤最大距离
    public float retreatSpeed = 15;         // 后撤移动速度
    [SerializeField] private Transform behindCollsionCheck; // 身后地形检测点
    [SerializeField] private bool hasRecoveryAnimation = true; // 是否拥有眩晕恢复动画

    protected override void Awake()
    {
        base.Awake();
        // 初始化通用敌人状态
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        // 初始化法师专属状态，替换父类战斗状态
        mageSpellCastState = new Enemy_MageSpellCastState(this, stateMachine, "spellCast");
        mageRetreatState = new Enemy_MageRetreatState(this, stateMachine, "battle");
        mageBattleState = new Enemy_MageBattleState(this, stateMachine, "battle");
        battleState = mageBattleState;

        // 配置眩晕恢复动画开关
        anim.SetBool("hasStunRecovery", hasRecoveryAnimation);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); // 初始进入闲置状态
    }

    /// <summary>修改施法完成标记</summary>
    public void SetSpellCastPerformed(bool performed) => spellCastPerformed = performed;

    /// <summary>重写特殊攻击，启动连续施法协程</summary>
    public override void SpecialAttack()
    {
        StartCoroutine(CastSpellCo());
    }

    /// <summary>连续释放多段法术弹丸，带释放间隔</summary>
    private IEnumerator CastSpellCo()
    {
        for (int i = 0; i < amountToCast; i++)
        {
            Enemy_MageProjectile projectile
                = Instantiate(spellPrefab, spellStartPosition.position, Quaternion.identity).GetComponent<Enemy_MageProjectile>();

            projectile.SetupProjectile(player.transform, combat);
            yield return new WaitForSeconds(spellCastCooldown);
        }
        SetSpellCastPerformed(true);
    }

    /// <summary>被玩家反击，切换眩晕状态</summary>
    public void HandleCounter()
    {
        if (!CanBeCountered)
            return;
        stateMachine.ChangeState(stunnedState);
    }

    /// <summary>检测身后是否有墙/无地面，判断能否继续后撤</summary>
    public bool CanMoveBackwards()
    {
        // 后方墙体检测
        bool detectedWall = Physics2D.Raycast(behindCollsionCheck.position, Vector2.right * -facingDir, 1.5f, whatIsGround);
        // 脚下地面检测
        bool noGround = !Physics2D.Raycast(behindCollsionCheck.position, Vector2.down, 1.5f, whatIsGround);
        return detectedWall || noGround;
    }

    /// <summary>绘制身后地形检测调试射线</summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // 后方墙体检测线
        Gizmos.DrawLine(behindCollsionCheck.position,
            new Vector3(behindCollsionCheck.position.x + (1.5f * -facingDir), behindCollsionCheck.position.y));
        // 下方地面检测线
        Gizmos.DrawLine(behindCollsionCheck.position,
            new Vector3(behindCollsionCheck.position.x, behindCollsionCheck.position.y - 1.5f));
    }
}