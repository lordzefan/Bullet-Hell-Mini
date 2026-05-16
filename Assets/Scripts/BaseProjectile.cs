using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField]protected ProjectileData projectileData; 

    private float timer;

    protected virtual void OnEnable()
    {
        timer = projectileData.lifeTime;
    }

    protected virtual void Update()
    {
        MoveProjectile();

        LifeTimeProjectile();
    }

    protected virtual void LifeTimeProjectile()
    {
       timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        } 
    }

    protected virtual void MoveProjectile()
    {
        transform.position +=
            transform.up * projectileData.speed * Time.deltaTime;
    }

    protected void DisableProjectile()
    {
        gameObject.SetActive(false);
    }
}