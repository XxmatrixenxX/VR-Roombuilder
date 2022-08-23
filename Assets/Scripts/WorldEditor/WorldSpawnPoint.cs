using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject body;

    private void Start()
    {
        StartOfMode();
    }

    private void Awake()
    {
        StartOfMode();
    }

    public void StartOfMode()
    {
        if (FindObjectOfType<UIRoomChooser>() != null)
        {
            EnableBody();
        }
        else
        {
            DisableBody();
        }
    }
    
    public void EnableBody()
    {
        body.SetActive(true);
    }
    
    public void DisableBody()
    {
        body.SetActive(false);
    }
}
