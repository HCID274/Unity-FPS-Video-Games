using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // 用于显示当前分数的文本框
    [SerializeField] private GameObject floatingScorePrefab; // 浮动分数预制体，实现分数在屏幕上的浮动效果
    [SerializeField] private Transform floatingScoreSpawnPoint; // 浮动分数出现的位置

    private int currentScore = 0; // 当前玩家的分数，初始化为0

    private void Start()
    {
        UpdateScoreText(); // 游戏开始时，更新分数显示
    }

    public void AddScore(int points)
    {
        currentScore += points; // 玩家得分，增加当前分数
        UpdateScoreText(); // 更新分数显示
        ShowFloatingScore(points); // 显示得分动画
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{currentScore}"; // 将当前分数显示到UI上
    }

    private void ShowFloatingScore(int points)
    {
        GameObject floatingScoreInstance = Instantiate(floatingScorePrefab, floatingScoreSpawnPoint.position, Quaternion.identity, floatingScoreSpawnPoint); // 创建浮动分数实例
        TextMeshProUGUI floatingScoreText = floatingScoreInstance.GetComponent<TextMeshProUGUI>(); // 获取浮动分数的Text组件
        floatingScoreText.text = $"+{points}"; // 设置浮动分数的文本为得分值
        Destroy(floatingScoreInstance, 2f); // 2秒后销毁浮动分数实例
    }
}
