using UnityEngine;

// 弹药拾取类
public class AmmunitionPickup : MonoBehaviour, ILootable
{
    [SerializeField] private int ammunitionCount; // 弹药数量
    [SerializeField] private AmmunitionTypes ammunitionType; // 弹药类型

    // 当玩家开始查看弹药时
    public void OnStartLook()
    {
        // 显示工具提示UI
        //Debug.Log($"Started looking at {ammunitionType}!");
    }

    // 当玩家与弹药互动时
    public void OnInteract()
    {
        // 增加弹药
        AmmunitionManager.instance.AddAmmunition(ammunitionCount, ammunitionType);
        // 销毁弹药拾取对象
        Destroy(gameObject);
    }

    // 当玩家停止查看弹药时
    public void OnEndLook()
    {
        // 隐藏工具提示UI
        //Debug.Log($"Stopped looking at {ammunitionType}!");
    }
}
