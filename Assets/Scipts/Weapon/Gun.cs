using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public string gunName; // 武器名称，供玩家在游戏中识别和选择
    public GameObject gunPickup;

    [Header("Stats")]
    public AmmunitionTypes ammunitionType;
    public int minmumDamage; // 武器的最小伤害值，用于计算随机伤害范围内的实际伤害
    public int maxmumDamage; // 武器的最大伤害值，用于计算随机伤害范围内的实际伤害
    public float maxmumRange; // 武器的最大射程，决定玩家可以射击到的最远距离
    public float recoilAmount = 1.0f; // 武器后座力大小
    public float recoilRecoveryFactor = 1.0f; // 后座力恢复因子，用于调整恢复速度与后坐力大小的比例
    public AudioClip shootAudioClip;
    public float fireRate;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    public float bulletSpeed = 20f;


    protected float timeOfLastShot;

    private Transform cameraTransform;
    private PlayerCameraController playerCameraController;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        playerCameraController = cameraTransform.GetComponent<PlayerCameraController>();
    }

    private Vector3 GetAimDirection()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            return (hit.point - firePoint.position).normalized;
        }
        else
        {
            return cameraTransform.forward;
        }
    }

    protected void Fire()
    {
        if (AmmunitionManager.instance.ConsumeAmmunition(ammunitionType))// 如果有足够的弹药
        {
            // 播放射击音效
            AudioSource.PlayClipAtPoint(shootAudioClip, transform.position);
            Vector3 aimDirection = GetAimDirection();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(aimDirection));

            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.minimumDamage = minmumDamage;
            bulletController.maximumDamage = maxmumDamage;
            bulletController.maximumRange = maxmumRange;
            bulletController.speed = bulletSpeed;
            bulletController.gun = this; // 设置子弹的枪支脚本引用

            Destroy(bullet, 10f); // 销毁子弹，防止子弹过多影响性能，你可以根据需要调整这个值。
        }

        // 计算垂直后坐力和水平后坐力
        float verticalRecoil = Random.Range(recoilAmount * 0.02f, recoilAmount * 0.03f);
        float horizontalRecoil = Random.Range(-recoilAmount * 0.0125f, recoilAmount * 0.0125f);

        // 记录摄像机产生后坐力前的初始位置
        Vector2 initialLookingPos = playerCameraController.GetCurrentLookingPos();

        // 添加后座力偏移值
        Vector2 recoil = new Vector2(horizontalRecoil, verticalRecoil);
        playerCameraController.ApplyRecoilWithRecovery(recoil, recoil.magnitude * recoilRecoveryFactor, initialLookingPos); // 后坐力会在0.3秒内被应用完
    }
}