using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : BaseProjectile
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(projectileData.damage);
            gameObject.SetActive(false);
        }
    }
}