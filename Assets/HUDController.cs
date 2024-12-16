using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI abilityPointsText;
    [SerializeField] private TextMeshProUGUI potionsText;
    [SerializeField] private TextMeshProUGUI runeText;

    [Header("Enemy Stats")]
    [SerializeField] private Slider enemyHealthBar;

    public void UpdateHealth(int current, int max)
    {
        healthBar.value = (float)current / max;
        healthText.text = $"{current} / {max}";
    }

    public void UpdateXP(int current, int max)
    {
        xpBar.value = (float)current / max;
        xpText.text = $"{current} / {max}";
    }

    public void UpdateLevel(int level)
    {
        levelText.text = $"Level {level}";
    }

    public void UpdateAbilityPoints(int points)
    {
        abilityPointsText.text = $"Ability Points: {points}";
    }

    public void UpdatePotions(int count)
    {
        potionsText.text = $"Potions: {count}";
    }

    public void UpdateRunes(int count)
    {
        runeText.text = $"Runes: {count}";
    }

    public void UpdateEnemyHealth(int current, int max)
    {
        enemyHealthBar.value = (float)current / max;
    }
}
