// ========================================================
// 作者：娇娇 
// 创建时间：2026-02-10 17:19:53
// 版本：V1.1
// 描述：技能树节点组件
// 实现单个技能节点的核心逻辑：解锁/锁定/退款、鼠标交互、提示框触发、连接状态同步
// 是技能树系统的最小交互单元，实现IPointer接口处理鼠标事件
// ========================================================

using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;

    [Header("解锁规则配置")]
    public UI_TreeNode[] neededNodes;// 前置技能节点数组
    public UI_TreeNode[] conflictNodes;// 冲突技能节点数组
    public bool isUnlocked;// 是否已解锁
    public bool isLocked;// 是否被锁定

    [Header("技能数据配置")]
    public SkillDataSO skillData;// 技能数据
    [SerializeField] private string skillName; // 技能名称
    [SerializeField] private Image skillIcon;// 技能图标Image组件
    [SerializeField] private int skillCost;// 技能解锁消耗
    [SerializeField] private string lockedColorHex = "#8A8A8A";// 锁定状态的颜色
    private Color lastcolor;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();    
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();
        // 初始化图标颜色为锁定状态
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void Start()
    {
        // 若技能配置为默认解锁，则自动解锁
        if (skillData.unlockedByDefault)
            Unlock();
    }
    /// <summary>
    /// 技能退款：重置解锁/锁定状态，返还技能点，更新连接线
    /// </summary>
    public void Refund()
    {
        if (isUnlocked == false || skillData.unlockedByDefault)
            return;

        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));
        // 返还技能点
        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(false);

    }
    /// <summary>
    /// 解锁技能：更新状态、消耗技能点、锁定冲突节点、同步连接线
    /// </summary>
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();// 锁定所有冲突节点

        skillTree.RemoveSkillPoints(skillData.cost);// 消耗技能点
        connectHandler.UnlockConnectionImage(true);// 更新连接线为已解锁状态
        // 同步解锁状态到玩家技能管理器
        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData); 
    }
    /// <summary>
    /// 校验技能是否可解锁（核心条件判断）
    /// 条件：未锁定+未解锁+技能点足够+前置节点全部解锁+冲突节点未解锁
    /// </summary>
    private bool CanBeUnlocked()
    {
        if(isLocked || isUnlocked)
            return false;

        if(skillTree.EnoughSkillPoints(skillData.cost) == false)
            return false;

        foreach(var node in neededNodes)
        {
            if(node.isUnlocked == false)
                return false;
        }

        foreach(var node in conflictNodes)
        {
            if(node.isUnlocked)
                return false;
        }
        return true;
    }
    /// <summary>
    /// 锁定冲突节点：解锁当前节点后，递归锁定所有冲突节点及其子节点
    /// </summary>
    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }
    /// <summary>
    /// 递归锁定子节点（冲突锁定时调用）
    /// </summary>
    public void LockChildNodes()
    {
        isLocked = true;

        foreach(var node in connectHandler.GetChildNodes())
            node.LockChildNodes();
    }
    /// <summary>
    /// 更新技能图标颜色（记录上一次颜色，便于悬浮恢复）
    /// </summary>
    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;
        lastcolor = skillIcon.color;
        skillIcon.color = color;
    }
    // 鼠标点击节点时触发
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            ui.skillToolTip.LockedSkillEffect();
    }
    // 鼠标悬浮到节点时触发
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect,this);

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(true);  
    }
    // 鼠标离开节点时触发
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(false);
    }
    /// <summary>
    /// 切换节点高亮状态（悬浮反馈）
    /// </summary>
    public void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * .9f; highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastcolor;

        UpdateIconColor(colorToApply);
    }
    /// <summary>
    /// Hex颜色码转Color（封装ColorUtility，简化调用）
    /// </summary>
    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);
        return color;
    }
    /// <summary>
    /// 组件禁用时：恢复图标颜色（避免状态异常）
    /// </summary>
    private void OnDisable()
    {
        if(isLocked)
            UpdateIconColor(GetColorByHex(lockedColorHex));
        if(isUnlocked)
            UpdateIconColor(Color.white);
    }
    /// <summary>
    /// 编辑器校验：同步skillData到组件配置，提升开发效率
    /// 仅在编辑器模式下生效，运行时不执行
    /// </summary>
    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost; 
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
