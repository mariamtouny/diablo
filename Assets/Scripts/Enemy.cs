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
<<<<<<< HEAD
<<<<<<< HEAD
    public float health;
    public float currentAttack = 0;

    // Audio Variables
    //[SerializeField] public AudioClip deathSound;
    //public AudioSource audioSource;
=======
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
    public float health = 40f;
    public float currentAttack = 0;

    // Audio Variables
    [SerializeField] public AudioClip deathSound;
    public AudioSource audioSource;
<<<<<<< HEAD
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6

    // Core Lifecycle Methods
    public virtual void Start()
    {
<<<<<<< HEAD
<<<<<<< HEAD
=======
        playerObject = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
        playerObject = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
        // Basic initialization to be implemented by child classes
    }

    public virtual void Update()
    {
        // Basic update logic to be implemented by child classes
    }

    // Movement Methods
    public virtual void ApproachPlayer() { }
<<<<<<< HEAD
<<<<<<< HEAD
    // State and Damage Methods
    public virtual void TakeDamage() { }

    public virtual void GetStunned() { }

    public virtual void Die() { }
=======
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6

    // State and Damage Methods
    public virtual void TakeDamage()
    {
        // Basic damage logic
        health -= 5f;
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
<<<<<<< HEAD
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6

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