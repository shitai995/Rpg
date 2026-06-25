// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-23 21:56:13
// 版本：V1.1
// 描述：对话面板UI，实现打字机文字、分支选择、对话结束触发商店/任务/领奖等行为
// ========================================================
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC对话界面核心控制脚本
/// </summary>
public class UI_Dialogue : MonoBehaviour
{
    private UI ui;
    private DialogueNpcData npcData;                // 当前对话NPC绑定任务、领奖类型
    private Player_QuestManager questManager;        // 任务管理器

    [Header("对话基础显示组件")]
    [SerializeField] private Image speakerPortrait; // 说话人头像
    [SerializeField] private TextMeshProUGUI speakerName; // 说话人名称
    [SerializeField] private TextMeshProUGUI dialogueText; // 对话正文
    [SerializeField] private TextMeshProUGUI[] dialogueChoicesText; // 玩家选项文本数组

    [Header("打字机参数")]
    [Space]
    [SerializeField] private float textSpeed = .1f; // 文字打字间隔
    private string fullTextToShow;                   // 当前完整台词
    private Coroutine typeTextCo;                   // 打字协程

    private DialogueLineSO currentLine;    // 当前播放对话节点
    private DialogueLineSO[] currentChoices; // 当前分支选项集合
    private DialogueLineSO selectedChoice; // 选中的分支对话
    private int selectedChoiceIndex;       // 当前选中选项下标

    private bool waitingToConfirm; // 文字播完等待点击确认标记
    private bool canInteract;      // 是否允许点击交互

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        questManager = Player.instance.questManager;
    }

    /// <summary>
    /// 传入NPC任务与领奖配置数据
    /// </summary>
    public void SetupNpcData(DialogueNpcData npcData) => this.npcData = npcData;

    /// <summary>
    /// 播放一段对话节点
    /// </summary>
    public void PlayDialogueLine(DialogueLineSO line)
    {
        currentLine = line;
        currentChoices = line.choiceLines;
        canInteract = false;
        selectedChoice = null;
        selectedChoiceIndex = 0;

        HideAllChoices();
        // 刷新头像、名称
        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        // 判断文字来源：普通对话随机台词 / 行为参数文本
        fullTextToShow = line.actionType == DialogueActionType.None || line.actionType == DialogueActionType.PlayerMakeChoice
            ? line.GetRandomLine()
            : line.actionLine;

        typeTextCo = StartCoroutine(TypeTextCo(fullTextToShow));
        StartCoroutine(EnableInteractionCo());
    }

    /// <summary>
    /// 根据当前对话行为执行对应逻辑（商店/任务面板/领奖/关闭对话等）
    /// </summary>
    private void HandleNextAction()
    {
        switch (currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToInGameUI();
                ui.OpenMerchantUI(true);
                break;
            case DialogueActionType.PlayerMakeChoice:
                if (selectedChoice == null)
                    ShowChoices(); // 未选择则展示选项
                else
                    PlayDialogueLine(currentChoices[selectedChoiceIndex]); // 播放选中分支
                break;
            case DialogueActionType.OpenQuest:
                ui.SwitchToInGameUI();
                ui.OpenQuestUI(npcData.quests);
                break;
            case DialogueActionType.GetQuestReward:
                ui.SwitchToInGameUI();
                questManager.TryGetRewardFrom(npcData.npcRewardType);
                break;
            case DialogueActionType.OpenCraft:
                ui.SwitchToInGameUI();
                ui.OpenCraftUI(true);
                break;
            case DialogueActionType.CloseDialogue:
                ui.SwitchToInGameUI();
                break;
        }
    }

    /// <summary>
    /// 玩家点击对话面板交互按钮/确认键
    /// </summary>
    public void DialogueInteraction()
    {
        if (!canInteract) return;

        // 打字动画未结束：直接一次性显示全部文字
        if (typeTextCo != null)
        {
            CompleteTyping();
            // 非分支选择行为，等待二次确认
            if (currentLine.actionType != DialogueActionType.PlayerMakeChoice)
                waitingToConfirm = true;
            else
                HandleNextAction();
            return;
        }

        // 文字已播完，确认后执行后续行为
        if (waitingToConfirm || selectedChoice != null)
        {
            waitingToConfirm = false;
            HandleNextAction();
        }
    }

    /// <summary>
    /// 停止打字协程，直接填充完整文字
    /// </summary>
    private void CompleteTyping()
    {
        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text = fullTextToShow;
            typeTextCo = null;
        }
    }

    /// <summary>
    /// 渲染所有玩家分支选项，选中项标黄
    /// </summary>
    private void ShowChoices()
    {
        for (int i = 0; i < dialogueChoicesText.Length; i++)
        {
            if (i < currentChoices.Length)
            {
                DialogueLineSO choice = currentChoices[i];
                string choiceText = choice.playerChoiceAnswer;

                dialogueChoicesText[i].gameObject.SetActive(true);
                // 选中文字黄色高亮
                dialogueChoicesText[i].text = selectedChoiceIndex == i
                    ? $"<color=yellow> {i + 1}) {choiceText}"
                    : $"{i + 1}) {choiceText}";
            }
            else
            {
                dialogueChoicesText[i].gameObject.SetActive(false);
            }
        }
        selectedChoice = currentChoices[selectedChoiceIndex];
    }

    /// <summary>
    /// 隐藏全部选择文本
    /// </summary>
    private void HideAllChoices()
    {
        foreach (var txt in dialogueChoicesText)
            txt.gameObject.SetActive(false);
    }

    /// <summary>
    /// 切换选项上下选择（键盘/手柄方向键调用）
    /// </summary>
    /// <param name="direction">1下一项 / -1上一项</param>
    public void NavigateChoice(int direction)
    {
        if (currentChoices == null || currentChoices.Length <= 1) return;
        selectedChoiceIndex += direction;
        selectedChoiceIndex = Mathf.Clamp(selectedChoiceIndex, 0, currentChoices.Length - 1);
        ShowChoices();
    }

    /// <summary>
    /// 打字机逐字输出协程
    /// </summary>
    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if (currentLine.actionType != DialogueActionType.PlayerMakeChoice)
        {
            waitingToConfirm = true;
        }
        else
        {
            yield return new WaitForSeconds(.2f);
            selectedChoice = null;
            HandleNextAction();
        }
        typeTextCo = null;
    }

    /// <summary>
    /// 延迟一帧开启交互权限，防止台词刚弹出立刻触发点击
    /// </summary>
    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract = true;
    }
}