using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform playerObject;
    public NavMeshAgent agent;
    public Animator animator;
    public bool alert = false;

    // Health and Attack Variables
    public float health ;
    public float currentAttack = 0;

    // Audio Variables
    [SerializeField] public AudioClip deathSound;
    public AudioSource audioSource;

    // Core Lifecycle Methods
    public virtual void Start();

    public virtual void Update();

    // Movement Methods
    public virtual void ApproachPlayer();

    // State and Damage Methods
    public virtual void TakeDamage();

    public virtual void GetStunned();

    public virtual void Die();

    // Coroutines
    public virtual IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
    }

    public virtual IEnumerator Reset()
    {
        yield return new WaitForSeconds(5f);
    }
}