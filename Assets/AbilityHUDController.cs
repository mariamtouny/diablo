using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class AbilityHUDController : MonoBehaviour
{
    // UI references found by tags
    private TextMeshProUGUI basicCooldownText;
    private TextMeshProUGUI defensiveCooldownText;
    private TextMeshProUGUI wildCardCooldownText;
    private TextMeshProUGUI ultimateCooldownText;

    private Button basicAbilityButton;
    private Button defensiveAbilityButton;
    private Button wildCardAbilityButton;
    private Button ultimateAbilityButton;

    private void Start()
    {
        // Find cooldown texts by their tags
        basicCooldownText = GameObject.FindGameObjectWithTag("BasicCooldown").GetComponent<TextMeshProUGUI>();
        defensiveCooldownText = GameObject.FindGameObjectWithTag("DefensiveCooldown").GetComponent<TextMeshProUGUI>();
        wildCardCooldownText = GameObject.FindGameObjectWithTag("WildCardCooldown").GetComponent<TextMeshProUGUI>();
        ultimateCooldownText = GameObject.FindGameObjectWithTag("UltimateCooldown").GetComponent<TextMeshProUGUI>();

        // Find buttons by their tags
        basicAbilityButton = GameObject.FindGameObjectWithTag("BasicButton").GetComponent<Button>();
        defensiveAbilityButton = GameObject.FindGameObjectWithTag("DefensiveButton").GetComponent<Button>();
        wildCardAbilityButton = GameObject.FindGameObjectWithTag("WildCardButton").GetComponent<Button>();
        ultimateAbilityButton = GameObject.FindGameObjectWithTag("UltimateButton").GetComponent<Button>();

        // Initialize cooldown texts to empty and buttons to disabled
        InitializeAbility(basicCooldownText, basicAbilityButton);
        InitializeAbility(defensiveCooldownText, defensiveAbilityButton);
        InitializeAbility(wildCardCooldownText, wildCardAbilityButton);
        InitializeAbility(ultimateCooldownText, ultimateAbilityButton);
    }

    private void InitializeAbility(TextMeshProUGUI cooldownText, Button abilityButton)
    {
        cooldownText.text = ""; // Clear text initially
        abilityButton.interactable = true; // Ensure buttons are interactable initially
    }

    public void SetAbilityCooldown(string abilityName, float cooldownTime)
    {
        // Determine which ability's cooldown and button to update
        switch (abilityName)
        {
            case "Basic":
                StartCoroutine(CooldownTimer(basicCooldownText, basicAbilityButton, cooldownTime));
                break;

            case "Defensive":
                StartCoroutine(CooldownTimer(defensiveCooldownText, defensiveAbilityButton, cooldownTime));
                break;

            case "WildCard":
                StartCoroutine(CooldownTimer(wildCardCooldownText, wildCardAbilityButton, cooldownTime));
                break;

            case "Ultimate":
                StartCoroutine(CooldownTimer(ultimateCooldownText, ultimateAbilityButton, cooldownTime));
                break;
        }
    }

    private IEnumerator CooldownTimer(TextMeshProUGUI cooldownText, Button abilityButton, float cooldownTime)
    {
        float remainingTime = cooldownTime;

        // Disable the button during the cooldown
        abilityButton.interactable = false;

        while (remainingTime > 0)
        {
            cooldownText.text = $"{Mathf.Ceil(remainingTime)}s"; // Update the cooldown text
            yield return new WaitForSeconds(1f); // Wait for 1 second
            remainingTime -= 1f;
        }

        // When cooldown ends, clear the text and enable the button
        cooldownText.text = "";
        abilityButton.interactable = true;
    }
}
