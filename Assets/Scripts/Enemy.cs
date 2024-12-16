using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class Enemy : MonoBehaviour
{
    public Transform playerObject;
    public NavMeshAgent agent;
    public Animator animator;
    public bool alert = false;

    // Health and Attack Variables
    public float health;
    public float currentAttack = 0;

    // Audio Variables
    [SerializeField] public AudioClip deathSound;
    public AudioSource audioSource;

    // Camp reference
    protected EnemyCamp parentCamp;

    public virtual void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        parentCamp = GetComponentInParent<EnemyCamp>();

        if (parentCamp == null)
        {
            Debug.LogWarning($"No parent EnemyCamp found for {gameObject.name}!");
        }
    }

    public virtual void Update()
    {
    }

    // Check if player is within camp range
    protected bool IsPlayerInCampRange()
    {
        if (parentCamp == null || playerObject == null) return false;

        float distanceToPlayer = Vector3.Distance(parentCamp.transform.position, playerObject.position);
        return distanceToPlayer <= parentCamp.campRange;
    }

    protected bool IsPlayerInAttackRange()
    {
        if (playerObject == null) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.position);
        return distanceToPlayer <= 3f;
    }

    // Check if player is within attack range
    //protected bool IsPlayerInAttackRange()
    //{
    //    if (playerObject == null) return false;

    //    float distanceToPlayer = Vector3.Distance(transform.position, playerObject.position);
    //    return distanceToPlayer <= 2f; // Keep melee range constant
    //}


    // Movement Methods
    public virtual void ApproachPlayer() { }

    // State and Damage Methods

    public virtual void TakeDamage()
    {
        // Basic damage logic
        health -= 5;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        // Basic damage logic
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public virtual void GetStunned()
    {
        // Basic stun logic
    }

    public virtual IEnumerator Die()
    {
        // Basic death logic
        Debug.Log("enemy died");
        yield return null;
    }

    // Coroutines
    public virtual IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
    }

    public virtual IEnumerator Reset()
    {
        yield return new WaitForSeconds(5f);
    }

    public virtual IEnumerator StopRunning()
    {
        yield return new WaitForSeconds(0.5f);
    }
}