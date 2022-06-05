using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject ItemSelectCanvas;
    [SerializeField] private GameObject ModeSelectCanvas;

    private void Awake()
    {
        ActivateModeSelect();
    }

    public void ActivateModeSelect()
    {
        ItemSelectCanvas.SetActive(false);
        ModeSelectCanvas.SetActive(true);
    }

    public void ActivateItemSelectCanvas()
    {
        ItemSelectCanvas.SetActive(true);
        ModeSelectCanvas.SetActive(false);
    }

    public void CloseMenus()
    {
        ItemSelectCanvas.SetActive(false);
        ModeSelectCanvas.SetActive(false);
    }

    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ActivateModeSelect();
        }
    }
}
