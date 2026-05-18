using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pool Data")]
public class PoolData: ScriptableObject
{
    public string poolTag = "PlayerProjectile";

    public GameObject prefab;

    public int amount = 50;


    [HideInInspector]
    public List<GameObject> pool;
}
