using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public Character currentCharacter;
    private Enemy enemy;

    // Attacking
    public float attackCooldown;
    public bool canAttack = true;

    // Parrying
    private float parryCooldown;
    private float parryTimer;
    public bool canParry = true;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        this.audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        currentCharacter = GetComponent<Character>();
    }

    void Update()
    {
        if (!canAttack)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
                canAttack = true;
        }

        if (!canParry)
        {
            parryCooldown -= Time.deltaTime;
            if (parryCooldown <= 0f)
            {
                canParry = true;
            }
        }

        if (currentCharacter.isParrying)
        {
            Debug.Log("PARRY");
            parryTimer += Time.deltaTime;
            if (parryTimer > currentCharacter.parryDuration)
                StopMeleeParry(currentCharacter);
        }
        else
        {
            parryTimer = 0f;
        }
    }

    public void MeleeAttack(Character targetCharacter)
    {
        if (canAttack && !currentCharacter.isParrying)
        {
            if (targetCharacter.isParrying)
            {
                GotParried();
                SuccessfulParry(targetCharacter);
                Debug.Log("Enemy got Parried!!!!");
            }
            else
            {
                if (targetCharacter.tag == "Player")
                    audioManager.Play("Enemy");
                else
                    audioManager.Play("Attack");
                
                targetCharacter.TakeDamage(currentCharacter.attackDamage);
                attackCooldown = 1 / currentCharacter.attackSpeed;
                canAttack = false;
            }
        }
    }

    public void StartMeleeParry(Character currentCharacter)
    {
        Debug.Log("Can't parry yet");
        if (canParry)
        {
            currentCharacter.isParrying = true;
        }
    }

    public void StopMeleeParry(Character currentCharacter)
    {
        parryCooldown = currentCharacter.parrySpeed;
        parryTimer = 0f;
        canParry = false;
        currentCharacter.isParrying = false;
    }


    /// <summary>
    /// Stops attack from attacker, and takes damage according to current attack damage. Gets double attack cooldown on next attack.
    /// </summary>
    public void GotParried()
    {
        currentCharacter.TakeDamage(currentCharacter.attackDamage);
        attackCooldown = 2 * (1 / currentCharacter.attackSpeed);
        canAttack = false;

        // UI: Questionmark spin over head
        if (currentCharacter.GetComponent<Enemy>() != null)
        {
            Debug.Log("INSIDE PARRY!");
            enemy = currentCharacter.GetComponent<Enemy>();
            enemy.EnableQuestionMark();
            Invoke("InvokeDisableFromEnemy", attackCooldown - enemy.reactionTime);
        }
    }

    /// <summary>
    /// If character gets a successfull parry, character gets a new attack instantly. 
    /// </summary>
    public void SuccessfulParry(Character targetCharacter)
    {
        audioManager.Play("Parry");
        targetCharacter.GetComponentInParent<CharacterCombat>().canAttack = true;
        targetCharacter.GetComponentInParent<CharacterCombat>().attackCooldown = 0f;
    }

    private void InvokeDisableFromEnemy()
    {
        enemy.DisableQuestionMark();
    }

}
