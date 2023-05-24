using UnityEngine;

public class DestroyInOneHit : MonoBehaviour, IDamageable
{
    public void DealDamage(int damage,Vector3 v3)
    {
        Destroy(gameObject); // 销毁对象（一击必破）
    }
}