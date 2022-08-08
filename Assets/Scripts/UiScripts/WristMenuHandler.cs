using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristMenuHandler : MonoBehaviour
{
    [SerializeField] private UIMenuChooseHandler ItemSelectCanvas;
    [SerializeField] private UIModeChoose ModeSelectCanvas;
    [SerializeField] private GameObject UIOpener;
    [SerializeField] private GameObject UIWindow;
    [SerializeField] private GameObject BottomRoofWindow;


    private void Awake()
    {
        ActivateModeSelect();
    }

    public void ActivateModeSelect()
    {
        ItemSelectCanvas.gameObject.SetActive(false);
        ModeSelectCanvas.gameObject.SetActive(true);
        BottomRoofWindow.SetActive(false);
    }

    public void ActivateItemSelectCanvas()
    {
        ItemSelectCanvas.gameObject.SetActive(true);
        ItemSelectCanvas.LoadingCanvas();
        ModeSelectCanvas.gameObject.SetActive(false);
        BottomRoofWindow.SetActive(false);
    }
    public void ActivateBottomRoof()
    {
        ModeSelectCanvas.gameObject.SetActive(false);
        BottomRoofWindow.SetActive(true);
    }

    public void CloseMenus()
    {
       ActivateUIOpener();
       CloseUIWindow();
    }

    public void OpenMenus()
    {
        DisableUIOpener();
        ActivateUIWindow();
    }

    public void ActivateUIOpener()
    {
        UIOpener.SetActive(true);
    }

    public void DisableUIOpener()
    {
        UIOpener.SetActive(false);
    }

    public void CloseUIWindow()
    {
        UIWindow.SetActive(false);
    }

    public void ActivateUIWindow()
    {
        UIWindow.SetActive(true);
    }

    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ActivateModeSelect();
        }
    }
}
