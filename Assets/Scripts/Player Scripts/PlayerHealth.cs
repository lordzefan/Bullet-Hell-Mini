using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    public TextMeshProUGUI gameOver;
    public Slider healthBar;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        
    }

    public override void TakeDamage(int damage)
    {

        healthBar.value = currentHealth;
        base.TakeDamage(damage);
        
        if(currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
            print("Player Death");
        }

    }

     IEnumerator DeathRoutine()
    {
        gameOver.gameObject.SetActive(true);
        gameOver.text = "GAME OVER";
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



   
}
