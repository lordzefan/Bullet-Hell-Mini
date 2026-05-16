using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Boss Stats")]
    public int maxHealthPhase1 = 100;
    public int maxHealthPhase2 = 50;

    [Header("Phase 1 - Shoot At Player")]
    public float fireRatePhase1 = 0.05f;
    public float bulletSpeedPhase1 = 20f;

    [Header("Phase 2 - Rotate Shot")]
    public float moveToCenterX = 0f;
    public float moveToCenterY = 0f;
    public float moveSpeedToCenter = 4f;
    public int bulletCountPhase2 = 12;
    public float rotateSpeed = 90f;
    public float bulletSpeedPhase2 = 8f;
    public float fireRatePhase2 = 0.05f;

    [Header("Effects")]
    public GameObject phaseTransitionEffect;

    private int currentPhase = 1;
    private bool isTransitioning = false;
    private Vector3 spawnPosition;
    private float currentRotationAngle = 0f;

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealthPhase1;
        spawnPosition = transform.position;

        StartCoroutine(Phase1Routine());
    }

    protected void Update()
    {
        if (currentPhase == 2 && !isTransitioning)
        {
            currentRotationAngle += rotateSpeed * Time.deltaTime;
            if (currentRotationAngle >= 360f)
                currentRotationAngle -= 360f;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isTransitioning) return;

        currentHealth -= damage;

        if (currentPhase == 1 && currentHealth <= 0)
            StartCoroutine(TransitionToPhase2());
        else if (currentPhase == 2 && currentHealth <= 0)
            Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(collionDamage);
    }

    // =====================
    // PHASE 1 - SHOOT AT PLAYER
    // =====================

    IEnumerator Phase1Routine()
    {
        while (currentPhase == 1 && !isTransitioning)
        {
            Vector2 directionToPlayer =
                (player.position - transform.position).normalized;

            FireBulletInDirection(directionToPlayer, bulletSpeedPhase1);

            yield return new WaitForSeconds(fireRatePhase1);
        }
    }

    // =====================
    // TRANSISI FASE
    // =====================

    IEnumerator TransitionToPhase2()
    {
        isTransitioning = true;
        Debug.Log("BOSS: Transisi ke Fase 2!");

        if (phaseTransitionEffect != null)
            Instantiate(phaseTransitionEffect, transform.position, Quaternion.identity);

        Vector3 centerPosition = new Vector3(moveToCenterX, moveToCenterY, 0f);

        while (Vector3.Distance(transform.position, centerPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                centerPosition,
                moveSpeedToCenter * Time.deltaTime
            );
            yield return null;
        }

        transform.position = centerPosition;
        yield return new WaitForSeconds(0.5f);

        currentPhase = 2;
        currentHealth = maxHealthPhase2;
        currentRotationAngle = 0f;
        isTransitioning = false;

        Debug.Log("BOSS: Fase 2 dimulai!");
        StartCoroutine(Phase2Routine());
    }

    // =====================
    // PHASE 2 - ROTATE SHOT
    // =====================

    IEnumerator Phase2Routine()
    {
        while (currentPhase == 2 && !isTransitioning)
        {
            FireRotateBurst(currentRotationAngle);

            yield return new WaitForSeconds(fireRatePhase2);
        }
    }

    void FireRotateBurst(float startAngle)
    {
        float angleStep = 360f / bulletCountPhase2;

        for (int i = 0; i < bulletCountPhase2; i++)
        {
            float angle = startAngle + (angleStep * i);
            FireBulletInDirection(AngleToDirection(angle), bulletSpeedPhase2);
        }
    }

    // =====================
    // HELPER
    // =====================

    void FireBulletInDirection(Vector2 direction, float speed)
    {
        GameObject bulletObj = ObjectPool.Instance.GetEnemyBullet();
        if (bulletObj == null) return;

        bulletObj.transform.position = transform.position;

        EnemyProjectile projectile = bulletObj.GetComponent<EnemyProjectile>();
        if (projectile != null)
            projectile.Launch(direction, speed);

        bulletObj.SetActive(true);
    }

    Vector2 AngleToDirection(float angleDegrees)
    {
        float rad = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    // =====================
    // DIE
    // =====================

    protected override void Die()
    {
        Debug.Log("BOSS: Mati!");
        GameManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }
}