using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characters;
    public Sprite[] characterSprites;
    public Image displayImage;
    public int selectedCharacter = 0;

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
    }
    public void StartGame()
    {
       PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(1);
    }
}

