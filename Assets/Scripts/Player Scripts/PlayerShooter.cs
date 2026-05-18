using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private ProjectileData fireRate;

    private float nextFireTime;

    void Awake()
    {
    }

    private void Update()
    {
        InputShoot();
    }

    //CLICK TO SHOOT
    private void InputShoot()
    {
        if (Input.GetMouseButton(0)
            && Time.time >= nextFireTime)
        {
            Shoot();

            nextFireTime = Time.time + fireRate.fireRate ;
        }
    }

    private void Shoot()
    {
        GameObject bullet =ObjectPool.Instance.GetObject("PlayerProjectile");

        if (bullet == null) return;

        bullet.transform.position = firePoint.position;

        bullet.transform.rotation = firePoint.rotation;

        bullet.SetActive(true);
    }
}
