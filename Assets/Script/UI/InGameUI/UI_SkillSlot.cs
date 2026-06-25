// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:31:09
// 版本：V1.1
// 描述：技能栏格子UI，管理图标、按键显示、冷却遮罩与悬浮提示
// ========================================================

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 技能格子UI
/// </summary>
public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    private Image skillIcon;
    private RectTransform rect;
    private Button button;

    private SkillDataSO skillData;

    public SkillType skillType;
    [SerializeField] private Image cooldownImage;        // 冷却遮罩
    [SerializeField] private string inputKeyName;        // 快捷键名称
    [SerializeField] private TextMeshProUGUI inputKeyText;// 快捷键文本
    [SerializeField] private GameObject conflictSlot;     // 冲突标识物体

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        button = GetComponent<Button>();
        skillIcon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    // 编辑器内自动修改物体名称
    private void OnValidate()
    {
        gameObject.name = $"UI_SkillSlot - {skillType}";
    }

    /// <summary>
    /// 初始化技能格子数据
    /// </summary>
    public void SetupSkillSlot(SkillDataSO selectedSkill)
    {
        skillData = selectedSkill;

        // 初始化冷却遮罩样式
        Color darkColor = Color.black;
        darkColor.a = 0.6f;
        cooldownImage.color = darkColor;

        inputKeyText.text = inputKeyName;
        skillIcon.sprite = selectedSkill.icon;

        //conflictSlot?.SetActive(false);
    }

    /// <summary>
    /// 开启技能冷却
    /// </summary>
    public void StartCooldown(float cooldown)
    {
        cooldownImage.fillAmount = 1f;
        StartCoroutine(CooldownCoroutine(cooldown));
    }

    /// <summary>
    /// 重置冷却状态
    /// </summary>
    public void ResetCooldown() => cooldownImage.fillAmount = 0f;

    /// <summary>
    /// 冷却协程
    /// </summary>
    private IEnumerator CooldownCoroutine(float duration)
    {
        float timePassed = 0f;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            cooldownImage.fillAmount = 1f - timePassed / duration;
            yield return null;
        }
        cooldownImage.fillAmount = 0f;
    }

    /// <summary>
    /// 鼠标移出，关闭技能提示
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, null);
    }

    /// <summary>
    /// 鼠标移入，弹出技能提示
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillData == null) return;
        ui.skillToolTip.ShowToolTip(true, rect, skillData, null);
    }
}