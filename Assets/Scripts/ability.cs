using UnityEngine;

public class ability : MonoBehaviour
{
    public enum AbilityType
    {
        Basic,
        Defensive,
        WildCard,
        Ultimate
    }
    public enum Activation
    {
        Instant,
        SelectEnemy,
        SelectPosition
    }
    public string abilityName;
    public int abilityDamage;
    public int abilityCooldown;
    public AbilityType abilityType;
    public Activation activation;
    public bool unlocked;
    public int coolDownTimer = 0;

    public ability(string name, AbilityType type, Activation act, int damage, int cooldown)
    {
        this.abilityName = name;
        this.abilityDamage = damage;
        this.abilityCooldown = cooldown;
        this.abilityType = type;
        this.activation = act;
        this.unlocked = false;
    }

    public bool IsOnCoolDown()
    {
        return coolDownTimer > 0;
    }

    private void Update()
    {
        if (coolDownTimer > 0)
        {
            coolDownTimer -= (int)Time.deltaTime;
        }
    }
}
