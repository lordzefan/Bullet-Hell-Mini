using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Player Projectile Pool")]
    [SerializeField]
    private GameObject playerProjectilePrefab;

    [SerializeField]
    private int playerPoolSize = 100;

    [SerializeField]
    private Transform playerProjectileContainer;

    private List<GameObject> playerProjectilePool;

    [Header("Enemy Bullet Pool")]
    [SerializeField]
    private GameObject enemyBulletPrefab;

    [SerializeField]
    private int enemyPoolSize = 200;

    [SerializeField]
    private Transform enemyBulletContainer;

    private List<GameObject> enemyBulletPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializePlayerPool();
        InitializeEnemyPool();
    }

    private void InitializePlayerPool()
    {
        playerProjectilePool = new List<GameObject>();

        for (int i = 0; i < playerPoolSize; i++)
        {
            GameObject bullet =
                Instantiate(
                    playerProjectilePrefab,
                    playerProjectileContainer
                );

            bullet.SetActive(false);

            playerProjectilePool.Add(bullet);
        }
    }

    private void InitializeEnemyPool()
    {
        enemyBulletPool = new List<GameObject>();

        for (int i = 0; i < enemyPoolSize; i++)
        {
            GameObject bullet =
                Instantiate(
                    enemyBulletPrefab,
                    enemyBulletContainer
                );

            bullet.SetActive(false);

            enemyBulletPool.Add(bullet);
        }
    }

    public GameObject GetPlayerBullet()
    {
        foreach (GameObject bullet in playerProjectilePool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;
    }

    public GameObject GetEnemyBullet()
    {
        foreach (GameObject bullet in enemyBulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;
    }
}