using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float radius = 2f;
    GameObject player;
    Character playerCharacter;
    CharacterCombat playerCombat;
    PlayerInput playerInput;

    Character currentCharacter;
    CharacterCombat enemyCombat;
    [HideInInspector] public NavMeshAgent navAgent;

    [SerializeField] private Image exclamationMark;
    public float reactionTime = 1f;
    [HideInInspector] public float distance;
    [SerializeField] private Image questionMark;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;
        playerCharacter = PlayerManager.instance.player.GetComponent<Character>();
        playerCombat = PlayerManager.instance.player.GetComponent<CharacterCombat>();
        playerInput = PlayerManager.instance.player.GetComponent<PlayerInput>();

        currentCharacter = GetComponent<Character>();
        enemyCombat = GetComponent<CharacterCombat>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = this.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<Character>().canTakeDamage)
        {
            this.GetComponent<CharacterCombat>().canAttack = false;
            this.GetComponent<CharacterCombat>().attackCooldown = 500f;

        }

        distance = Vector3.Distance(transform.position, player.transform.position);
        
        if (Input.GetKeyDown(playerInput.attack))
        {
            if (distance <= radius)
            {
                if (playerCombat != null)
                    playerCombat.MeleeAttack(currentCharacter);
            }
        }

        if (distance <= navAgent.stoppingDistance)
        {
            if (enemyCombat.attackCooldown < reactionTime)
                exclamationMark.enabled = true;

            if (!enemyCombat.canAttack || distance >= navAgent.stoppingDistance)
                Invoke("DisableImage", 0.2f);
                
            enemyCombat.MeleeAttack(playerCharacter);
        }
    }
    
    void OnDrawGizmos() // Gizmo for debug
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void DisableImage()
    {
        exclamationMark.enabled = false;
    }

    public void EnableQuestionMark()
    {
        questionMark.enabled = true;
        questionMark.GetComponent<Animator>().SetBool("IsSpinning", true);
    }

    public void DisableQuestionMark()
    {
        questionMark.enabled = false;
        questionMark.GetComponent<Animator>().SetBool("IsSpinning", false);
    }
}
