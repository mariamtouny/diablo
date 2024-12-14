using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CampController : MonoBehaviour
{
    public MinionController minion;

    void Start()
    {
        minion = FindObjectOfType(typeof(MinionController)) as MinionController;
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            Debug.Log("CampControllerTriggered");
            minion.SetAlert();
        }
    }

    public void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            Debug.Log("CampControllerUntriggered");
            minion.UnsetAlert();
        }
    }
}
