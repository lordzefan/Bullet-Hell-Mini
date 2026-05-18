using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public int score;
    public int highScore;
    private float gameTimer;

    
    public int aliveEnemies = 0;

    void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        gameTimer += Time.deltaTime;
    }

    //CHECK ENEMY ALIVE AND DEATH
    public void RegisterEnemy()
    {
        aliveEnemies++;
        Debug.Log($"Enemy registered. Total alive: {aliveEnemies}");
    }

    public void EnemyKilled()
    {
        score += 10;
        aliveEnemies--;
        Debug.Log($"Enemy killed. Remaining: {aliveEnemies}");

    }

    public bool AllEnemiesDead()
    {
        return aliveEnemies <= 0;
    }

    // SCORE
    public void ScoreSetting()
    {
        if(score > gameTimer) score -= Mathf.RoundToInt(gameTimer);
        if(score > highScore) {
            highScore = score;
            SaveHighScore();
        }
    }




    // SAVE AND LOAD SYSTEM
    [System.Serializable]
    public class SaveData
    {
        public int highScore;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);
    
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
        }
    }
}

