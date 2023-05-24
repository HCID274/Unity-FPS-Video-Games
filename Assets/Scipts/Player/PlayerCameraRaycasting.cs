using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRaycasting : MonoBehaviour
{
    [SerializeField] private float raycastDistance; // 射线距离

    private ILootable currentTarget; // 当前目标

    private void Update()
    {
        HandleRaycast(); // 处理射线检测

        if (Input.GetKeyDown(KeyCode.E)) // 如果按下E键
        {
            if (currentTarget != null) // 如果当前有目标
            {
                currentTarget.OnInteract(); // 与当前目标进行交互
            }
        }
    }

    private void HandleRaycast()
    {
        RaycastHit whatIHit; // 射线碰撞的结果

        if (Physics.Raycast(transform.position,
            transform.forward,
            out whatIHit, raycastDistance)) // 从当前位置向前发射射线
        {
            ILootable lootable = whatIHit.collider.GetComponent<ILootable>(); // 获取碰撞对象上的ILootable接口

            if (lootable != null) // 如果存在ILootable接口
            {
                if (lootable == currentTarget) // 如果碰撞到的是当前目标
                {
                    return;
                }
                else if (currentTarget != null) // 如果当前目标不为空
                {
                    currentTarget.OnEndLook(); // 结束观察当前目标
                    currentTarget = lootable; // 将新的目标设置为当前目标
                    currentTarget.OnStartLook(); // 开始观察新的目标
                }
                else
                {
                    currentTarget = lootable; // 将新的目标设置为当前目标
                    currentTarget.OnStartLook(); // 开始观察新的目标
                }
            }
            else
            {
                if (currentTarget != null) // 如果当前目标不为空
                {
                    currentTarget.OnEndLook(); // 结束观察当前目标
                    currentTarget = null; // 将当前目标设为空
                }
            }
        }
        else // 如果射线未检测到物体
        {
            if (currentTarget != null) // 如果当前目标不为空
            {
                currentTarget.OnEndLook(); // 结束观察当前目标
                currentTarget = null; // 将当前目标设为空
            }
        }
    }
}
