using HCID274._UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] public string enemyName; // 敌人名称
    [SerializeField] private float maxHealth;//最大生命值
    [SerializeField] private Slider healthbarSlider; // 生命条滑块
    [SerializeField] private Image healthbarFillImage; // 生命条填充图像
    [SerializeField] private Color maxHealthColor; // 最大生命值颜色
    [SerializeField] private Color zeroHealthColor; // 零生命值颜色
    [SerializeField] private GameObject damageTextPrefab; // 伤害文本预制件
    [SerializeField] private int enemyScore = 1000; // 敌人分数值
    [SerializeField] private float knockbackMultiplier ; // 伤害与击退力量之间的比例
    [SerializeField] private float minKnockbackForce ; // 最小击退力量
    [SerializeField] private float maxKnockbackForce ; // 最大击退力量

    public KillEffectController killEffectController;

    private float currentHealth; // 当前生命值
    public EnemyAI enemyAI; // 敌人的EnemyAI组件
    private Rigidbody enemyRigidbody; // 敌人的Rigidbody组件

    private void Start()
    {
        currentHealth = maxHealth; // 初始化当前生命值
        SetHealthbarUi(); // 设置生命条UI
        killEffectController = FindObjectOfType<KillEffectController>();
        enemyRigidbody = GetComponent<Rigidbody>(); // 获取Rigidbody组件
        enemyAI = GetComponent<EnemyAI>(); // 获取EnemyAI组件
    }

    public void DealDamage(int damage, Vector3 originPosition)
    {
        // 扣除伤害值
        currentHealth -= damage;

        // 实例化伤害文本并初始化
        Instantiate(damageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageText>().Initialise(damage);

        // 计算击退方向
        Vector3 knockbackDirection = (transform.position - originPosition).normalized;

        // 计算击退力量
        float knockbackMagnitude = Mathf.Clamp(damage * knockbackMultiplier, minKnockbackForce, maxKnockbackForce);

        // 计算向上的力量
        Vector3 upwardForce = Vector3.up * knockbackMagnitude*0.2f; // 取击退力量的20%
        // 计算向后的力量
        Vector3 backwardForce = knockbackDirection * knockbackMagnitude * 0.8f; // 取击退力量的80%

        // 计算最终的击退力
        Vector3 knockbackForce = upwardForce + backwardForce;

        // 使用计算出的力度进行击退
        StartCoroutine(ApplyKnockback(knockbackForce));

        CheckIfDead();
        // 更新生命条UI
        SetHealthbarUi();
    }

    // 创建协程来执行击退效果
    private IEnumerator ApplyKnockback(Vector3 knockbackForce)
    {
        float knockbackStartTime = Time.time;
        float knockbackDuration = 1f;

        enemyAI.isKnockedBack = true; // 当击退开始时，设置 isKnockedBack 为 true
        enemyRigidbody.isKinematic = false; // 将Rigidbody设置为非Kinematic

        // 其次，应用击退力
        enemyRigidbody.AddForce(knockbackForce, ForceMode.Impulse);

        while (Time.time < knockbackStartTime + knockbackDuration)
        {
            yield return null;
        }

        enemyRigidbody.velocity = Vector3.zero; // 清除任何剩余的速度
        enemyRigidbody.isKinematic = true; // 将Rigidbody设置为Kinematic
        enemyAI.isKnockedBack = false; // 当击退结束时，设置 isKnockedBack 为 false
    }


    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            LevelManager.instance.EnemyDefeated(); // 通知GameManager敌人已被击败
            ScoreUI scoreUI = FindObjectOfType<ScoreUI>(); // 获取ScoreUI组件的引用
            if (scoreUI != null)
            {
                scoreUI.AddScore(enemyScore); // 在敌人死亡时增加分数
            }
            Destroy(gameObject); // 销毁对象
            killEffectController.TriggerKillEffect();
            Debug.Log("Destroy!");
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage(); // 计算生命值百分比
        healthbarSlider.value = healthPercentage; // 设置滑块值
        healthbarFillImage.color = Color.Lerp(zeroHealthColor, maxHealthColor, healthPercentage / 100f); // 设置填充颜色
    }

    private float CalculateHealthPercentage()
    {
        return Mathf.Clamp((currentHealth / maxHealth) * 100, 0f, 100f); // 计算生命值百分比公式
    }
}