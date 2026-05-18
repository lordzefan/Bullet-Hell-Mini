using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : BaseEnemy
{
    protected  void Update()
    {
        LookAtPlayer();
        ChaserPlayer();
    }


    void ChaserPlayer()
    {
        Vector2 direction =
            (player.position - transform.position).normalized;

        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);
        
    }

    
}
