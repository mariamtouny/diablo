using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MinionController : Enemy
{
    public Transform player;
    public float speed = 1;
    private new float health = 20f;
    private bool isDying = false;
    private Vector3 startPosition;
    NavMeshAgent a;
    Animator n;

    public override void Start()
    {
        a = GetComponent<NavMeshAgent>();
        n = GetComponent<Animator>();
        startPosition = transform.position;
        a.speed = speed * Time.deltaTime * 100;
        a.isStopped = false;
    }

    public override void Update()
    {
        if (!alert)
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
                n.SetBool("walk", true);
                a.isStopped = false;
                a.updateRotation = true;
            }
        }
        else
        {
            a.updateRotation = true;
            a.isStopped = false;
            ApproachPlayer();
            if (!a.pathPending && a.remainingDistance < 2f && a.remainingDistance > 0f && a.velocity.sqrMagnitude == 0f)
            {
                SetBoolsOff();
                Attack(100);
            }
        }
        if (GetHealth() <= 0f && !isDying)
        {
            GetComponent<AudioSource>().Play();
            SetDying();
            SetBoolsOff();
            n.SetTrigger("die");
            Invoke(nameof(Die), 1f);
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
        n.SetTrigger("attack");
        StartCoroutine(Reset());
        return wandererHealth - 5;
    }

    public override void ApproachPlayer()
    {
        a.SetDestination(player.position);
        SetBoolsOff();
        n.SetBool("run", true);
    }

    public override void TakeDamage()
    {
        n.SetTrigger("damage");
        SetHealth(GetHealth() - 5f);
        StartCoroutine(Reset());
    }

    public override void GetStunned()
    {
        n.SetTrigger("stunned");
        StartCoroutine(Reset());
    }

    public override IEnumerator Die()
    {
        Destroy(gameObject);
        // add 10 to the wanderer xp
        yield return null;
    }

    public override IEnumerator Reset()
    {
        yield return new WaitForSeconds(5f);
        n.SetTrigger("break");
    }
}
