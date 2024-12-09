using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public class Barbarian : PlayerLeveling
{
    NavMeshAgent agent;
    private float lastAbilityUsedTime = 0;
    private bool shieldActive = false;
    private Renderer renderer;
    Animator animator;
    Camera cam;
    private float aoeRadius = 1.5f;

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

        lastAbilityUsedTime = Time.time;

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
                    if (enemy.CompareTag("Enemy"))
                    {
                        if (enemy != null)
                        {
                            animator.SetTrigger("Bash");
                            Debug.Log("Mouse clicked!");
                            agent.SetDestination(enemy.transform.position);
                            FaceTarget(enemy.transform);
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
                if (hitCollider.CompareTag("Enemy"))
                {
                    // Add damage logic here
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
        Debug.Log("Charging 1.0");
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.Log("Mouse Clicked");
        

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("found hit");

            agent.SetDestination(hit.point);
            Debug.Log("set destination");

            FaceTarget(hit.transform);
            Debug.Log("face target");

            animator.SetBool("isRunning", true);
            Debug.Log("runs");

            agent.updateRotation = true;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);
            Debug.Log("Mouse Clicked");

            foreach (Collider hitCollider in hitColliders)
            {
                //enemies taking damage here
                     
            }
        }

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
