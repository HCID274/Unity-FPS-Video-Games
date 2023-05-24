using HCID274._UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;//最大生命值
    [SerializeField] private Slider healthbarSlider; // 生命条滑块
    [SerializeField] private Image healthbarFillImage; // 生命条填充图像
    [SerializeField] private Color maxHealthColor; // 最大生命值颜色
    [SerializeField] private Color zeroHealthColor; // 零生命值颜色
    [SerializeField] private GameObject damageTextPrefab; // 伤害文本预制件
    [SerializeField] private AudioClip healingAudioClip; // 治疗音效

    private int currentHealth; // 当前生命值
    private AudioSource audioSource; // 音频源组件

    private int healingAmount = 1; // 每秒回复的生命值
    private float healingTimer = 10f; // 回血计时器
    private float lastDamageTime; // 上次受到伤害的时间
    private float healingAccumulator = 0f;


    private void Start()
    {
        currentHealth = 100; // 初始化当前生命值
        SetHealthbarUi(); // 设置生命条UI

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= healingTimer && currentHealth < (int)maxHealth)
        {
            Heal(healingAmount * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = healingAudioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (audioSource.clip == healingAudioClip && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }


    public void DealDamage(int damage)
    {
        currentHealth -= damage; // 扣除伤害值
        lastDamageTime = Time.time; // 更新上次受到伤害的时间

        CheckIfDead(); // 检查是否死亡
        SetHealthbarUi(); // 更新生命条UI
    }

    private void Heal(float amount)
    {
        healingAccumulator += amount;
        int healingToApply = Mathf.FloorToInt(healingAccumulator);
        if (healingToApply >= 1)
        {
            currentHealth = Mathf.Min((int)maxHealth, currentHealth + healingToApply);
            healingAccumulator -= healingToApply;
            SetHealthbarUi();
        }
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // 销毁对象
            Debug.Log("Destroy!");
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage(); // 计算生命值百分比
        healthbarSlider.value = healthPercentage; // 设置滑块值
        healthbarFillImage.color = Color.Lerp(zeroHealthColor, maxHealthColor, healthPercentage / maxHealth); // 设置填充颜色
    }

    private float CalculateHealthPercentage()
    {
        return Mathf.Clamp(((float)currentHealth / maxHealth) * 100, 0f, maxHealth); // 计算生命值百分比公式
    }
}
