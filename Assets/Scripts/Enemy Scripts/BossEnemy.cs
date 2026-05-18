using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Health")]
    [SerializeField ] int plusHealth = 10;

    [Header("Phase 1 - Minigun")]
    [SerializeField] ProjectileData phase1Projectile;

    [Header("Phase 2 - Radial")]
    [SerializeField]ProjectileData phase2Projectile;

    [SerializeField]float moveToCenterSpeed = 5f;

    [SerializeField]int bulletCount = 10;

    [SerializeField]float rotateSpeed = 90f;
    float rotationAngle;

    [Header("Projectile Type")]
    
    [SerializeField]string projectileType2 = "BossProjectile2";

    int currentPhase = 1;

    bool isTransitioning;



    Coroutine phase1Routine;
    Coroutine phase2Routine;

    protected override void Start()
    {
        base.Start();

        phase1Routine = StartCoroutine(Phase1Routine());
        phase2Projectile.fireRate = 1;
    }

    private void Update()
    {
        if (currentPhase != 2 || isTransitioning)
            return;

        
        transform.Rotate(new Vector3(0, 0, rotateSpeed ) * Time.deltaTime);
    }


    // DAMAGE
    public override void TakeDamage(int damage)
    {
        if (isTransitioning || currentHealth <= 0)return;

        currentHealth -= damage;

        if (currentHealth > 0)return;

        if (currentPhase == 1)
        {
            StartCoroutine(TransitionToPhase2());
        }
        else
        {
            Die();
        }
    }

  
    // PHASE 1
    private IEnumerator Phase1Routine()
    {
        while (currentPhase == 1)
        {
            ShootAtPlayer();

            yield return new WaitForSeconds(
                phase1Projectile.fireRate
            );
        }
    }

    private void ShootAtPlayer()
    {
        GameObject bullet =ObjectPool.Instance.GetObject(projectileType);

        if (bullet == null)
            return;

        bullet.transform.position = transform.position;

        Vector2 direction =
            (player.position - transform.position).normalized;

        float angle =Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bullet.transform.rotation =Quaternion.Euler(0, 0, angle - 90 );

        bullet.SetActive(true);
    }

    // TRANSITION
    private IEnumerator TransitionToPhase2()
    {
        isTransitioning = true;

        StopCoroutine(phase1Routine);

        Vector3 centerPosition = Vector3.zero;

        while (Vector3.Distance( transform.position, centerPosition) > 0.1f)
        {
            transform.position =
                Vector3.MoveTowards( transform.position, centerPosition, moveToCenterSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = centerPosition;

        yield return new WaitForSeconds(1f);

        currentPhase = 2;

        currentHealth = maxHealth + plusHealth;

        rotationAngle = 0f;

        isTransitioning = false;

        phase2Routine =
            StartCoroutine(Phase2Routine());
    }

   
    // PHASE 2
    private IEnumerator Phase2Routine()
    {
        while (currentPhase == 2)
        {
            FireRadial();

            yield return new WaitForSeconds(phase2Projectile.fireRate);
        }
    }

    private void FireRadial()
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle =rotationAngle + (angleStep * i);

            GameObject bullet = ObjectPool.Instance.GetObject( projectileType2);

            if (bullet == null)
                continue;

            bullet.transform.position =
                transform.position;

            bullet.transform.rotation = Quaternion.Euler( 0,0, angle - 90f);

            bullet.SetActive(true);
        }
    }


    // DIE
    protected override void Die()
    {
        if (phase1Routine != null) StopCoroutine(phase1Routine);

        if (phase2Routine != null) StopCoroutine(phase2Routine);

        base.Die();
    }
}