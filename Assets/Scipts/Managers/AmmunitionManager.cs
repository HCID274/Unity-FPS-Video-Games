using HCID274._UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionManager : MonoBehaviour
{
    public static AmmunitionManager instance; // 创建一个AmmunitionManager类的单例实例

    public AmmunitionUI ammunitionUI;// 弹药UI组件

    private Dictionary<AmmunitionTypes, int> ammunitionCounts = new Dictionary<AmmunitionTypes, int>(); // 创建一个字典，用于存储每种弹药的数量

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // 如果单例实例为空，将当前实例设为单例
        }
        else if (instance != this)
        {
            Destroy(this); // 如果已存在其他实例，销毁当前实例
        }
    }

    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(AmmunitionTypes)).Length; i++)
        {
            ammunitionCounts.Add((AmmunitionTypes)i, 0); // 将每种弹药的初始数量设置为0
        }
    }

    public void AddAmmunition(int value, AmmunitionTypes ammunitionType)
    {
        ammunitionCounts[ammunitionType] += value; // 根据弹药类型，增加指定数量的弹药
        ammunitionUI.UpdateAmmunitionCount(ammunitionCounts[ammunitionType]);
    }

    public int GetAmmunitionCount(AmmunitionTypes ammunitionType)
    {
        return ammunitionCounts[ammunitionType];
    }

    public bool ConsumeAmmunition(AmmunitionTypes ammunitionType)
    {
        if (ammunitionCounts[ammunitionType] > 0)
        {
            ammunitionCounts[ammunitionType]--; // 若有弹药，消耗一发弹药
            ammunitionUI.UpdateAmmunitionCount(ammunitionCounts[ammunitionType]);
            return true; // 返回消耗成功
        }
        else
        {
            Debug.Log("No Ammo!"); // 若无弹药，输出提示信息
            return false; // 返回消耗失败
        }
    }
}
