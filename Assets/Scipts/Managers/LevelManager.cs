using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // 单例模式，方便其他脚本访问
    public GameObject[] enemySpawnPoints; // 敌人生成点数组
    public GameObject enemyPrefab; // 敌人预制体
    public RoundUI roundUI; // 回合UI
    public GameObject player; // 玩家对象
    public GameObject[] weaponPrefabs; // 武器预制体数组
    public GameObject[] ammoPrefabs; // 弹药预制体数组
    public GameObject[] enemyPrefabs;//敌人预制体数组

    public AudioClip helicopterAudioClip;
    public AudioClip weaponDropAudioClip;
    public AudioClip ammoDropAudioClip;// 添加音效

    private int currentRound = 0; // 当前回合数
    private int enemiesLeft; // 剩余敌人数量
    private int totalEnemiesToSpawn; // 本回合需要生成的敌人总数
    private bool isRoundInProgress = false; // 是否处于回合进行中
    private bool hasGameStarted = false; // 游戏是否开始
    private int enemiesDefeatedCount = 0; // 已击败的敌人计数
    private int enemiesToDropSupply = 10; // 击败多少敌人掉落补给
    private int supplyCounter = 0; // 添加一个新变量来跟踪击败的敌人数量
    private int cycleCount = 0;
    private int spawnCounter = 0; // 添加一个生成敌人的计数器

    private List<GameObject> allWeaponsCollected = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) // 单例模式初始化
        {
            instance = this;
        }
        else if (instance != this) // 如果已经存在实例，销毁新实例
        {
            Destroy(gameObject);
        }
    }

    public void PlayerEnteredStart() // 玩家进入开始区域
    {
        if (!hasGameStarted) // 如果游戏尚未开始
        {
            hasGameStarted = true; // 标记游戏开始
            BeginRound(); // 开始回合
        }
    }

    private Vector3 GetRandomSpawnPositionNearPlayer(float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        Vector3 spawnPosition = player.transform.position + randomDirection;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, distance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return spawnPosition;
    }

    private void DropSupply()
    {
        Vector3 spawnPosition = GetRandomSpawnPositionNearPlayer(4);
        spawnPosition += Vector3.up * 5;

        Debug.Log("Playing helicopter sound");
        AudioSource.PlayClipAtPoint(helicopterAudioClip, spawnPosition);

        if (cycleCount == 0)// 第一个轮回
        {
            switch (supplyCounter)
            {
                case 0:
                    DropWeapon(2, spawnPosition);
                    break;
                case 1:
                    DropAmmo(spawnPosition);
                    break;
                case 2:
                    DropWeapon(4, spawnPosition);
                    break;
                case 3:
                    DropWeapon(3, spawnPosition);
                    break;
                case 4:
                    DropAmmo(spawnPosition);
                    break;
                case 5:
                    DropWeapon(5, spawnPosition);
                    break;
                case 6:
                    DropAmmo(spawnPosition);
                    break;
                case 7:
                    DropWeapon(6, spawnPosition);
                    break;
                case 8:
                    DropAmmo(spawnPosition);
                    break;
                case 9:
                    DropAmmo(spawnPosition);
                    break;
                case 10:
                    DropWeapon(7, spawnPosition);
                    DropAmmo(spawnPosition);
                    break;
                case 11:
                    DropAmmo(spawnPosition);
                    break;
                case 12:
                    DropAmmo(spawnPosition);
                    break;
            }
            supplyCounter = (supplyCounter + 1) % 12;

            // 当前轮回结束后，自增cycleCount进入下一个轮回
            if (supplyCounter == 0)
            {
                cycleCount++;
            }
        }
        else // 其他轮回，仅生成弹药箱
        {
            DropAmmo(spawnPosition);
        }

    }

    private void DropWeapon(int index, Vector3 spawnPosition)
    {
        GameObject weaponToDrop = weaponPrefabs[index];
        GameObject droppedWeapon = Instantiate(weaponToDrop, spawnPosition, Quaternion.identity);
        droppedWeapon.GetComponent<Rigidbody>().velocity = Vector3.down;
        StartCoroutine(PlayDelayedAudio(weaponDropAudioClip, spawnPosition, 2f));
    }

    private void DropAmmo(Vector3 spawnPosition)
    {
        GameObject ammoToDrop = GetRandomAmmoDrop();
        GameObject droppedAmmo = Instantiate(ammoToDrop, spawnPosition, Quaternion.identity);
        droppedAmmo.GetComponent<Rigidbody>().velocity = Vector3.down;
        StartCoroutine(PlayDelayedAudio(ammoDropAudioClip, spawnPosition, 2f));
    }

    private GameObject GetRandomAmmoDrop()
    {
        int randomIndex = -1;
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < 0.7f) // 70% 概率
        {
            randomIndex = 0; // 当前武器的弹药类型
        }
        else if (randomValue < 0.85f) // 15% 概率
        {
            randomIndex = 1; // 其他类型1
        }
        else // 15% 概率
        {
            randomIndex = 2; // 其他类型2
        }

        return ammoPrefabs[randomIndex];
    }

    private IEnumerator PlayDelayedAudio(AudioClip audioClip, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Playing drop sound");
        AudioSource.PlayClipAtPoint(audioClip, position);
    }

    private void BeginRound() // 开始新回合
    {
        if (!isRoundInProgress && hasGameStarted) // 如果不在回合进行中且游戏已开始
        {
            currentRound++; // 回合数增加
            StartCoroutine(roundUI.ShowRoundText(currentRound)); // 显示回合数
            totalEnemiesToSpawn = 1 + (currentRound - 1) * 2; // 计算本回合生成敌人数量
            enemiesLeft = totalEnemiesToSpawn; // 初始化剩余敌人数量
            StartCoroutine(SpawnEnemies()); // 生成敌人
            isRoundInProgress = true; // 标记回合进行中
        }
    }

    private IEnumerator SpawnEnemies() // 生成敌人协程
    {
        while (enemiesLeft > 0) // 当还有敌人未生成
        {
            foreach (GameObject spawnPoint in enemySpawnPoints) // 遍历生成点
            {
                // 先更新生成敌人计数器
                spawnCounter++;

                // 根据生成计数器选择要生成的敌人类型
                GameObject enemyPrefabToSpawn;

                if (spawnCounter % 15 == 0 && spawnCounter != 0) // 每15个敌人生成一个Boss
                {
                    enemyPrefabToSpawn = enemyPrefabs[3]; // Boss
                }
                else if (spawnCounter % 10 == 0 && spawnCounter != 0) // 每10个敌人生成一个Elite
                {
                    enemyPrefabToSpawn = enemyPrefabs[2]; // Elite
                }
                else if (spawnCounter % 5 == 0 && spawnCounter != 0) // 每5个敌人生成一个Minion
                {
                    enemyPrefabToSpawn = enemyPrefabs[1]; // Minion
                }
                else
                {
                    enemyPrefabToSpawn = enemyPrefabs[0]; // Normal
                }

                // 生成选择的敌人类型
                Instantiate(enemyPrefabToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);

                enemiesLeft--; // 更新剩余敌人数量

                if (enemiesLeft <= 0) break; // 如果所有敌人都生成，跳出循环
            }
            yield return new WaitForSeconds(1); // 等待1秒
        }
        spawnCounter = 0; // 在回合结束时重置生成敌人计数器
    }

    public void EnemyDefeated() // 敌人被击败
    {
        totalEnemiesToSpawn--; // 更新本回合敌人总数

        enemiesDefeatedCount++;
        if (enemiesDefeatedCount >= enemiesToDropSupply)
        {
            DropSupply();
            enemiesDefeatedCount = 0;
        }

        if (totalEnemiesToSpawn <= 0) // 如果所有敌人都被击败
        {
            isRoundInProgress = false; // 标记回合结束
        }
    }

    private void Update() // 每帧更新
    {
        if (hasGameStarted && !isRoundInProgress) // 如果游戏开始且不在回合进行中
        {
            BeginRound(); // 开始新回合
        }
    }
}
