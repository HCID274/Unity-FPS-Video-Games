using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scene1"); 
    }

    public void ExitGame()
    {
        // 在编辑器中使用此方法将停止播放。
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 在构建的游戏中使用此方法将关闭游戏。
        Application.Quit();
        #endif
    }
}
