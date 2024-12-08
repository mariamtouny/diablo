using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : MonoBehaviour
{

    public List<ability> abilities;
    private float lastAbilityUsedTime = 0;
    private Renderer renderer;
    Animator animator;
    PlayerLeveling playerLeveling;

    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
        playerLeveling = GetComponent<PlayerLeveling>();
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

        if (Time.time < lastAbilityUsedTime + ability.abilityCooldown)
        {
            Debug.Log($"{abilityName} is on cooldown!");
            return;
        }

        lastAbilityUsedTime = Time.time;

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
        
    }
}
