// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:26:28
// 版本：V1.1
// 描述：全局音频管理器，统一控制背景音乐切换、淡入淡出、全局/场景音效播放
// ========================================================

using System.Collections;
using UnityEngine;

/// <summary>
/// 音频总管理器（单例）
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDatabaseSO audioDB;  // 音频资源数据库
    [SerializeField] private AudioSource bgmSource;   // 背景音乐播放器
    [SerializeField] private AudioSource sfxSource;   // 全局音效播放器

    private Transform player;
    private AudioClip lastMusicPlayed;
    private string currentBgmGroupName;
    private Coroutine currentBgmCo;
    [SerializeField] private bool bgmShouldPlay;

    private void Awake()
    {
        // 单例初始化
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // BGM播放完毕且允许播放时，自动切下一首
        if (!bgmSource.isPlaying && bgmShouldPlay)
        {
            if (!string.IsNullOrEmpty(currentBgmGroupName))
                NextBGM(currentBgmGroupName);
        }

        // 停止播放时，淡出背景音乐
        if (bgmSource.isPlaying && !bgmShouldPlay)
            StopBGM();
    }

    /// <summary>
    /// 启动指定分组背景音乐
    /// </summary>
    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        if (musicGroup == currentBgmGroupName) return;

        NextBGM(musicGroup);
    }

    /// <summary>
    /// 切换同分组下一首BGM
    /// </summary>
    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentBgmGroupName = musicGroup;

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);

        currentBgmCo = StartCoroutine(SwitchMusicCoroutine(musicGroup));
    }

    /// <summary>
    /// 停止并淡出背景音乐
    /// </summary>
    public void StopBGM()
    {
        bgmShouldPlay = false;
        StartCoroutine(FadeVolumeCoroutine(bgmSource, 0f, 1f));

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);
    }

    /// <summary>
    /// 切换背景音乐协程（淡出旧音乐 → 播放并淡入新音乐）
    /// </summary>
    private IEnumerator SwitchMusicCoroutine(string musicGroup)
    {
        AudioClipData data = audioDB.Get(musicGroup);
        if (data == null || data.clips.Count == 0)
        {
            Debug.LogWarning($"未找到分组音频：{musicGroup}");
            yield break;
        }

        AudioClip nextMusic = data.GetRandomClip();
        // 避免连续播放同一首曲目
        if (data.clips.Count > 1)
        {
            while (nextMusic == lastMusicPlayed)
                nextMusic = data.GetRandomClip();
        }

        // 淡出当前音乐
        if (bgmSource.isPlaying)
            yield return FadeVolumeCoroutine(bgmSource, 0f, 1f);

        // 播放新音乐并淡入
        lastMusicPlayed = nextMusic;
        bgmSource.clip = nextMusic;
        bgmSource.volume = 0f;
        bgmSource.Play();

        yield return FadeVolumeCoroutine(bgmSource, data.maxVolume, 1f);
    }

    /// <summary>
    /// 音量渐变协程
    /// </summary>
    private IEnumerator FadeVolumeCoroutine(AudioSource source, float targetVolume, float duration)
    {
        float time = 0f;
        float startVolume = source.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    /// <summary>
    /// 播放场景空间音效（受距离衰减影响）
    /// </summary>
    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSound = 5f)
    {
        // 玩家位置获取逻辑（原代码注释保留）
         if (player == null)
             player = Player.instance.transform;

         AudioClipData data = audioDB.Get(soundName);
         if (data == null)
        {
            Debug.Log($"尝试播放音效：{soundName}");
            return;
        }

         AudioClip clip = data.GetRandomClip();
         if (clip == null) return;

         float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position, player.position);
        float t = Mathf.Clamp01(1f - (distance / minDistanceToHearSound));

        sfxSource.pitch = Random.Range(0.95f, 1.1f);
        // 指数距离衰减
         sfxSource.volume = Mathf.Lerp(0f, maxVolume, t * t);
         sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 播放全局UI/提示音效（无距离衰减）
    /// </summary>
    public void PlayGlobalSFX(string soundName)
    {
        AudioClipData data = audioDB.Get(soundName);
        if (data == null) return;

        AudioClip clip = data.GetRandomClip();
        if (clip == null) return;

        Debug.Log($"播放全局音效：{soundName}");
        sfxSource.pitch = Random.Range(0.95f, 1.1f);
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}