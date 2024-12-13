using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Demon : Enemy
{
    private NavMeshAgent a;
    private Transform[] Targets;
    private int i = 0;
    private PlayerLeveling player;
    public GameObject playerGameObject;
    public Transform playerObject;

    [SerializeField] private float patrolRadius = 5f; // Configurable patrol radius


    public override void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        playerObject = playerGameObject.transform;

        health = 40f; // Set a custom health value for the Demon
        alert = false; // Default alert state

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(Delay());
        audioSource = GetComponent<AudioSource>();

        if (playerObject != null)
        {
            player = playerGameObject.GetComponent<Barbarian>();
        }

        // Create patrol points
        CreatePatrolPoints();
    }

    void CreatePatrolPoints()
    {
        // Ensure the Targets array is initialized
        if (Targets == null || Targets.Length < 2)
        {
            Targets = new Transform[2];

            // Create first patrol point (to the right)
            GameObject patrolPoint1 = new GameObject("PatrolPoint1");
            patrolPoint1.transform.position = transform.position + transform.right * patrolRadius;
            Targets[0] = patrolPoint1.transform;

            // Create second patrol point (to the left)
            GameObject patrolPoint2 = new GameObject("PatrolPoint2");
            patrolPoint2.transform.position = transform.position - transform.right * patrolRadius;
            Targets[1] = patrolPoint2.transform;
        }
    }


    public override void Update()
    {
        if (animator.GetBool("walking") && agent.remainingDistance < 0.5f)
        {
            if (!alert)
            {
                Patrole();
            }
            else
            {
                animator.SetBool("walking", false);
                animator.SetBool("run", false);
                animator.SetBool("isIdle", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ApproachPlayer();
        }
        if (alert && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(StopRunning());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            GetStunned();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            SwordAttack();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            BombAttack();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartPatrole();
        }
    }

    void SwordAttack()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("walking", false);
        animator.SetTrigger("attack1");
        StartCoroutine(Reset());

        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 4f)
        {
            player.TakeDamage(10);
        }
    }

    void BombAttack()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("walking", false);
        animator.SetTrigger("attack2");
        StartCoroutine(Reset());

        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 8f)
        {
            player.TakeDamage(10);
        }
    }

    void StartPatrole()
    {
        animator.SetBool("isIdle", false);
        agent.SetDestination(Targets[i].position);
        animator.SetBool("walking", true);
        animator.SetInteger("attack", 0);
        alert = false;
    }

    void Patrole()
    {
        animator.SetBool("isIdle", false);
        animator.SetInteger("attack", 0);
        i = (i + 1) % Targets.Length;
        agent.SetDestination(Targets[i].position);
    }

    public override void GetStunned()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("stunned");
        StartCoroutine(Reset());
    }

    public override void TakeDamage()
    {
        animator.SetBool("isIdle", false);
        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 1f)
        {
            animator.SetTrigger("damage");
            health -= 5f;
            if (health <= 0)
            {
                Die();
            }
            StartCoroutine(Reset());
        }
    }

    public override void TakeDamage(int damage)
    {
        animator.SetBool("isIdle", false);
        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 1f)
        {
            animator.SetTrigger("damage");
            health -= 5f;
            if (health <= 0)
            {
                Die();
            }
            StartCoroutine(Reset());
        }
    }

    public override void ApproachPlayer()
    {
        animator.SetBool("isIdle", false);
        if (playerObject)
        {
            agent.SetDestination(playerObject.position);
            animator.SetBool("run", true);
            animator.SetInteger("attack", 0);
            alert = true;
        }
    }

    public override void Die()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("death");
        audioSource.PlayOneShot(deathSound);
        StartCoroutine(Delay());
    }

    private IEnumerator StopWalking()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("walking", false);
        animator.SetBool("isIdle", true);
    }

    private IEnumerator StopRunning()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("run", false);
        animator.SetBool("isIdle", true);
    }
}
