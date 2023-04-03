using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public HealthBar healthBar;

    [Header ("Combat")]
    // Attack
    public int attackDamage;
    public float attackSpeed = 1f;

    // Parry
    public bool isParrying;
    public float parrySpeed = 3f;
    public float parryDuration = 1f;
    private bool canTakeDamage = true;



    public bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
            CheckHealth();
        }
    }

    public void CanTakeDamage(bool canTakeDamage)
    {
        this.canTakeDamage = canTakeDamage;
    }

    public void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetHealth(currentHealth);
    }

    public void Die()
    {
        this.isDead = true;

        if (GetComponent<Enemy>() != null)
        {
            Destroy(gameObject);
        }
    }
}
