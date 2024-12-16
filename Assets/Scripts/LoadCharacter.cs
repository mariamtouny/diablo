using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public GameObject BarbarianAbilitiesPanel;
    public GameObject RogueAbilitiesPanel;
    public GameObject SorcererAbilitiesPanel;
    public Transform spawnPoint;
    Camera camera;

    private void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject prefab = characterPrefabs[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        camera = Camera.main;
        CameraController controller = camera.GetComponent<CameraController>();
        controller.target = GameObject.FindGameObjectWithTag("Player").transform;

       if (selectedCharacter == 0) // Barbarian
        {
            BarbarianAbilitiesPanel.SetActive(true);
            Destroy(SorcererAbilitiesPanel);
            Destroy(RogueAbilitiesPanel);
        }
       if (selectedCharacter == 1) // Sorcerer
        {
            SorcererAbilitiesPanel.SetActive(true);
            Destroy(BarbarianAbilitiesPanel);
            Destroy(RogueAbilitiesPanel);
        }
       if(selectedCharacter == 2) // Rogue
        {
            RogueAbilitiesPanel.SetActive(true);
            Destroy(SorcererAbilitiesPanel);
            Destroy(BarbarianAbilitiesPanel);
        }
    }
}
