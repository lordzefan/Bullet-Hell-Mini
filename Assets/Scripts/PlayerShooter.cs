using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float fireRate = 0.2f;

    private float nextFireTime;

    private void Update()
    {
        InputShoot();
    }

    private void InputShoot()
    {
        if (Input.GetMouseButton(0)
            && Time.time >= nextFireTime)
        {
            Shoot();

            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        GameObject bullet =
            ObjectPool.Instance.GetPlayerBullet();

        if (bullet == null) return;

        bullet.transform.position =
            firePoint.position;

        bullet.transform.rotation =
            firePoint.rotation;

        bullet.SetActive(true);
    }
}
