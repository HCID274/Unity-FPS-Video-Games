using UnityEngine;
using TMPro;

namespace HCID274._UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float destroyTime; // 伤害文本销毁时间
        [SerializeField] private Vector3 offset; // 伤害文本位置偏移量
        [SerializeField] private Vector3 randomiseOffset; // 随机位置偏移范围
        [SerializeField] private Color damageColour; // 伤害文本颜色

        private TextMeshPro damageText; // 伤害文本组件

        private void Awake()
        {
            damageText = GetComponent<TextMeshPro>(); // 获取伤害文本组件
            transform.localPosition += offset; // 应用位置偏移量
            transform.localPosition += new Vector3(
                Random.Range(-randomiseOffset.x, randomiseOffset.x), // 随机X轴偏移
                Random.Range(-randomiseOffset.y, randomiseOffset.y), // 随机Y轴偏移
                Random.Range(-randomiseOffset.z, randomiseOffset.z) // 随机Z轴偏移
            );
            Destroy(gameObject, destroyTime); // 在指定时间后销毁游戏对象
        }

        public void Initialise(int damageValue)
        {
            damageText.text = damageValue.ToString(); // 设置伤害文本的值
        }
    }
}
