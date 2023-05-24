using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum State // 敌人的状态：追击、攻击、攻击延迟、改变避障优先级、撤退
    {
        Chasing,
        Attacking,
        AttackCooldown
    }

    private NavMeshAgent agent; // 敌人的导航代理
    private Transform playerTransform; // 玩家的Transform组件
    private EnemyHealth enemyHealth; // 敌人的生命值组件
    private PlayerHealth playerHealth; // 玩家的生命值组件

    public float attackDistance = 2.0f; // 攻击距离
    public float attackDamage = 12.0f; // 攻击伤害
    public float attackDelay = 2.0f; // 攻击延迟
    public bool isKnockedBack; // 添加一个新的布尔值来检查敌人是否正在被击退

    public State state; // 敌人当前的状态

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // 获取NavMeshAgent组件

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 获取玩家的Transform组件
        enemyHealth = GetComponent<EnemyHealth>(); // 获取EnemyHealth组件
        playerHealth = playerTransform.GetComponent<PlayerHealth>(); // 获取玩家的PlayerHealth组件

        state = State.Chasing; // 初始设置敌人状态为追击
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); // 计算玩家与敌人的距离

        switch (state)
        {
            case State.Chasing: // 如果敌人处于追击状态
                if (distanceToPlayer <= attackDistance) // 如果敌人与玩家的距离小于等于攻击距离
                {
                    state = State.Attacking; // 将敌人状态设置为攻击
                }
                else
                {
                    if (!isKnockedBack)// 只有当敌人没有被击退并没有跳跃时
                    {
                        agent.SetDestination(playerTransform.position); // 设置目标为玩家位置
                    }
                }
                break;

            case State.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    state = State.Chasing;
                }
                else
                {
                    AttackPlayer();
                }
                break;

            case State.AttackCooldown: // In AttackCooldown state, do nothing
                break;
        }
    }

    private void AttackPlayer() // 敌人攻击玩家的方法
    {
        playerHealth.DealDamage(Mathf.RoundToInt(attackDamage)); // 造成伤害
        state = State.AttackCooldown; // 敌人攻击后进入攻击冷却状态
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay() // 攻击延迟协程
    {
        yield return new WaitForSeconds(attackDelay); // 等待攻击延迟
        state = State.Chasing;
    }
}
