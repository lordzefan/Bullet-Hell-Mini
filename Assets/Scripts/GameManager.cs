using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int aliveEnemies = 0;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy()
    {
        aliveEnemies++;
        Debug.Log($"Enemy registered. Total alive: {aliveEnemies}");
    }

    public void EnemyKilled()
    {
        aliveEnemies--;
        Debug.Log($"Enemy killed. Remaining: {aliveEnemies}");

    }

    public bool AllEnemiesDead()
    {
        return aliveEnemies <= 0;
    }
}