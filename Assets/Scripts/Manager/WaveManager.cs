using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject chaserEnemy;
    public GameObject shooterEnemy;
    public GameObject chargerEnemy;
    public GameObject bossEnemy;
    public Transform enemyContainer;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private int currentWave = 0;
    private bool isTransitioning = false;
    private bool waveInProgress = false;
    private bool allSpawned = false; 

    void Start()
    {
        StartCoroutine(StartWaveRoutine());
    }

    void Update()
    {
        // Cek hanya jika:
        // 1. Wave sedang berjalan
        // 2. Tidak sedang transisi
        // 3. Semua enemy sudah di-spawn (bukan cuma belum mulai)
        // 4. Semua enemy sudah mati
        if (waveInProgress && !isTransitioning && allSpawned && GameManager.Instance.AllEnemiesDead())
        {
            Debug.Log($"Wave {currentWave} cleared!");
            isTransitioning = true;
            waveInProgress = false;
            allSpawned = false;
            StartCoroutine(NextWaveRoutine());
        }
    }

    IEnumerator NextWaveRoutine()
    {
        Debug.Log("Next wave in 3 seconds...");
        yield return new WaitForSeconds(3f);

        currentWave++;

        if (currentWave > 4)
        {
            GameWin();
            yield break;
        }

        isTransitioning = false;
        StartWave(currentWave);
    }

    void GameWin()
    {
        Debug.Log("YOU WIN ALL WAVES!");
    }

    IEnumerator StartWaveRoutine()
    {
        yield return new WaitForSeconds(2f);
        StartWave(1);
    }

    void StartWave(int waveIndex)
    {
        currentWave = waveIndex;
        waveInProgress = true;
        allSpawned = false;
        Debug.Log($"=== Starting Wave {waveIndex} ===");

        switch (waveIndex)
        {
            case 1: StartCoroutine(SpawnWave1()); break;
            case 2: StartCoroutine(SpawnWave2()); break;
            case 3: StartCoroutine(SpawnWave3()); break;
            case 4: StartCoroutine(SpawnBoss()); break;
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, enemyContainer);

        GameManager.Instance.RegisterEnemy();
    }

    IEnumerator SpawnWave1()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy(chaserEnemy);
            yield return new WaitForSeconds(1f);
        }
        allSpawned = true;
        Debug.Log("Wave 1 - All enemies spawned");
    }

    IEnumerator SpawnWave2()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy(chaserEnemy);
            yield return new WaitForSeconds(0.7f);
        }
        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(shooterEnemy);
            yield return new WaitForSeconds(1.5f);
        }
        allSpawned = true;
        Debug.Log("Wave 2 - All enemies spawned");
    }

    IEnumerator SpawnWave3()
    {
        for (int i = 0; i < 4; i++)
        {
            SpawnEnemy(chargerEnemy);
            yield return new WaitForSeconds(2f);
        }
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy(shooterEnemy);
            yield return new WaitForSeconds(1f);
        }
        allSpawned = true;
        Debug.Log("Wave 3 - All enemies spawned");
    }

    IEnumerator SpawnBoss()
    {
        Instantiate(bossEnemy, spawnPoints[1].position, Quaternion.identity);
        GameManager.Instance.RegisterEnemy();
        yield return new WaitForSeconds(1f);

        allSpawned = true;
        Debug.Log("Wave Boss - All enemies spawned");
    }
}