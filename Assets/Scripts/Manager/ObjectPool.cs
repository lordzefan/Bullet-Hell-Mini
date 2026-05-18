using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public Transform container;

    [SerializeField]
    private List<PoolData> pools;

    private Dictionary<string, PoolData> poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, PoolData>();

        foreach (PoolData poolData in pools)
        {
            InitializePool(poolData);

            poolDictionary.Add( poolData.poolTag, poolData);
        }
    }


    //INTANTIATE PROJECTILE
    private void InitializePool(PoolData poolData)
    {
        poolData.pool = new List<GameObject>();

        for (int i = 0; i < poolData.amount; i++)
        {

            GameObject obj = Instantiate(poolData.prefab, container);

            obj.SetActive(false);

            poolData.pool.Add(obj);
        }
    }


    //MAKE A POOL TAG LIST
    public GameObject GetObject(string poolTag)
    {
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogWarning(
                $"Pool with tag {poolTag} not found!"
            );

            return null;
        }

        List<GameObject> pool = poolDictionary[poolTag].pool;

        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        return null;
    }
}