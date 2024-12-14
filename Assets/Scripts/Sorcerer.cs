using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Sorcerer : PlayerLeveling
{
    NavMeshAgent agent;
    private Renderer renderer;
    Animator animator;
    Camera cam;
    bool infernoActive = false;
    public GameObject fireball;
    private Vector3 scaleChange = new Vector3(-0.6f, -0.6f, -0.6f);
    private Vector3 positionChange = new Vector3(0.0f, 0.0f, 5.0f);



    void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
        fireball.SetActive(false);
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
        StartCoroutine(Fireball());

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
        if (abilities[3].unlocked && !abilities[3].IsOnCoolDown())
        {
            Debug.Log("Inferno activated!");
            StartCoroutine(InfernoRoutine());
        }
    }

    private IEnumerator InfernoRoutine()
    {
        Debug.Log("Select a point for Inferno...");
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 spawnPosition = hit.point;

            GameObject fireCircle = Instantiate(fireball, spawnPosition, Quaternion.identity);
            fireCircle.SetActive(true);
            fireCircle.transform.localScale = new Vector3(1.5f * 2, 0.1f, 1.5f * 2);
            Destroy(fireCircle, 5f);
            Debug.Log("Inferno spawned at " + spawnPosition);

            ApplyDamage(spawnPosition, 1.5f, abilities[3].abilityDamage);

            StartCoroutine(ContinuousDamage(spawnPosition, 1.5f, abilities[3].abilityDamage / 2, 5f));

            Destroy(fireCircle, 5f);

            Debug.Log("Inferno finished!");
        }
        else
        {
            Debug.Log("No valid point selected!");
        }
    }
    private void ApplyDamage(Vector3 position, float radius, int damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // damage enemies
                Debug.Log($"Enemy at {hitCollider.transform.position} took {damage} damage!");
            }
        }
    }
    private IEnumerator ContinuousDamage(Vector3 position, float radius, int damage, float duration)
    {
        float elapsed = 0f;
        float tickInterval = 1f;

        while (elapsed < duration)
        {
            ApplyDamage(position, radius, damage);
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            PerformInferno();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformFireball();
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

    private IEnumerator Fireball()
    {
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            GameObject enemy = hit.collider.gameObject;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5.0f);
            foreach (Collider hitCollider in hitColliders)
            {

                if (hitCollider.gameObject == enemy)
                {

                    if (enemy.CompareTag("Enemy"))
                    {

                        if (enemy != null)
                        {
                            //FaceTarget(enemy.transform);

                             Vector3 direction = (enemy.transform.position - transform.position).normalized;
                            //direction.y = 0; // We only want to rotate around the y-axis

                    
                        // Calculate the target rotation that looks towards the enemy
                            Quaternion targetRotation = Quaternion.LookRotation(direction);

                        // Rotate 60 degrees around the Y-axis
                            Quaternion additionalRotation = Quaternion.Euler(0, 60, 0);
                            Quaternion finalRotation = targetRotation * additionalRotation;

                        // Smoothly interpolate to the final target rotation
                            //transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 15f);
                    
                            fireball.SetActive(true);
                            animator.SetTrigger("Fireball");

                            yield return new WaitForSeconds(2f);
                            Vector3 position = transform.position;
                            GameObject fball = Instantiate(fireball, new Vector3(position.x - 0.122494f, position.y + 1.13f, position.z + 0.9492f), Quaternion.identity);
                            fball.transform.localScale += scaleChange;
                            StartCoroutine(MoveFireball(fball, enemy.transform.position));

                            fireball.SetActive(false);
                            Debug.Log("Mouse clicked!");
                            
                        }
                    }
                }
            }


        }

    }

    private IEnumerator MoveFireball(GameObject fball, Vector3 targetPosition)
    {
        float speed = 5f; // Adjust the speed of the fireball
        float journeyLength = Vector3.Distance(fball.transform.position, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(fball.transform.position, targetPosition) > 0.1f)
        {
            // Calculate how far along the journey we are
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            // Move the fireball towards the target
            fball.transform.position = Vector3.Lerp(fball.transform.position, targetPosition, fractionOfJourney);
            // Wait for the next frame
            yield return null;
        }

        Collider[] colliders = Physics.OverlapSphere(fball.transform.position, 1f); // Radius 1

        foreach (Collider collider in colliders)
        {
            Debug.Log("Overlap detected with: " + collider.gameObject.name);
            if (collider.gameObject.CompareTag("Demon"))
            {
                Demon demon = collider.gameObject.GetComponent<Demon>();
                demon.TakeDamage(5);
                GainXP(30);
            }
            else if (collider.gameObject.CompareTag("Minion"))
            {
                //Minion minion = collider.gameObject.GetComponent<Minion>();
                //minion.TakeDamage(5);
                GainXP(10);
            }
        }
        // Optionally, you can destroy the fireball after it has moved
        Destroy(fball);
    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
    }
}
