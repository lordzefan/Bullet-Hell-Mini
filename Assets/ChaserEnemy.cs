using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : EnemyBase
{
    protected override void Update()
    {
        base.Update();

        Vector2 direction =
            (player.position - transform.position).normalized;

        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
}