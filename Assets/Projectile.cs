using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;

    private float timer;

    void OnEnable()
    {
        timer = lifeTime;
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.TakeDamage(1);

            gameObject.SetActive(false);
        }
    }
}     
    