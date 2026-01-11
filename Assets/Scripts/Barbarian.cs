using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public class Barbarian : PlayerLeveling
{
    NavMeshAgent agent;
    private bool shieldActive = false;
    private Renderer renderer;
    Animator animator;
    Camera cam;
    private float aoeRadius = 1.5f;
    //private bool charge = false;

    private void Awake()
    {
        abilities = new List<ability>
        {
            new ability { abilityName = "Bash", abilityType = ability.AbilityType.Basic, activation = ability.Activation.SelectEnemy, abilityDamage = 5, abilityCooldown = 1 },
            new ability { abilityName = "Shield", abilityType = ability.AbilityType.Defensive, activation = ability.Activation.Instant, abilityDamage = 0, abilityCooldown = 10 },
            new ability { abilityName = "Iron Maelstrom", abilityType = ability.AbilityType.WildCard, activation = ability.Activation.Instant, abilityDamage = 10, abilityCooldown = 5 },
            new ability { abilityName = "Charge", abilityType = ability.AbilityType.Ultimate, activation = ability.Activation.SelectPosition, abilityDamage = 20, abilityCooldown = 10 }
        };

        // Unlock the basic ability by default
        abilities[0].unlocked = true;
    }

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        base.Start();
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
    }

    public void UseAbility(string abilityName)
    {
        var ability = abilities.Find(a => a.abilityName == abilityName && a.unlocked);
        if (ability == null)
        {
            Debug.Log($"{abilityName} is not unlocked or does not exist!");
            return;
        }

        if (ability.IsOnCoolDown())
        {
            Debug.Log($"{abilityName} is on cooldown!");
            return;
        }


        switch (abilityName)
        {
            case "Bash":
                PerformBash();
                break;
            case "Shield":
                PerformShield();
                break;
            case "Iron Maelstrom":
                PerformIronMaelstrom();
                break;
            case "Charge":
                PerformCharge();
                break;
        }
    }
    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PerformShield();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            PerformIronMaelstrom();
        }
    
        if (Input.GetKeyDown(KeyCode.B))
        {
            PerformBash();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PerformCharge();
        }


    }
    private void PerformBash()
    {
        if (abilities[0].unlocked == true && !abilities[0].IsOnCoolDown())
        {
            StartCoroutine(Bash());
            Debug.Log("Barbarian performs Bash!");

        }

        //ability unavailable       
    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
    }

    private void PerformShield()
    {
        //Debug.Log("Shield cooldown: " + abilities[1].coolDownTimer);
        Debug.Log(abilities[1].unlocked);
        if (abilities[1].unlocked == true && !abilities[1].IsOnCoolDown())
        {
            Debug.Log("Barbarian raises Shield!");
            shieldActive = true;
            StartCoroutine(ShieldDuration());
        }
        //ability unavailable
    }

    private IEnumerator ShieldDuration()
    {
        // Example shield active time logic
        Debug.Log("Shield active!");
        yield return new WaitForSeconds(3f);
        shieldActive = false;
        Debug.Log("Shield expired!");
    }

    private IEnumerator Bash()
    {
        //Vector3 mousePosition = transform.position;
        while (!Input.GetMouseButtonDown(1))
        {
            //mousePosition = Input.mousePosition;
            yield return null; 
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            GameObject enemy = hit.collider.gameObject;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject==enemy)
                {
                    if (enemy.CompareTag("Demon"))
                    {
                        if (enemy != null)
                        {
                            animator.SetTrigger("Bash");
                            Debug.Log("Mouse clicked!");
                            agent.SetDestination(enemy.transform.position);
                            FaceTarget(enemy.transform);
                            Demon demon = hitCollider.gameObject.GetComponentInParent<Demon>();
                            demon.TakeDamage(5);
                        }
                    }
                }
            }
            
            
        }
        
       
    }

    private void PerformIronMaelstrom()
    {
        if (abilities[2].unlocked == true && !abilities[2].IsOnCoolDown())
        {
            Debug.Log("Barbarian unleashes Iron Maelstrom!");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);
            animator.SetTrigger("whirl");
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Demon"))
                {
                    Demon demon = hitCollider.gameObject.GetComponentInParent<Demon>();
                    demon.TakeDamage(10);
                    Debug.Log("Iron Maelstrom hits enemy!");
                }
            }
        }
        //ability unavailable
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0 , 0, 0.3f);
        Gizmos.DrawSphere(transform.position, aoeRadius);
    }

    private IEnumerator Charge()
    {
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        

        if (Physics.Raycast(ray, out hit, 100))
        {
            //charge = true;
            agent.SetDestination(hit.point);

            FaceTarget(hit.transform);

            animator.SetTrigger("Run");
            StartCoroutine(UnCharge());
            Debug.Log("runs");

            agent.updateRotation = true;
            
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collision");
    //    if (charge)
    //    {
    //        if (collision.gameObject.CompareTag("Demon"))
    //        {
    //            Demon demon = collision.gameObject.GetComponentInParent<Demon>();
    //            StartCoroutine(demon.Die());
    //            GainXP(30);
    //        }
    //    }
    //}

    public IEnumerator UnCharge()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Run");
        //charge = false;
    }

    private void PerformCharge()
    {
        if (abilities[3].unlocked == true && !abilities[3].IsOnCoolDown())
        {
            Debug.Log("Barbarian charges forward!");
            StartCoroutine(Charge());
        }
        //ability unavailable
    }

    public override void TakeDamage(int damage)
    {
        if (shieldActive)
        {
            Debug.Log("Shield absorbed damage!");
            return;
        }
        base.TakeDamage(damage);
    }
}
