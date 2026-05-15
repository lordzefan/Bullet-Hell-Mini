using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public int maxHealth = 3;

    protected int currentHealth;

    protected Transform player;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}