using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MinionController : Enemy
{
    public float speed = 2;
    private bool isDying = false;
    private bool cooldown = false;
    private bool isAttacking = false;
    private Vector3 startPosition;
    NavMeshAgent a;
    Animator n;

    private static MinionController[] alertedMinions = new MinionController[5];
    private static int alertedCount = 0;

    public override void Start()
    {
        base.Start();

        a = GetComponent<NavMeshAgent>();
        n = GetComponent<Animator>();
        startPosition = transform.position;
        a.speed = speed * Time.deltaTime * 100;
        a.isStopped = false;
        health = 20f;
        if (playerObject != null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public override void Update()
    {
        base.Update();
        if (isDying) return;

        bool wasAlerted = alert;
        bool isPlayerInRange = IsPlayerInCampRange();

        // Check if player has left the camp
        if (!isPlayerInRange && wasAlerted)
        {
            ClearAllAlertedMinions();
        }
        else if (isPlayerInRange)
        {
            if (!alert && TryAddToAlertedMinions())
            {
                alert = true;
                Debug.Log($"Minion {gameObject.name} alerted. Total alerted: {alertedCount}");
            }
        }

        if (!alert)
        {
            ReturnToStartPosition();
        }
        else // When alerted
        {
            if (!isAttacking)
            {
                a.updateRotation = true;
                a.isStopped = false;
                ApproachPlayerWithinCamp();
                if (IsPlayerInAttackRange())
                {
                    Attack(0);
                }
            }
        }

        HandleHealthAndDebugInputs();
    }

    private void ReturnToStartPosition()
    {
        if (Vector3.Distance(transform.position, startPosition) < 1f)
        {
            SetBoolsOff();
            n.SetBool("idle", true);
            a.isStopped = true;
            a.updateRotation = false;
        }
        else
        {
            SetBoolsOff();
            a.SetDestination(startPosition);
            a.isStopped = false;
            a.updateRotation = true;
            // Only show walking animation when alerted
            n.SetBool("idle", true);
        }
    }

    private void ApproachPlayerWithinCamp()
    {
        if (parentCamp == null) return;

        Vector3 targetPosition = playerObject.position;
        float distanceToCampCenter = Vector3.Distance(targetPosition, parentCamp.transform.position);

        // If target is outside camp range, clamp it to camp boundary
        if (distanceToCampCenter > parentCamp.campRange)
        {
            Vector3 directionToCamp = (targetPosition - parentCamp.transform.position).normalized;
            targetPosition = parentCamp.transform.position + directionToCamp * parentCamp.campRange;
        }

        a.SetDestination(targetPosition);
        SetBoolsOff();
        n.SetBool("run", true);
    }

    private void ClearAllAlertedMinions()
    {
        for (int i = 0; i < alertedMinions.Length; i++)
        {
            if (alertedMinions[i] != null)
            {
                alertedMinions[i].UnsetAlert();
                alertedMinions[i] = null;
            }
        }
        alertedCount = 0;
        alert = false;
    }

    private void HandleHealthAndDebugInputs()
    {
        if (GetHealth() <= 0f && !isDying)
        {
            GetComponent<AudioSource>().Play();
            SetDying();
            SetBoolsOff();
            StartCoroutine(Die());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetHealth(0f);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetStunned();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack(0);
        }
    }

    private bool TryAddToAlertedMinions()
    {
        // Check if already in array
        for (int i = 0; i < alertedMinions.Length; i++)
        {
            if (alertedMinions[i] == this) return true;
        }

        // Look for empty slot
        if (alertedCount < 5)
        {
            for (int i = 0; i < alertedMinions.Length; i++)
            {
                if (alertedMinions[i] == null)
                {
                    alertedMinions[i] = this;
                    alertedCount++;
                    return true;
                }
            }
        }
        return false;
    }

    private void RemoveFromAlertedMinions()
    {
        for (int i = 0; i < alertedMinions.Length; i++)
        {
            if (alertedMinions[i] == this)
            {
                alertedMinions[i] = null;
                alertedCount--;
                break;
            }
        }
    }

    private void OnDestroy()
    {
        // Handle removal from alerted array when destroyed
        if (alert)
        {
            RemoveAndReplaceMinion();
        }
    }

    private void RemoveAndReplaceMinion()
    {
        // First remove this minion from the array
        RemoveFromAlertedMinions();

        // Try to find a replacement if we're not at max capacity
        if (alertedCount < alertedMinions.Length)
        {
            // Find all minions in the scene
            MinionController[] allMinions = FindObjectsOfType<MinionController>();

            // Try to alert a non-alerted minion that's in camp range
            foreach (MinionController minion in allMinions)
            {
                if (minion != null && !minion.alert && minion.IsPlayerInCampRange())
                {
                    minion.SetAlert();
                    minion.TryAddToAlertedMinions();
                    Debug.Log($"Replaced destroyed minion with {minion.gameObject.name}");
                    break;
                }
            }
        }
    }

    public void SetDying()
    {
        isDying = true;
    }

    public void SetAlert()
    {
        alert = true;
    }

    public void UnsetAlert()
    {
        alert = false;
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetBoolsOff()
    {
        n.SetBool("run", false);
        n.SetBool("walk", false);
        n.SetBool("idle", false);
    }

    public float Attack(float wandererHealth)
    {
        if (!cooldown)
        {
            cooldown = true;
            isAttacking = true;
            n.SetTrigger("attack");
            StartCoroutine(Reset());
            return wandererHealth - 5;
        }
        SetBoolsOff();
        n.SetBool("idle", true);
        return wandererHealth;
    }

    public override void ApproachPlayer()
    {
        a.SetDestination(playerObject.position);
        Debug.Log(a.destination);
        SetBoolsOff();
        n.SetBool("run", true);
    }

    public override void TakeDamage()
    {
        SetBoolsOff();
        n.SetTrigger("damage");
        health -= 5;
        //StartCoroutine(Reset());
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public override void TakeDamage(int damage)
    {
        SetBoolsOff();
        n.SetTrigger("damage");
        health-=damage;
        //StartCoroutine(Reset());
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public override void GetStunned()
    {
        n.SetTrigger("stunned");
        StartCoroutine(Reset());
    }

    public override IEnumerator Die()
    {
        n.SetTrigger("die");
        if (alert)
        {
            RemoveFromAlertedMinions();
        }
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }

    public override IEnumerator Reset()
    {
        SetBoolsOff();
        n.SetBool("idle", true);
        yield return new WaitForSeconds(2f);
        cooldown = false;
        isAttacking = false;
    }
}
