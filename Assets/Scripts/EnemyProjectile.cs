using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : BaseProjectile
{
    private Vector2 customDirection;
    private bool useCustomDirection = false;

    public void Launch(Vector2 direction, float customSpeed = -1f)
    {
        customDirection = direction.normalized;
        useCustomDirection = true;

        if (customSpeed > 0f)
            projectileData.speed = customSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        useCustomDirection = false; 
        customDirection = Vector2.zero;
    }

    protected override void MoveProjectile()
    {
        if (useCustomDirection)
            transform.position += (Vector3)(customDirection * projectileData.speed * Time.deltaTime);
        else
            base.MoveProjectile(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(projectileData.damage);
            gameObject.SetActive(false);
        }
    }
}