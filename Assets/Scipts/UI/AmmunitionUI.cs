using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HCID274._UI
{
    public class AmmunitionUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammunitionTypeText; // 显示当前枪支类型的文本
        [SerializeField] TextMeshProUGUI ammunitionCountText; // 显示当前弹药数量的文本

        private CanvasGroup canvasGroup; // UI元素的CanvasGroup组件

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>(); // 获取CanvasGroup组件
        }

        // 更新显示的弹药类型和数量
        public void UpdateAmmunitionType(Gun gun)
        {
            if (gun == null)
            {
                canvasGroup.alpha = 0; // 如果没有枪支，将UI透明度设为0（隐藏）
                return;
            }

            canvasGroup.alpha = 1; // 如果有枪支，将UI透明度设为1（显示）

            // 更新显示的弹药数量
            UpdateAmmunitionCount(AmmunitionManager.instance.GetAmmunitionCount(gun.ammunitionType));

            // 更新显示的弹药类型文本
            ammunitionTypeText.text = gun.ammunitionType.ToString();
        }

        // 更新显示的弹药数量文本
        public void UpdateAmmunitionCount(int newCount)
        {
            ammunitionCountText.text = newCount.ToString();
        }
    }
}

