using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : BaseEnemy
{
    [Header("Shoot Settings")]
    [SerializeField] ProjectileData fireRate;
    [SerializeField] int projectilePerMove = 3;      // jumlah tembakan sebelum pindah
    [SerializeField] int amount = 3;
    [SerializeField] float angle = 60f;
    

    [Header("Movement Settings")]
    [SerializeField] Vector2 positionRangeX = new Vector2(-8f, 8f);
    [SerializeField] Vector2 positionRangeY = new Vector2(-4f, 4f);
    [SerializeField] float overlapCheckRadius = 1f;
    [SerializeField] int maxRetryCount = 5;

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
        LookAtPlayer();
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

    // FIRE
    IEnumerator ShootThenMoveRoutine()
    {
        while (true)
        {
            // === FASE SHOOT===
            isShooting = true;
            yield return StartCoroutine(DoBurst());
            isShooting = false;

            // === FASE MOVE ===
            Vector2 newTarget = GetValidPosition();
            if (newTarget != Vector2.zero)
            {
                targetPosition = newTarget;
                isMoving = true;

                yield return new WaitUntil(() => !isMoving);
            }
        }
    }

    // FIRE dELAY
    IEnumerator DoBurst()
    {
        for (int i = 0; i < projectilePerMove; i++)
        {
            Attack();
            yield return new WaitForSeconds(fireRate.fireRate);
        }
    }


    // mOVE
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

    // CHECK OVERLAPPING
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
        SpreadShot(amount, angle);
    }

    void SpreadShot(int bulletCount, float spreadAngle)
    {
        float startAngle = -spreadAngle / 2;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(projectileType);

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