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
        private Vector3 scaleChange = new Vector3(1f, 1f, 1f);
        public GameObject smoke;

        void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            cam = Camera.main;
            animator = GetComponent<Animator>();
            renderer = GetComponent<Renderer>();
    
            abilities = new List<ability>
            {
                new ability { abilityName = "Arrow", abilityType = ability.AbilityType.Basic, activation = ability.Activation.SelectEnemy, abilityDamage = 5, abilityCooldown = 1 },
                new ability { abilityName = "Smoke Bomb", abilityType = ability.AbilityType.Defensive, activation = ability.Activation.Instant, abilityDamage = 0, abilityCooldown = 10 },
                new ability { abilityName = "Dash", abilityType = ability.AbilityType.WildCard, activation = ability.Activation.SelectPosition, abilityDamage = 0, abilityCooldown = 5 },
                new ability { abilityName = "Shower of Arrows", abilityType = ability.AbilityType.Ultimate, activation = ability.Activation.SelectPosition, abilityDamage = 10, abilityCooldown = 10 }
            };

            // Unlock the basic ability by default
            abilities[0].unlocked = true;
            arrow.SetActive(false);
        }

        public void UseAbility(string abilityName)
        {
            ability ability = abilities.Find(a => a.abilityName == abilityName);
            //Debug.Log(ability + " " + ability.abilityName + " " + ability.unlocked);
            if (!ability.unlocked)
            {
                Debug.Log($"{abilityName} is not unlocked!");
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
                case "Shower of Arrows":
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
                //Debug.Log(hit.collider.gameObject);
                GameObject enemy = hit.collider.gameObject;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5.0f);
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
                                //direction.y = 0; // We only want to rotate around the y-axis


                                // Calculate the target rotation that looks towards the enemy
                                Quaternion targetRotation = Quaternion.LookRotation(direction);

                                // Rotate 60 degrees around the Y-axis
                                Quaternion additionalRotation = Quaternion.Euler(0, 60, 0);
                                Quaternion finalRotation = targetRotation * additionalRotation;

                                // Smoothly interpolate to the final target rotation
                                //transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 15f);

                                animator.SetTrigger("Arrow");
                                yield return new WaitForSeconds(1f);
                                arrow.SetActive(true);
                                Quaternion newRotation = Quaternion.Euler(90, 0, -15.58f);
                                yield return new WaitForSeconds(1f);
                                arrow.transform.rotation = newRotation;
                                yield return new WaitForSeconds(1f);
                                Vector3 position = transform.position;
                                GameObject fball = Instantiate(arrow, new Vector3(position.x - 0.122494f, position.y + 1.13f, position.z ), Quaternion.identity);
                                fball.transform.localScale += scaleChange;
                                fball.transform.rotation = Quaternion.Euler(0, 90, 0);
                                Vector3 demonhitpoint = enemy.transform.position + new Vector3(0, 2.25f, 0);
                                StartCoroutine(MoveArrow(fball, demonhitpoint));
                                arrow.SetActive(false);
                                Quaternion oldRotation = Quaternion.Euler(0, 0, -15.58f);
                                arrow.transform.rotation = oldRotation;
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
            Destroy(Arrow);
        }
        private void PerformSmokeBomb()
        {
            if (abilities[1].unlocked && !abilities[1].IsOnCoolDown())
            {
                Debug.Log("She like the way that i activate!");
                SmokeBomb();
            }
        }

        private void SmokeBomb()
        {

                GameObject smokeCircle = Instantiate(smoke, transform.position, Quaternion.identity);
                smokeCircle.SetActive(true);
                smokeCircle.transform.localScale = new Vector3(1.5f * 2, 0.1f, 1.5f * 2);
                Debug.Log("Smoke Bomb detonated");

                ApplyStun(transform.position, 5f, 10);


                Destroy(smokeCircle, 5f);

                Debug.Log("Smoke finished!");
        }
        private void ApplyStun(Vector3 position, float radius, int damage)
        {
            Debug.DrawLine(position, position + Vector3.up * radius, Color.red, 1f);
            Debug.DrawLine(position, position - Vector3.up * radius, Color.red, 1f);
            Vector3 spherePosition = position + Vector3.up * 2;
            Collider[] hitColliders = Physics.OverlapSphere(spherePosition, radius);

            foreach (var hitCollider in hitColliders)
            {
            Debug.Log(hitCollider.gameObject);
                if (hitCollider.gameObject.CompareTag("Demon"))
                {

                    Demon demon = hitCollider.gameObject.GetComponentInParent<Demon>();
                    demon.GetStunned();
                }
                else if (hitCollider.gameObject.CompareTag("Minion"))
                {
                    MinionController minion = hitCollider.gameObject.GetComponent   <MinionController>(); 
                    minion.GetStunned();
                }
            }
        }

    private void PerformDash()
    {
        Debug.Log("Dashing!");
    }

    private void PerformShowerOfArrows()
    {
        Debug.Log("Shower");
        StartCoroutine(ShowerRoutine());

    }

    private IEnumerator ShowerRoutine()
    {
        Debug.Log("Select a point for Shower of arrows...");
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 spawnPosition = hit.point;

            
            Debug.Log("Arrows raining at " + spawnPosition);

            ApplyDamage(spawnPosition, 1.5f, 10);
            Debug.Log("Shower of Arrows finished!");
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
                demon.TakeDamage(10);
                agent = hitCollider.gameObject.GetComponentInParent<NavMeshAgent>();
                float speed = agent.speed;
                StartCoroutine(SlowDown(agent, speed, 3f));

            }
            else if (hitCollider.gameObject.CompareTag("Minion"))
            {
                //Minion minion = collider.gameObject.GetComponent<Minion>();
                //minion.TakeDamage(5);
            }
        }
    }

    private IEnumerator SlowDown(NavMeshAgent agent, float speed, float duration)
    {
        agent.speed = speed* 0.25f;
        yield return new WaitForSeconds(duration);
        agent.speed = speed;
    }



    void Update()
    {
        base.Update();
       if (Input.GetKeyDown(KeyCode.Q))
        {
            UseAbility("Arrow");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            UseAbility("Shower of Arrows");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseAbility("Smoke Bomb");
        }
    }
}
