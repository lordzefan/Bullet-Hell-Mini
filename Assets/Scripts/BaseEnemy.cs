using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseHealth
{
    public int collionDamage = 1;
    public float moveSpeed = 3f;
    protected Transform player;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player =
        other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(collionDamage);

            Die();
        }
    }

    protected virtual void Die()
    {
        GameManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }
}
