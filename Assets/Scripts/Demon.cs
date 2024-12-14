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

    private static Demon currentApproachingDemon = null; // Tracks the demon currently approaching the player
    private bool isAttacking = false; // Tracks if the demon is currently attacking

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

        StartPatrole();
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
        if (playerObject != null && health > 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.position);

            // Follow the player dynamically while within range (10 units)
            if (distanceToPlayer <= 10f && currentApproachingDemon == null)
            {
                currentApproachingDemon = this;
                ApproachPlayer();
            }

            // If already approaching the player, follow dynamically or attack
            if (currentApproachingDemon == this)
            {
                // Follow the player's position
                agent.SetDestination(playerObject.position);

                if (distanceToPlayer <= 10f)
                {
                    ApproachPlayer();
                }

                // Stop and attack when within 1 unit
                if (distanceToPlayer <= 1f)
                {
                    StopWalking();
                    if (!isAttacking) StartCoroutine(AttackPattern());
                }
                else
                {
                    StopAttacking(); // Stop attack if the player moves away
                    ApproachPlayer();
                }

                // If the player moves out of range, return to patrol
                if (distanceToPlayer > 10f)
                {
                    StopAttacking();
                    StopWalking();
                    currentApproachingDemon = null;
                    StartPatrole();
                    Patrole();
                }

                return; // Exit Update to avoid interrupting follow/attack logic
            }
        }

        // Patrol Logic: Only execute when not following the player
        if (!alert && agent.remainingDistance < 0.5f)
        {
            Patrole();
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
    }


    IEnumerator AttackPattern()
    {
        isAttacking = true;

        while (health > 0)
        {
            SwordAttack();
            yield return new WaitForSeconds(2f);

            SwordAttack();
            yield return new WaitForSeconds(2f);

            BombAttack();
            yield return new WaitForSeconds(2f);
        }

        isAttacking = false;
    }

    void SwordAttack()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("attack1");
        StartCoroutine(Reset());

        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 1f)
        {
            player.TakeDamage(10);
        }
    }

    void BombAttack()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("attack2");
        StartCoroutine(Reset());

        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 1f)
        {
            player.TakeDamage(15);
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
        i = (i + 1) % Targets.Length; // Continue patrol
        agent.SetDestination(Targets[i].position);
    }

    public override void ApproachPlayer()
    {
        animator.SetBool("isIdle", false);
        if (playerObject)
        {
            agent.SetDestination(playerObject.position); // Follow player's dynamic position
            animator.SetBool("run", true);
            animator.SetInteger("attack", 0);
            alert = true;
        }
    }

    public override IEnumerator Die()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("death");
        audioSource.PlayOneShot(deathSound);

        if (currentApproachingDemon == this)
        {
            currentApproachingDemon = null; // Release control so another demon can approach
        }
        yield return new WaitForSeconds(5.5f);
        Destroy(gameObject);
    }

    private void StopAttacking()
    {
        isAttacking = false; // Stop the attack coroutine
        animator.SetBool("isIdle", true);
        StopAllCoroutines(); // Stop any ongoing coroutines
    }

    private void StopWalking()
    {
        agent.isStopped = true; // Stop the NavMeshAgent
        animator.SetBool("walking", false);
        animator.SetBool("run", false);
        animator.SetBool("isIdle", true);
    }
}
