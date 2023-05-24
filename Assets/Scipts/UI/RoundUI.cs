using System.Collections;
using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundText; // 显示当前回合的文本
    [SerializeField] AudioClip roundAudioClip; // 回合音效
    [SerializeField] float roundTextDisplayTime = 1.0f; // 回合文本显示时长
    [SerializeField] float roundTextFadeDuration = 2.0f; // 回合文本淡出时长

    private CanvasGroup canvasGroup; // UI元素的CanvasGroup组件

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>(); // 获取CanvasGroup组件
    }

    public IEnumerator ShowRoundText(int currentRound)
    {
        roundText.text = "Round " + currentRound; // 更新回合显示文本
        AudioSource.PlayClipAtPoint(roundAudioClip, Camera.main.transform.position); // 播放回合音效

        canvasGroup.alpha = 1; // 将UI透明度设为1（显示）
        yield return new WaitForSeconds(roundTextDisplayTime); // 等待指定时间

        float elapsedTime = 0;
        while (elapsedTime < roundTextFadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / roundTextFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0; // 将UI透明度设为0（隐藏）
    }
}
