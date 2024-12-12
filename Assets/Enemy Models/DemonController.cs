using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DemonController : MonoBehaviour
{
    public Transform[] Targets;
    public Transform playerObject;
    private NavMeshAgent a;
    private int i = 0;
    private Animator animator;
    private float currentAttack = 0;
    public float health = 40f;
    private bool alert = false;

    //sound effects
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    void Start()
    {
        a = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(Delay());

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (animator.GetBool("walking") && a.remainingDistance < 0.5f && !alert)
        {
            patrole();
        }

        // P key press just sets the destination and alert state
        if (Input.GetKeyDown(KeyCode.P))
        {
            approachPlayer();
        }

        // Separate check for reaching the player
        if (alert && a.remainingDistance < 0.5f)
        {
            StopRunning();
        }

        // Rest of your code remains the same...
        if (Input.GetKeyDown(KeyCode.X))
        {
            takeDamage();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            getStunned();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            swordAttack();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            bombAttack();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            startWalking();
        }
    }

    void startWalking()
    {
        a.SetDestination(Targets[i].position);
        animator.SetBool("walking", true);
        animator.SetInteger("attack", 0);
        alert = false;
    }

    void getStunned()
    {
        animator.SetTrigger("stunned");
        StartCoroutine(Reset());
    }


    void takeDamage()
    {
        animator.SetTrigger("damage");
        health -= 5f;
        if (health <= 0)
        {
            die();
        }
        StartCoroutine(Reset());
    }

    void patrole()
    {
        animator.SetInteger("attack", 0);
        i = (i + 1) % Targets.Length;
        a.SetDestination(Targets[i].position);
    }

    void approachPlayer()
    {
        if (playerObject)
            a.SetDestination(playerObject.position);
        animator.SetBool("run", true);
        animator.SetInteger("attack", 0);
        alert = true;
    }

    void swordAttack()
    {
        animator.SetBool("walking", false);
        animator.SetTrigger("attack1");
        StartCoroutine(Reset());
    }

    void bombAttack()
    {
        animator.SetBool("walking", false);
        animator.SetTrigger("attack2");
        StartCoroutine(Reset());
    }
    

    void die()
    {
        animator.SetTrigger("death");
        audioSource.PlayOneShot(deathSound);
        Delay();
        //Destroy(gameObject);
    }

    //Coroutines

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(5f);
        animator.SetTrigger("isIdle");
    }
    private IEnumerator StopWalking()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("walking", false);
    }

    private IEnumerator StopRunning()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("run", false);
    }
}