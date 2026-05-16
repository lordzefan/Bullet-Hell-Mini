using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 100;
    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

    }

    public virtual void TakeDamage(int damage)
    {

        

        if (currentHealth > 0)
        {
           currentHealth -= damage; 
        }else
        {
            StartCoroutine(DeathRoutine());
            print("Player Death");
        }
        
    }

     IEnumerator DeathRoutine()
    {
        Debug.Log("GAME OVER");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    
}
