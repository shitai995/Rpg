// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 13:29:18
// 版本：V1.1
// 描述：技能基类，定义所有技能的通用逻辑（冷却、解锁、升级、使用检测）
// ========================================================

using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager {  get; private set; }
    public Player player {  get; private set; }

    public DamageScaleData damageScaleData { get; private set; }


    [Header("通用技能参数")]
    [SerializeField] protected SkillType skillType;// 技能类型
    [SerializeField] protected SkillUpgradeType upgradeType;// 技能升级类型
    [SerializeField] protected float cooldown;// 技能冷却时间
    private float lastTimeUsed;// 技能上次使用时间



    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed = lastTimeUsed - cooldown;
        damageScaleData = new DamageScaleData();    
    }
    /// <summary>
    /// 尝试使用技能（虚方法，子类重写实现具体释放逻辑）
    /// 外部调用此方法触发技能释放
    /// </summary>
    public virtual void TryUseSkill()
    {

    }
    /// <summary>
    /// 设置技能升级数据（由技能管理器调用）
    /// 覆盖冷却时间、升级类型、伤害倍率配置
    /// </summary>
    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;  // 更新升级类型
        cooldown = upgrade.cooldown;// 更新冷却时间
        damageScaleData = upgrade.damageScaleData;// 关联伤害/元素效果配置
        ResetCooldown();
    }
    // 检测技能是否可使用
    public virtual bool CanUseSkill()
    {
        // 未升级（未解锁）：不可使用
        if (upgradeType == SkillUpgradeType.None)
            return false;
        // 处于冷却中：不可使用，打印提示
        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }
        return true;
    }
    /// <summary>
    /// 检测技能是否解锁到指定升级类型
    /// </summary>
    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;
    /// <summary>
    /// 检测技能是否处于冷却中
    /// </summary>
    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    /// <summary>
    /// 将技能设为冷却状态（技能释放后调用）
    /// 记录当前时间为上次使用时间
    /// </summary>
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    /// <summary>
    /// 减少技能冷却时间（用于冷却缩减效果）
    /// </summary>
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    // 重置技能冷却
    public void ResetCooldown() => lastTimeUsed = Time.time - cooldown;


}
