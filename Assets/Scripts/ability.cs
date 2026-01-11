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
    public bool unlocked = false;
    public int coolDownTimer = 0;
    public bool IsOnCoolDown()
    {
        return coolDownTimer > 0;
    }

    private void Update()
    {
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Mathf.FloorToInt(Time.deltaTime);
            if (coolDownTimer < 0) coolDownTimer = 0;
        }
    }
}