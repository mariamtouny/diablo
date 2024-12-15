using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characters;
    public Sprite[] characterSprites;
    public Image displayImage;
    public int selectedCharacter = 0;
    public TextMeshProUGUI characterNameText;
    public string[] characterNames = { "Barbarian", "Sorcerer", "   Rogue" };

    private void Start()
    {
        updateCharacterDisplay();
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
        updateCharacterDisplay();
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length ;
        }
        characters[selectedCharacter].SetActive(true);
        updateCharacterDisplay();
    }

    private void updateCharacterDisplay()
    {
        displayImage.sprite = characterSprites[selectedCharacter];
        characterNameText.text = characterNames[selectedCharacter];
    }
    public void StartGame()
    {
       PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(1);
    }
}

