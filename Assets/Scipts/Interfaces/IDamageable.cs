using UnityEngine;

public interface IDamageable
{
    void DealDamage(int damage, Vector3 originPosition); // 处理受到的伤害
}
