using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject PlayPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject CreditsPanel;

    [Header("Sliders")]
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider EffectsSlider;

    private void Start()
    {
        PlayPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);

        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        EffectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
    }

    public void OpenPlayPanel()
    {
        PlayPanel.SetActive(true);    
    }

    public void OpenOptionsPanel()
    {
        OptionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("CharacterSelection"); 
    }

    public void ClosePlayPanel()
    {
        PlayPanel.SetActive(false); 
    }

    public void OpenCreditsPanel()
    {
        CreditsPanel.SetActive(true);
    }

    public void CloseOptionsPanel()
    {
        OptionsPanel.SetActive(false);
    }

    public void CloseCreditsPanel()
    {
        CreditsPanel.SetActive(false);
    }
    public Text musicVolumeText;
    public Text effectsVolumeText;
    public void SetMusicVolume(float value)
    {
        Debug.Log($"Music Volume: {value}");
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicVolumeText.text = $"{Mathf.RoundToInt(value * 100)}%";
        AudioListener.volume = value; //globally to all audio
    }

    public void SetEffectsVolume(float value)
    {
        Debug.Log($"Effects Volume: {value}");
        PlayerPrefs.SetFloat("EffectsVolume", value);
        effectsVolumeText.text = $"{Mathf.RoundToInt(value * 100)}%";
        // effectsAudioSource.volume = value;
    }

}
