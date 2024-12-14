using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rogue : PlayerLeveling
{
    NavMeshAgent agent;
    private float lastAbilityUsedTime = 0;
    private Renderer renderer;
    Animator animator;
    Camera cam;
    public GameObject arrow;
    private Vector3 scaleChange = new Vector3(-0.6f, -0.6f, -0.6f);

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
            new ability { abilityName = "Arrow", abilityType = ability.AbilityType.Basic, activation = ability.Activation.SelectEnemy, abilityDamage = 5, abilityCooldown = 1 },
            new ability { abilityName = "Smoke Bomb", abilityType = ability.AbilityType.Defensive, activation = ability.Activation.Instant, abilityDamage = 0, abilityCooldown = 10 },
            new ability { abilityName = "Dash", abilityType = ability.AbilityType.WildCard, activation = ability.Activation.SelectPosition, abilityDamage = 0, abilityCooldown = 5 },
            new ability { abilityName = "Shower of Arrows", abilityType = ability.AbilityType.Ultimate, activation = ability.Activation.SelectPosition, abilityDamage = 10, abilityCooldown = 10 }
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
            case "Arrow":
                PerformArrow();
                break;
            case "Smoke Bomb":
                PerformSmokeBomb();
                break;
            case "Dash":
                PerformDash();
                break;
            case "Shower Of Arrows":
                PerformShowerOfArrows();
                break;
            default:
                Debug.Log($"{abilityName} does not exist!");
                break;
        }
    }
    private void PerformArrow()
    {
        Debug.Log("Arrow!");
        StartCoroutine(Arrow());

    }
    private IEnumerator Arrow()
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

                            arrow.SetActive(true);
                            animator.SetTrigger("Arrow");

                            yield return new WaitForSeconds(2f);
                            Vector3 position = transform.position;
                            GameObject fball = Instantiate(arrow, new Vector3(position.x - 0.122494f, position.y + 1.13f, position.z + 0.9492f), Quaternion.identity);
                            fball.transform.localScale += scaleChange;
                            StartCoroutine(MoveArrow(fball, enemy.transform.position));

                            arrow.SetActive(false);
                            Debug.Log("Mouse clicked!");

                        }
                    }
                }
            }


        }

    }

    private IEnumerator MoveArrow(GameObject Arrow, Vector3 targetPosition)
    {
        float speed = 5f; // Adjust the speed of the fireball
        float journeyLength = Vector3.Distance(Arrow.transform.position, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(Arrow.transform.position, targetPosition) > 0.1f)
        {
            // Calculate how far along the journey we are
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            // Move the fireball towards the target
            Arrow.transform.position = Vector3.Lerp(Arrow.transform.position, targetPosition, fractionOfJourney);
            // Wait for the next frame
            yield return null;
        }

        Collider[] colliders = Physics.OverlapSphere(Arrow.transform.position, 1f); // Radius 1

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
        Destroy(Arrow);
    }
    private void PerformSmokeBomb()
    {
        Debug.Log("Smoke");
    }

    private void PerformDash()
    {
        Debug.Log("Dashing!");
    }

    private void PerformShowerOfArrows()
    {
        Debug.Log("Shower");
    }
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Q))
        {
            UseAbility("Arrow");
        }
    }
}
