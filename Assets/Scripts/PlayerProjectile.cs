using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : BaseProjectile
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseEnemy enemy =
            other.GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(projectileData.damage);

            DisableProjectile();
        }
    }
}
