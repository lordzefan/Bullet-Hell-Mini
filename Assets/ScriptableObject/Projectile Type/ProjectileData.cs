using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public float speed = 15f;
    public float lifeTime = 3f;
    public int damage = 1;
    public float fireRate = 1f;
}
