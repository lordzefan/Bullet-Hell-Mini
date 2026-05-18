using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 10;
    protected int currentHealth;

    protected bool isInvulnerable;
    SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;

    }

    public virtual void TakeDamage(int damage)
    {

        if (currentHealth > 0)
        {
           currentHealth -= damage; 
           StartCoroutine(DamageFlash());
        }
        
    }

    //HIT EFFECT
    IEnumerator DamageFlash()
    {
        isInvulnerable = true;

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.4f);

        isInvulnerable = false;
    }

    
}
