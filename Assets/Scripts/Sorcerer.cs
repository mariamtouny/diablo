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
    public GameObject clonePrefab;
    public GameObject explosionPrefab;


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
        StartCoroutine(CloneRoutine());
    }

    private IEnumerator CloneRoutine()
    {
        Debug.Log("Select a point to spawn the clone...");

        // Wait for user to select a position
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 spawnPosition = hit.point;
            GameObject clone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);
            NavMeshAgent cloneAgent = clone.GetComponent<NavMeshAgent>();

            Renderer cloneRenderer = clone.GetComponent<Renderer>();
            if (cloneRenderer != null)
            {
                cloneRenderer.material.color = new Color(1.5f, 0.5f, 0.5f); 
            }

            Debug.Log("modifying");

            string originalTag = gameObject.tag;
            gameObject.tag = "PlayerTemporary";

            //Sorcerer cloneSorcerer = clone.GetComponent<Sorcerer>();
            //if (cloneSorcerer != null)
            //{
            //    cloneSorcerer.enabled = false;
            //}

            Debug.Log("Clone spawned at " + spawnPosition);

            // Optional: Make the clone follow a specific target or wander
            //StartCoroutine(CloneBehaviorRoutine(clone, cloneAgent));

            StartCoroutine(DestroyCloneAfterDuration(clone, 5f, originalTag));
        }
        else
        {
            Debug.Log("No valid point selected for the clone!");
        }
    }
    private IEnumerator DestroyCloneAfterDuration(GameObject clone, float duration, string originalTag)
    {
        yield return new WaitForSeconds(duration);

        if (clone != null)
        {
            Debug.Log("Clone exploded!");
            Explode(clone.transform.position, 5f);
            Destroy(clone);
        }

        gameObject.tag = originalTag;
    }

    private void Explode(Vector3 position, float radius)
    {
        GameObject explosionEffect = Instantiate(explosionPrefab, position, Quaternion.identity);
        Destroy(explosionEffect, 2f);

        Collider[] hitColliders = Physics.OverlapSphere(position, radius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Demon"))
            {
                Demon demon = hit.GetComponentInParent<Demon>();
                if (demon != null)
                {
                    demon.TakeDamage(10);
                    Debug.Log("Damaged enemy: " + hit.name);
                }
            }
            if (hit.CompareTag("Minion"))
            {
                MinionController minion = hit.GetComponent<MinionController>();
                minion.TakeDamage(10);
                Debug.Log("Damaged minion: " + hit.name);
            }
        }
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
            fireCircle.transform.localScale = new Vector3(5f * 2, 1f, 5f * 2);
            Destroy(fireCircle, 5f);
            Debug.Log("Inferno spawned at " + spawnPosition);

            ApplyDamage(spawnPosition, 5f, 10);

            StartCoroutine(ContinuousDamage(spawnPosition, 2f, 2, 5f));

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
        Debug.DrawLine(position, position + Vector3.up * radius, Color.red, 1f);
        Debug.DrawLine(position, position - Vector3.up * radius, Color.red, 1f);
        Vector3 spherePosition = position + Vector3.up * 2; // Adjust for height
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, radius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Demon"))
            {
                Demon demon = hitCollider.gameObject.GetComponentInParent<Demon>();
                demon.TakeDamage(5);
            }
            else if (hitCollider.gameObject.CompareTag("Minion"))
            {
                //Minion minion = collider.gameObject.GetComponent<Minion>();
                //minion.TakeDamage(5);
            }
        }
    }


    private IEnumerator ContinuousDamage(Vector3 position, float radius, int damage, float duration)
    {
        float elapsed = 0f;
        float tickInterval = 1f;
        yield return new WaitForSeconds(1f);

        while (elapsed < duration)
        {
            Debug.Log($"Applying continuous damage at position {position}, elapsed: {elapsed}");
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


        if (Input.GetKeyDown(KeyCode.C))
        {
            PerformClone();
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
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
        if (Physics.Raycast(ray, out hit, 100))
        {
            //Debug.Log(hit.collider.gameObject);
            GameObject enemy = hit.collider.gameObject;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10.0f);
            foreach (Collider hitCollider in hitColliders)
            {

                if (hitCollider.gameObject == enemy)
                {

                    if (enemy.CompareTag("Demon"))
                    {

                        if (enemy != null)
                        {
                            //FaceTarget(enemy.transform);

                             Vector3 direction = (enemy.transform.position - transform.position).normalized;
                            direction.y = 0; // We only want to rotate around the y-axis

                    
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
                            Vector3 demonhitpoint = enemy.transform.position +  new Vector3(0, 2.25f, 0);
                            StartCoroutine(MoveFireball(fball, demonhitpoint));

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
            //Debug.Log("Overlap detected with: " + collider.gameObject);
            if (collider.gameObject.CompareTag("Demon"))
            {
                Demon demon = collider.gameObject.GetComponentInParent<Demon>();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
