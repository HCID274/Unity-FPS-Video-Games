using HCID274._Weapons;
using UnityEngine;

// 枪支拾取类
public class GunPickup : MonoBehaviour, ILootable
{
    [SerializeField] private Gun gun; // 该拾取物关联的枪支对象

    // 当玩家开始查看枪支拾取物时
    public void OnStartLook()
    {
        //Debug.Log($"Started looking at {gun.gunName}!");
    }

    // 当玩家与枪支拾取物互动时
    public void OnInteract()
    {
        //Debug.Log($"Trying to pick up {gun.gunName}!");
        WeaponHandler.instance.PickUpGun(gun);
        Destroy(gameObject); // 销毁掉落的枪支对象
    }

    // 当玩家停止查看枪支拾取物时
    public void OnEndLook()
    {
        //Debug.Log($"Stopped looking at {gun.gunName}!");
    }
}
