using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int damage; // 将 damage 设置为公共变量
    public Gun gun; // 引用到枪支脚本
    public Vector3 originPosition; // 子弹的发射点

    public int minimumDamage; // 从 Gun 脚本传递过来的最小伤害值
    public int maximumDamage; // 从 Gun 脚本传递过来的最大伤害值
    public float maximumRange; // 从 Gun 脚本传递过来的最大射程

    private Rigidbody rb;
    private float flightTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        originPosition = gun.firePoint.position; // 记录发射点
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        flightTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float distanceTraveled = flightTime * speed;
            float normalizedDistance = Mathf.Clamp01(distanceTraveled / maximumRange);
            damage = Mathf.RoundToInt(Mathf.Lerp(maximumDamage, minimumDamage, normalizedDistance));
            // 将碰撞信息传递给DealDamage方法
            damageable.DealDamage(damage, originPosition);
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}
