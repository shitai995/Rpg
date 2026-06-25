// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:30:44
// 版本：V1.1
// 描述：场景淡入淡出遮罩，实现画面黑屏过渡效果
// ========================================================

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面淡入淡出控制器
/// </summary>
public class UI_FadeScreen : MonoBehaviour
{
    public Coroutine fadeEffectCo { get; private set; }
    private Image fadeImage;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        //fadeImage.color = new Color(0, 0, 0, 1);
    }

    /// <summary>
    /// 淡入：从黑屏变为透明
    /// </summary>
    public void DoFadeIn(float duration = 1f)
    {
        fadeImage.color = Color.black;
        FadeEffect(0f, duration);
    }

    /// <summary>
    /// 淡出：从透明变为黑屏
    /// </summary>
    public void DoFadeOut(float duration = 1f)
    {
        fadeImage.color = new Color(0, 0, 0, 0f);
        FadeEffect(1f, duration);
    }

    /// <summary>
    /// 启动过渡效果
    /// </summary>
    private void FadeEffect(float targetAlpha, float duration)
    {
        if (fadeEffectCo != null)
            StopCoroutine(fadeEffectCo);

        fadeEffectCo = StartCoroutine(FadeEffectCo(targetAlpha, duration));
    }

    /// <summary>
    /// 淡入淡出协程
    /// </summary>
    private IEnumerator FadeEffectCo(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = color;
            yield return null;
        }

        // 强制修正为目标透明度
        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;
    }
}