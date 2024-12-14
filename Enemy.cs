using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    //public Transform playerObject;
=======
    public Transform playerObject;
>>>>>>> 425f62003e3c212f09c0399d45f61408f2aff59d
=======
    //public Transform playerObject;
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
    //public Transform playerObject;
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
    //public Transform playerObject;
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
    public Transform playerObject;
>>>>>>> 425f62003e3c212f09c0399d45f61408f2aff59d
=======
    public Transform playerObject;
>>>>>>> 425f62003e3c212f09c0399d45f61408f2aff59d
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
    public virtual void TakeDamage(int damage)
    {
        // Basic damage logic
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage()
    {
        // Basic damage logic
        health -= 5;
        if (health <= 0)
        {
            Die();
        }
    }
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
    public virtual void TakeDamage();
>>>>>>> 425f62003e3c212f09c0399d45f61408f2aff59d
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
>>>>>>> d469a7ef73a6f0b749da8f70ec6bf505f4ce1ed6
=======
    public virtual void TakeDamage();
>>>>>>> 425f62003e3c212f09c0399d45f61408f2aff59d
=======
    public virtual void TakeDamage();
>>>>>>> 425f62003e3c212f09c0399d45f61408f2aff59d

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