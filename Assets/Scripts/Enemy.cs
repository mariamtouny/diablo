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
    public float health = 40f;
    public float currentAttack = 0;

    // Audio Variables
    [SerializeField] public AudioClip deathSound;
    public AudioSource audioSource;

    // Core Lifecycle Methods
    public virtual void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        // Basic initialization to be implemented by child classes
    }

    public virtual void Update()
    {
        // Basic update logic to be implemented by child classes
    }

    // Movement Methods
    public virtual void ApproachPlayer() { }
<<<<<<< HEAD

    // State and Damage Methods

=======
>>>>>>> ed7bfd93b0038c801b660db5f49cec471a0336a5
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
            Die();
        }
    }

    public virtual void GetStunned()
    {
        // Basic stun logic
    }

    public virtual void Die()
    {
        // Basic death logic
        Debug.Log("enemy died");
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