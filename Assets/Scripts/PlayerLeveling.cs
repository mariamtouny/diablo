using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class PlayerLeveling : MonoBehaviour
{
    public enum CharacterClass
    {
        Barbarian,
        Sorcerer,
        Rogue
    }
    public int currentHealth;
    public int maxHealth = 100;
    public int healingPotions = 0;
    private int maxHealingPotions = 3;

    public int currentXP = 0;
    public int currentLevel = 1;
    private int maxLevel = 4;
    private int xpToNextLevel;
    private int abilityPoints = 0;

    public List<ability> abilities;
    public CharacterClass characterClass;



    void Start()
    {
        currentHealth = maxHealth;
        xpToNextLevel = 100 * currentLevel;
        //set character class
        InitializeAbilities();
    }

    private void InitializeAbilities()
    {
        abilities = new List<ability>();
        switch (characterClass)
        {
            case CharacterClass.Barbarian:
                abilities.Add(new ability("Bash", ability.AbilityType.Basic, ability.Activation.SelectEnemy, 5, 1));
                abilities.Add(new ability("Shield Wall", ability.AbilityType.Defensive, ability.Activation.Instant, 0, 10));
                abilities.Add(new ability("Battle Roar", ability.AbilityType.WildCard, ability.Activation.Instant, 10, 5));
                abilities.Add(new ability("Charge", ability.AbilityType.Ultimate, ability.Activation.SelectPosition, 20, 10));
                break;

            case CharacterClass.Sorcerer:
                abilities.Add(new ability("Fireball", ability.AbilityType.Basic, ability.Activation.SelectEnemy, 10, 2));
                abilities.Add(new ability("Arcane Shield", ability.AbilityType.Defensive, ability.Activation.Instant, 0, 15));
                abilities.Add(new ability("Teleport", ability.AbilityType.WildCard, ability.Activation.SelectPosition, 0, 8));
                abilities.Add(new ability("Meteor Strike", ability.AbilityType.Ultimate, ability.Activation.SelectPosition, 50, 20));
                break;

            case CharacterClass.Rogue:
                abilities.Add(new ability("Arrow Shot", ability.AbilityType.Basic, ability.Activation.SelectEnemy, 7, 1));
                abilities.Add(new ability("Evasion", ability.AbilityType.Defensive, ability.Activation.Instant, 0, 10));
                abilities.Add(new ability("Explosive Arrow", ability.AbilityType.WildCard, ability.Activation.SelectEnemy, 15, 6));
                abilities.Add(new ability("Rain of Arrows", ability.AbilityType.Ultimate, ability.Activation.SelectPosition, 25, 15));
                break;
        }

        // Unlock the basic ability
        abilities[0].unlocked = true;
    }
    void Update()
    {

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth += 20;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentHealth -= 20;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            abilityPoints += 1;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseHealingPotion();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(100);
        }
    }

    private void UseHealingPotion()
    {
        if (healingPotions > 0 && currentHealth < maxHealth)
        {
            healingPotions--;
            int healingAmount = Mathf.FloorToInt(maxHealth * 0.5f);
            currentHealth = Mathf.Min(currentHealth + healingAmount, maxHealth);
        }
    }

    public void AddHealingPotion()
    {
        if (healingPotions < maxHealingPotions)
        {
            healingPotions++;
        }
    }

    public void GainXP(int xpAmount)
    {
        currentXP += xpAmount;
        while (currentXP >= xpToNextLevel && currentLevel < maxLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        xpToNextLevel = 100 * currentLevel;

        maxHealth += 100;
        currentHealth = maxHealth;

        abilityPoints++;

        //unlock new abilities
    }

    public void Die()
    {
       //gameOver
    }
}
