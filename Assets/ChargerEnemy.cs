using UnityEngine;
using System.Collections;

public class ChargerEnemy : EnemyBase
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.3f;
    public float cooldown = 2f;

    private bool isDashing;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);

            isDashing = true;

            Vector2 direction =
                (player.position - transform.position).normalized;

            float timer = dashDuration;

            while (timer > 0)
            {
                transform.position +=
                    (Vector3)(direction * dashSpeed * Time.deltaTime);

                timer -= Time.deltaTime;

                yield return null;
            }

            isDashing = false;
        }
    }
}
