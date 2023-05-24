using UnityEngine;

// 半自动枪支类
public class SemiAutomatic : Gun
{
    // 在Update中检查鼠标左键是否被按下
    private void Update()
    {
        // 如果按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            // 如果时间超过上次射击的时间加上射速
            if (Time.time - timeOfLastShot >= 1 / fireRate)
            {
                // 发射子弹
                Fire();
                // 更新上次射击的时间
                timeOfLastShot = Time.time;
            }
        }
    }
}