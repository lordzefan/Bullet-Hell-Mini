using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject bulletPrefab;
    public int poolSize = 100;

    private List<GameObject> bulletPool;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);

            bullet.SetActive(false);

            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;
    }
}
