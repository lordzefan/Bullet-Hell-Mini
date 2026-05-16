using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : BaseHealth
{
    private SpriteRenderer spriteRenderer;

    private bool isInvulnerable;
    public TextMeshProUGUI healthText;

    protected override void Start()
    {
        base.Start();
         spriteRenderer =
            GetComponent<SpriteRenderer>();
            UpdateHealthUI();
    }

    public override void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        StartCoroutine(DamageFlash());
        base.TakeDamage(damage);
        UpdateHealthUI();

    }

    public void UpdateHealthUI()
    {
        healthText.text =
            "HP : " + currentHealth;
    }
    IEnumerator DamageFlash()
    {
        isInvulnerable = true;

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.4f);

        isInvulnerable = false;
    }

   
}
