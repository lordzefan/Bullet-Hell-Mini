using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseHealth
{
    public int collionDamage = 1;
    public float moveSpeed = 3f;
    public string projectileKind = "EnemyProjectile";
    private ParticleSystem damageParticle;
    protected Transform player;


    protected override void Awake()
    {
        base.Awake();
        GameObject particle = GameObject.FindWithTag("particle");
        damageParticle = particle.GetComponentInChildren<ParticleSystem>();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player =
        other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(collionDamage);

            Die();
        }
    }

    protected virtual void Die()
    {   
        damageParticle.transform.position = transform.position;
        damageParticle.Play();
        GameManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }

    protected void LookAtPlayer()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
