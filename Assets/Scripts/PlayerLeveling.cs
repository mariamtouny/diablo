using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class PlayerLeveling : MonoBehaviour
{
    public List<ability> abilities;
    public int currentHealth;
    public int maxHealth = 100;
    public int healingPotions = 0;
    public int maxHealingPotions = 3;

    public int currentXP = 0;
    public int currentLevel = 1;
    public int maxLevel = 4;
    public int xpToNextLevel;
    public int abilityPoints = 0;

    public void Start()
    {
        currentHealth = maxHealth;
        xpToNextLevel = 100 * currentLevel;
    }

    public void Update()
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
            Debug.Log("XP Gained");
            GainXP(100);
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Ability Unlocked");
            abilityPoints = 4;
            UnlockAbility(0);
            UnlockAbility(1);
            UnlockAbility(2);
            UnlockAbility(3);
        }
        //UpdateCooldowns();
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

    public void UnlockAbility(int abilityIndex)
    {
        if (abilityPoints > 0 && !abilities[abilityIndex].unlocked)
        {
            abilities[abilityIndex].unlocked = true;
            abilityPoints--;
            Debug.Log("Ability unlocked: " + abilities[abilityIndex].abilityName);
        }
    }

    //public void UpdateCooldowns()
    //{
    //    foreach (var ability in abilities)
    //    {
    //        if (ability.coolDownTimer > 0)
    //        {
    //            ability.coolDownTimer -= Mathf.CeilToInt(Time.deltaTime);
    //            if (ability.coolDownTimer < 0) ability.coolDownTimer = 0;
    //        }
    //    }
    //}


    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

}
