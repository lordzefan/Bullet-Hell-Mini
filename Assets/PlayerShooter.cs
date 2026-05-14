using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float fireRate = 0.2f;

    private float nextFireTime;

    void Update()
    {
        InputShoot();
    }

    void InputShoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();

            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }
}
