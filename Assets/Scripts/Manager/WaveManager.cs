using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject chaserEnemy;
    public GameObject shooterEnemy;
    public GameObject chargerEnemy;
    public GameObject bossEnemy;

    [Header("Container")]
    public Transform enemyContainer;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI")]
    public TextMeshProUGUI waveTxt;
    public TextMeshProUGUI highScoreText;

    private int currentWave = 0;

    private bool isTransitioning;
    private bool waveInProgress;
    private bool allSpawned;

    private void Start()
    {
        waveTxt.gameObject.SetActive(false);
        GameManager.Instance.LoadHighScore();
        highScoreText.text = "High Score : " + GameManager.Instance.highScore;

        StartCoroutine(StartWaveRoutine());
    }

    private void Update()
    {
         

        if (waveInProgress &&!isTransitioning &&allSpawned &&GameManager.Instance.AllEnemiesDead())
        {
            Debug.Log($"Wave {currentWave} cleared!");

            isTransitioning = true;
            waveInProgress = false;
            allSpawned = false;
            
            StartCoroutine(ShowWaveText());
            StartCoroutine(NextWaveRoutine());
        }
    }


    // START GAME
    IEnumerator StartWaveRoutine()
    {
        yield return new WaitForSeconds(2f);

        StartWave(1);
    }


    // NEXT WAVE
    IEnumerator NextWaveRoutine()
    {
        yield return new WaitForSeconds(3f);

        currentWave++;

        if (currentWave > 4)
        {
            StartCoroutine(GameWin());
            yield break;
        }

        isTransitioning = false;

        StartWave(currentWave);
    }

 
    // START WAVE
    void StartWave(int waveIndex)
    {
        currentWave = waveIndex;

        waveInProgress = true;
        allSpawned = false;


        switch (waveIndex)
        {
            case 1:
                StartCoroutine(SpawnWave1());
                break;

            case 2:
                StartCoroutine(SpawnWave2());
                break;

            case 3:
                StartCoroutine(SpawnWave3());
                break;

            case 4:
                StartCoroutine(SpawnBoss());
                break;
        }
    }

 
    // WAVE UI
    IEnumerator ShowWaveText()
    {
        waveTxt.gameObject.SetActive(true);

        if (currentWave == 3)
        {
            waveTxt.text = "BOSS WAVE";
        }
        else if ( currentWave < 3)
        {
            waveTxt.text = "WAVE " + (currentWave + 1);
        }
        else
        {
            waveTxt.text = "YOU WIN";
        }
        yield return new WaitForSeconds(2f);

        waveTxt.gameObject.SetActive(false);
    }


    // SPAWN
    void SpawnEnemy(GameObject enemyPrefab)
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(
            enemyPrefab, spawnPoint.position, Quaternion.identity, enemyContainer);

        GameManager.Instance.RegisterEnemy();
    }

    // WAVE 
    IEnumerator SpawnWave1()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy(chaserEnemy);

            yield return new WaitForSeconds(1f);
        }

        allSpawned = true;
    }


    // WAVE 2
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
    }

    // WAVE 3
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
    }

    // SPAWN BOSS
    IEnumerator SpawnBoss()
    {
        Instantiate(
            bossEnemy,
            spawnPoints[1].position,
            Quaternion.identity,
            enemyContainer
        );

        GameManager.Instance.RegisterEnemy();

        yield return new WaitForSeconds(1f);

        allSpawned = true;
    }


    // WIN
    IEnumerator GameWin()
    {
        GameManager.Instance.ScoreSetting();
         
        waveTxt.gameObject.SetActive(true);
        waveTxt.text = "SCORE : "+ GameManager.Instance.score;;

         yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("YOU WIN ALL WAVES!");
    }
}