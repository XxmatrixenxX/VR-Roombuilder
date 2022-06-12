using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristMenuHandler : MonoBehaviour
{
    [SerializeField] private UIMenuChooseHandler ItemSelectCanvas;
    [SerializeField] private UIModeChoose ModeSelectCanvas;

    private void Awake()
    {
        ActivateModeSelect();
    }

    public void ActivateModeSelect()
    {
        ItemSelectCanvas.gameObject.SetActive(false);
        ModeSelectCanvas.gameObject.SetActive(true);
    }

    public void ActivateItemSelectCanvas()
    {
        ItemSelectCanvas.gameObject.SetActive(true);
        ItemSelectCanvas.LoadingCanvas();
        ModeSelectCanvas.gameObject.SetActive(false);
    }

    public void CloseMenus()
    {
        ItemSelectCanvas.gameObject.SetActive(false);
        ModeSelectCanvas.gameObject.SetActive(false);
    }

    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ActivateModeSelect();
        }
    }
}
