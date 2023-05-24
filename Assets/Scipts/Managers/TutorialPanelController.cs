using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialPanelController : MonoBehaviour
{
    public GameObject tutorialPanel; // 你的TutorialPanel对象

    void Start()
    {
        tutorialPanel.SetActive(false); // 在开始时隐藏TutorialPanel
    }

    public void ShowTutorial() // 显示TutorialPanel
    {
        tutorialPanel.SetActive(true);

        Cursor.visible = true; // 显示鼠标
        Cursor.lockState = CursorLockMode.None; // 解锁鼠标

        Time.timeScale = 0; // 暂停游戏
    }

    public void CloseTutorial() // 关闭TutorialPanel
    {
        tutorialPanel.SetActive(false);

        Cursor.visible = false; // 隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标

        Time.timeScale = 1; // 恢复游戏
    }
}
