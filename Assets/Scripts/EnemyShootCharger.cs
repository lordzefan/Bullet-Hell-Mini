using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootCharger : BaseEnemy
{
    [Header("Movement Settings")]
    public float lifetime = 5f;          // berapa lama enemy bergerak sebelum destroy

    [Header("Shoot Settings")]
    public float fireRate = 1.5f;        // jeda antar tembakan

    private Vector2 moveDirection;
    private bool isActive = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ChargerRoutine());
    }

    void Update()
    {
        if (!isActive) return;

        // Bergerak lurus terus ke arah awal (tidak mengikuti player)
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    IEnumerator ChargerRoutine()
    {
        // Ambil arah ke player saat spawn — tidak diupdate lagi
        moveDirection = (player.position - transform.position).normalized;
        isActive = true;

        // Tembak dan bergerak bersamaan
        StartCoroutine(ShootRoutine());

        // Hancurkan enemy setelah lifetime habis
        yield return new WaitForSeconds(lifetime);

        isActive = false;
        GameManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }

    IEnumerator ShootRoutine()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(fireRate);

            if (isActive)
                ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        GameObject bullet = ObjectPool.Instance.GetEnemyBullet();

        if (bullet == null) return;

        bullet.transform.position = transform.position;

        // Arah tembak selalu update ke posisi player saat ini
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        bullet.SetActive(true);
    }
}
