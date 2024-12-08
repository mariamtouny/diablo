using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbarian : PlayerLeveling
{
    private float lastAbilityUsedTime = 0;
    private bool shieldActive = false;
    private Renderer renderer;
    Animator animator;

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

        if (Time.time < lastAbilityUsedTime + ability.abilityCooldown)
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
    }
    private void PerformBash()
    {
        if (abilities[0].unlocked == true && !abilities[0].IsOnCoolDown())
        {
            //perform bash  
            Debug.Log("Barbarian performs Bash!");

        }
        //ability unavailable       
    }

    private void PerformShield()
    {
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
        yield return new WaitForSeconds(5f);
        shieldActive = false;
        Debug.Log("Shield expired!");
    }

    private void PerformIronMaelstrom()
    {
        if (abilities[2].unlocked == true && !abilities[2].IsOnCoolDown())
        {
            Debug.Log("Barbarian unleashes Iron Maelstrom!");
            // Add AOE damage logic here
        }
        //ability unavailable
    }

    private void PerformCharge()
    {
        if (abilities[3].unlocked == true && !abilities[3].IsOnCoolDown())
        {
            Debug.Log("Barbarian charges forward!");
            // Add movement and damage logic here
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
