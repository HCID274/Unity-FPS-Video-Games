using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // 暂停面板游戏对象
    private bool isPaused; // 游戏是否暂停的标志

    private void Start()
    {
        pausePanel.SetActive(false); // 开始时关闭暂停面板
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // 当按下Esc键时
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // 将游戏时间缩放设置为0，暂停游戏
        pausePanel.SetActive(true); // 激活暂停面板
        isPaused = true;

        // 解锁鼠标并显示鼠标光标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // 将游戏时间缩放设置为1，恢复游戏
        pausePanel.SetActive(false); // 关闭暂停面板
        isPaused = false;

        // 锁定鼠标并隐藏鼠标光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        // 将当前场景中的所有活动游戏对象设置为非活动状态
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            obj.SetActive(false);
        }
        // 加载主菜单场景
        SceneManager.LoadScene("MainMenu");
    }
}
