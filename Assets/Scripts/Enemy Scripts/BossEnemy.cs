using System.Collections;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Health")]
    public int phase1Health = 100;
    public int phase2Health = 150;

    [Header("Phase 1 - Minigun")]
    [SerializeField] ProjectileData phase1FireRate;
    public float phase1Cooldown;

    [Header("Phase 2 - Radial")]
    public float moveToCenterSpeed = 5f;
    public int bulletCount = 10;
    [SerializeField] ProjectileData phase2FireRate;

    public float rotateSpeed = 90f;

    [Header("Projectile")]
    public string projectilePoolTag = "EnemyBullet";

    private int currentPhase = 1;
    private bool isTransitioning;
    private float rotationAngle;

    private Coroutine phase1Routine;
    private Coroutine phase2Routine;

    protected override void Start()
    {
        base.Start();

        currentHealth = phase1Health;

        phase1Routine = StartCoroutine(Phase1Routine());
    }

    void Update()
    {
        if (currentPhase == 2 && !isTransitioning)
        {
            rotationAngle += rotateSpeed * Time.deltaTime;

            if (rotationAngle >= 360f)
                rotationAngle -= 360f;
        }
    }

    // =========================
    // DAMAGE
    // =========================
    public override void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        if (isTransitioning) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (currentPhase == 1)
            {
                StartCoroutine(TransitionToPhase2());
            }
            else
            {
                Die();
            }
        }
    }

    // =========================
    // PHASE 1
    // =========================
    IEnumerator Phase1Routine()
    {
        phase1Cooldown = 0f;

        while (currentPhase == 1 && !isTransitioning)
        {
            ShootAtPlayer();

            phase1Cooldown = phase1FireRate.fireRate;

            while (phase1Cooldown > 0f)
            {
                phase1Cooldown -= Time.deltaTime;
                yield return null;
            }
        }
    }

    void ShootAtPlayer()
    {
        GameObject bullet =
            ObjectPool.Instance.GetObject(projectilePoolTag);

        if (bullet == null) return;

        bullet.transform.position = transform.position;

        Vector2 dir =
            (player.position - transform.position).normalized;

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        bullet.transform.rotation =
            Quaternion.Euler(0, 0, angle - 90f);

        bullet.SetActive(true);
    }

    // =========================
    // TRANSITION
    // =========================
    IEnumerator TransitionToPhase2()
    {
        isTransitioning = true;

        // STOP ONLY PHASE 1 (NOT ALL COROUTINES)
        if (phase1Routine != null)
            StopCoroutine(phase1Routine);

        Vector3 target = new Vector3(0, 0, 0);

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveToCenterSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = target;

        yield return new WaitForSeconds(1f);

        currentPhase = 2;
        currentHealth = phase2Health;
        rotationAngle = 0f;
        isTransitioning = false;

        phase2Routine = StartCoroutine(Phase2Routine());
    }

    // =========================
    // PHASE 2
    // =========================
    IEnumerator Phase2Routine()
    {
        while (currentPhase == 2 && !isTransitioning)
        {
            FireRadial();

            yield return new WaitForSeconds(phase2FireRate.fireRate);
        }
    }

    void FireRadial()
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = rotationAngle + (angleStep * i);

            GameObject bullet =
                ObjectPool.Instance.GetObject(projectilePoolTag);

            if (bullet == null) continue;

            bullet.transform.position = transform.position;

            bullet.transform.rotation =
                Quaternion.Euler(0, 0, angle - 90f);

            bullet.SetActive(true);
        }
    }

    // =========================
    // DIE
    // =========================
    protected override void Die()
    {
        GameManager.Instance.EnemyKilled();

        if (phase1Routine != null)
            StopCoroutine(phase1Routine);

        if (phase2Routine != null)
            StopCoroutine(phase2Routine);

        gameObject.SetActive(false);
    }
}