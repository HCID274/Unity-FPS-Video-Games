using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HCID274._Helpers
{
    public class DDOL : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this); // 当加载新场景时，不销毁此游戏对象
            SceneManager.LoadScene(1); // 加载场景索引为1的场景
        }
    }
}
