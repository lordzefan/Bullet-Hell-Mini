using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : BaseEnemy
{
    [Header("Shoot Settings")]
    public float fireRate = 1f;
    public int bulletsPerBurst = 3;      // jumlah tembakan sebelum pindah
    public float burstDelay = 0.5f;      // jeda antar tembakan dalam 1 burst

    [Header("Movement Settings")]
    public Vector2 positionRangeX = new Vector2(-8f, 8f);
    public Vector2 positionRangeY = new Vector2(-4f, 4f);
    public float overlapCheckRadius = 1f;
    public int maxRetryCount = 5;

    private Vector2 targetPosition;
    private bool isMoving = false;
    private bool isShooting = false;

    protected override void Start()
    {
        base.Start();
        targetPosition = transform.position;
        StartCoroutine(ShootThenMoveRoutine());
    }

    protected void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    // Siklus utama: tembak dulu → selesai → pindah → selesai → tembak lagi
    IEnumerator ShootThenMoveRoutine()
    {
        while (true)
        {
            // === FASE TEMBAK ===
            isShooting = true;
            yield return StartCoroutine(DoBurst());
            isShooting = false;

            // === FASE PINDAH ===
            Vector2 newTarget = GetValidPosition();
            if (newTarget != Vector2.zero)
            {
                targetPosition = newTarget;
                isMoving = true;

                // Tunggu sampai enemy benar-benar sampai di posisi tujuan
                yield return new WaitUntil(() => !isMoving);
            }
        }
    }

    // Tembak sebanyak bulletsPerBurst dengan jeda burstDelay
    IEnumerator DoBurst()
    {
        for (int i = 0; i < bulletsPerBurst; i++)
        {
            Attack();
            yield return new WaitForSeconds(burstDelay);
        }
    }

    Vector2 GetValidPosition()
    {
        for (int i = 0; i < maxRetryCount; i++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(positionRangeX.x, positionRangeX.y),
                Random.Range(positionRangeY.x, positionRangeY.y)
            );

            if (!IsPositionOccupied(candidate))
                return candidate;

            Debug.Log($"Posisi {candidate} ditempati, coba lagi... ({i + 1}/{maxRetryCount})");
        }

        Debug.Log($"{gameObject.name} tidak menemukan posisi kosong.");
        return Vector2.zero;
    }

    bool IsPositionOccupied(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, overlapCheckRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject && hit.CompareTag("Enemy"))
                return true;
        }

        return false;
    }

    void Attack()
    {
        SpreadShot(3, 60f);
    }

    void SpreadShot(int bulletCount, float spreadAngle)
    {
        float startAngle = -spreadAngle / 2;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetEnemyBullet();

            if (bullet != null)
            {
                bullet.transform.position = transform.position;

                Vector2 direction = (player.position - transform.position).normalized;
                float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float currentAngle = baseAngle + startAngle + (angleStep * i);

                bullet.transform.rotation = Quaternion.Euler(0, 0, currentAngle - 90);
                bullet.SetActive(true);
            }
        }
    }
}