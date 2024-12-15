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
    private bool cooldown = false;
    private bool isAttacking = false;
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Update()
    {
        if (isDying) return;
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
            if (!isAttacking)
            {
                a.updateRotation = true;
                a.isStopped = false;
                ApproachPlayer();
                if (!a.pathPending && a.remainingDistance < 1.7f)
                {
                    SetBoolsOff();
                    Attack(0);
                }
            }
        }
        if (GetHealth() <= 0f && !isDying)
        {
            GetComponent<AudioSource>().Play();
            SetDying();
            SetBoolsOff();
            n.SetTrigger("die");
            Invoke(nameof(Death), 1f);
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

    public void Death()
    {
        Destroy(gameObject);
    }

    public override IEnumerator Die()
    {
        Debug.Log("enemy died");
        yield return null;
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
