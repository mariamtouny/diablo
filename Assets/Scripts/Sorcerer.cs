using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Sorcerer : PlayerLeveling
{
    NavMeshAgent agent;
    private float lastAbilityUsedTime = 0;
    private Renderer renderer;
    Animator animator;
    Camera cam;
    GameObject fireball;

    void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
    }

    private void Awake()
    {
        abilities = new List<ability>
        {
            new ability { abilityName = "Fireball", abilityType = ability.AbilityType.Basic, activation = ability.Activation.SelectEnemy, abilityDamage = 5, abilityCooldown = 1 },
            new ability { abilityName = "Teleport", abilityType = ability.AbilityType.Defensive, activation = ability.Activation.SelectPosition, abilityDamage = 0, abilityCooldown = 10 },
            new ability { abilityName = "Clone", abilityType = ability.AbilityType.WildCard, activation = ability.Activation.SelectPosition, abilityDamage = 10, abilityCooldown = 10 },
            new ability { abilityName = "Inferno", abilityType = ability.AbilityType.Ultimate, activation = ability.Activation.SelectPosition, abilityDamage = 10, abilityCooldown = 15 }
        };

        // Unlock the basic ability by default
        abilities[0].unlocked = true;
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
            case "Fireball": 
                PerformFireball();
                break;
            case "Teleport":
                PerformTeleport();
                break;
            case "Clone":
                PerformClone();
                break;
            case "Inferno":
                PerformInferno();
                break;
            default:
                Debug.Log($"{abilityName} does not exist!");
                break;
        }
    }
    private void PerformFireball()
    {
        Debug.Log("Fireball!");
    }
    private void PerformTeleport()
    {
        Debug.Log("Teleport!");
        StartCoroutine(Teleport());
    }

    private void PerformClone()
    {
        Debug.Log("Clone!");
    }

    private void PerformInferno()
    {
        Debug.Log("Inferno!");
    }
    void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PerformTeleport();
        }
        
    }

    private IEnumerator Teleport()
    {
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100))
        {

            transform.position = hit.point;
            agent.SetDestination(hit.point);
            FaceTarget(hit.transform);
        }

    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
    }
}
