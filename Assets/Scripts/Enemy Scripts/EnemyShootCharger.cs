using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootCharger : BaseEnemy
{
    [Header("Movement Settings")]
    public float lifetime = 5f;          

    [Header("Shoot Settings")]
    [SerializeField]ProjectileData fireRate;

    Vector2 moveDirection;
    bool isActive = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ChargerRoutine());
    }

    void Update()
    {
        if (!isActive) return;
        LookAtPlayer();
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }


    //MOVE AND SHOOT
    IEnumerator ChargerRoutine()
    {
        moveDirection = (player.position - transform.position).normalized;
        isActive = true;

        StartCoroutine(ShootRoutine());

        yield return new WaitForSeconds(lifetime);

        isActive = false;
        GameManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }

    IEnumerator ShootRoutine()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(fireRate.fireRate);

            if (isActive)
                ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        GameObject bullet = ObjectPool.Instance.GetObject(projectileType);

        if (bullet == null) return;

        bullet.transform.position = transform.position;

        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        bullet.SetActive(true);
    }
}
