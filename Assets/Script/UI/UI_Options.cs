// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:30:44
// 版本：V1.1
// 描述：游戏设置面板，控制音量、血条显示，支持本地数据存取
// ========================================================

using System;
using UnityEngine;
using UnityEngine.Audio;
//using UnityEngine.AudioMixer;
using UnityEngine.UI;

/// <summary>
/// 游戏设置界面
/// </summary>
public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthBarToggle;        // 血条显示开关

    [SerializeField] private AudioMixer audioMixer;         // 音频混合器
    [SerializeField] private float mixerMultiplier = 25;    // 音量换算系数

    [Header("背景音乐音量")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string bgmParametr;           // 混合器BGM参数名

    [Header("音效音量")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParametr;           // 混合器音效参数名

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBarToggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }

    /// <summary>
    /// 调整背景音乐音量
    /// </summary>
    public void BGMSliderValue(float value)
    {
        float newValue = MathF.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParametr, newValue);
    }

    /// <summary>
    /// 调整音效音量
    /// </summary>
    public void SFXSliderValue(float value)
    {
        float newValue = MathF.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParametr, newValue);
    }

    /// <summary>
    /// 切换血条显示状态
    /// </summary>
    private void OnHealthBarToggleChanged(bool isOn)
    {
        player.health.EnableHealthBar(isOn);
    }

    // 切换回主菜单
     public void GoMainMenuBTN() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);

    private void OnEnable()
    {
        // 启用面板时读取本地音量配置
         sfxSlider.value = PlayerPrefs.GetFloat(sfxParametr, 0.6f);
         bgmSlider.value = PlayerPrefs.GetFloat(bgmParametr, 0.6f);
    }

    private void OnDisable()
    {
        // 关闭面板时保存音量配置
         PlayerPrefs.SetFloat(sfxParametr, sfxSlider.value);
         PlayerPrefs.SetFloat(bgmParametr, bgmSlider.value);
    }

    /// <summary>
    /// 加载本地音量配置
    /// </summary>
    public void LoadUpVolume()
    {
         sfxSlider.value = PlayerPrefs.GetFloat(sfxParametr, 0.6f);
         bgmSlider.value = PlayerPrefs.GetFloat(bgmParametr, 0.6f);
    }
}