using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class Demon : Enemy
{
    private Transform[] Targets;
    private int i = 0;
    private PlayerLeveling player;
    private static Demon currentApproachingDemon = null;
    public bool isAttacking = false;
    [SerializeField] private float patrolRadius = 5f;

    public bool cooldown = false;

    public int currentAttack = 0;

    public override void Start()
    {
        base.Start(); // Call base Start to set up camp reference

        health = 40f;
        alert = false;
        animator = GetComponent<Animator>();
        StartCoroutine(Delay());
        audioSource = GetComponent<AudioSource>();

        if (playerObject != null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player").transform;
            //player = playerObject.GetComponent<Barbarian>();
        }

        CreatePatrolPoints();
        StartPatrole();
    }

    public override void Update()
    {
        if (playerObject != null && health > 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.position);

            // Use camp range instead of hardcoded value
            if (IsPlayerInCampRange() && !IsPlayerInAttackRange() && currentApproachingDemon == null)
            {
                currentApproachingDemon = this;
                ApproachPlayer();
            }

            if (currentApproachingDemon == this)
            {
                agent.SetDestination(playerObject.position);

                if (IsPlayerInCampRange() && !isAttacking)
                {
                    ApproachPlayer();
                }


                if (IsPlayerInAttackRange())
                {
                    agent.isStopped = true;  // Stop movement when in attack range
                    animator.SetBool("run", false);
                    animator.SetBool("walking", false);
                    if (!isAttacking && !cooldown)
                    {
                        StartCoroutine(AttackPattern());
                    }
                }
                else
                {
                    agent.isStopped = false;  // Resume movement when out of attack range
                    if (isAttacking)
                    {
                        StopAttacking();
                        ApproachPlayer();
                    }
                }

                // Reset if player leaves camp range
                if (!IsPlayerInCampRange())
                {
                    alert = false;
                    StopAttacking();
                    animator.SetBool("IsIdle", true);
                    currentApproachingDemon = null;
                    StartPatrole();
                }
                else if (!IsPlayerInAttackRange())
                {
                    ApproachPlayer();
                }

                return;
            }
        }
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

    IEnumerator AttackPattern()
    {
        isAttacking = true;
        currentAttack = 0;

        while (IsPlayerInAttackRange() && health > 0)
        {
            switch (currentAttack)
            {
                case 0:
                case 1:
                    if (!cooldown)
                    {
                        cooldown = true;
                        SwordAttack();
                        currentAttack++;
                        yield return new WaitForSeconds(5f); // Wait for cooldown
                        cooldown = false;
                    }
                    break;

                case 2:
                    if (!cooldown)
                    {
                        cooldown = true;
                        BombAttack();
                        currentAttack = 0; // Reset to start the pattern again
                        yield return new WaitForSeconds(5f); // Wait for cooldown
                        cooldown = false;
                    }
                    break;
            }

            yield return null; // Wait for next frame
        }

        // Reset when pattern ends
        isAttacking = false;
        cooldown = false;
        currentAttack = 0;

        // Only approach if player is in camp range but out of attack range
        if (IsPlayerInCampRange() && !IsPlayerInAttackRange())
        {
            ApproachPlayer();
        }
    }


    void SwordAttack()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("attack1");
        StartCoroutine(Reset());

        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 2f)
        {
            player.TakeDamage(10);
        }
    }

    void BombAttack()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("attack2");
        StartCoroutine(Reset());

        if (playerObject != null && Vector3.Distance(transform.position, playerObject.position) <= 2f)
        {
            player.TakeDamage(15);
        }
    }

    void StartPatrole()
    {
        Targets = null;
        CreatePatrolPoints();
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
        animator.SetBool("walking", true);
        i = (i + 1) % Targets.Length; // Continue patrol
        agent.SetDestination(Targets[i].position);
    }

    public override void ApproachPlayer()
    {
        agent.isStopped = false; // Make sure the agent can move
        agent.SetDestination(playerObject.position);
        animator.SetBool("isIdle", false);
        if (playerObject)
        {
            agent.SetDestination(playerObject.position);
            animator.SetBool("run", true);
            animator.SetInteger("attack", 0);
            alert = true;
        }
    }

    public override void GetStunned()
    {
        animator.SetTrigger("stunned");
        StartCoroutine(Reset());
    }

    //public override void Die()
    //{
    //    animator.SetBool("isIdle", false);
    //    animator.SetTrigger("death");
    //    audioSource.PlayOneShot(deathSound);

    //    if (currentApproachingDemon == this)
    //    {
    //        currentApproachingDemon = null; // Release control so another demon can approach
    //    }
    //    StartCoroutine(Delay());
    //    Destroy(gameObject);
    //}

    public override void TakeDamage()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("damage");
        health -= 5f;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public override void TakeDamage(int damage)
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("damage");
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
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
        StopCoroutine("AttackPattern");
        isAttacking = false;
        // Remove the IsIdle settings from here
    }

    private void StopWalking()
    {
        agent.isStopped = true;
        animator.SetBool("walking", false);
        animator.SetBool("run", false);
        // Only set IsIdle when explicitly stopping
        animator.SetBool("IsIdle", true);
    }

    public override IEnumerator Reset()
    {
        animator.SetBool("IsIdle", true);
        yield return new WaitForSeconds(5f);

        // Only reset IsIdle if we're still attacking
        if (isAttacking)
        {
            animator.SetBool("IsIdle", false);
        }
    }

}
