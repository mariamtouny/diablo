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

    }
}
