using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    public float shootRange = 7f;
    public float fireRate = 1f;

    private float nextFireTime;

    protected override void Update()
    {
        base.Update();

        float distance =
            Vector2.Distance(transform.position, player.position);

        if (distance > shootRange)
        {
            Vector2 direction =
                (player.position - transform.position).normalized;

            transform.position +=
                (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();

                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        Debug.Log("Enemy Shoot");
    }
}
