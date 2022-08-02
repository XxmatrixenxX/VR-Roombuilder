using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class SitableFuniture : Funiture
{
    [SerializeField] private Transform exitPosition;
    [SerializeField] private Transform enterPosition;
    [SerializeField] private XROrigin playerOrigin;

    [SerializeField] private bool sitting = false;

    private float hightBeforeSitting;
    public override void StartOptions()
    {
        base.StartOptions();
        playerOrigin = FindObjectOfType<XROrigin>();
        inputManager.RightControllerSecondary += ExitSitting;
        inputManager.LeftControllerSecondary += ExitSitting;
    }

    private void Update()
    {
        SitDown();
    }


    public void SittingOnChair()
    {
        if (buildingCharakter.activeMode == SC_For_Mode.Mode.playerMode)
        {
            hightBeforeSitting = playerOrigin.transform.position.y;
            sitting = true;
        }
            
    }
    public void SitDown()
    {
        if (!sitting) return;
        var transformPlayer = playerOrigin.transform;
        var transformEnter = enterPosition.transform;
        transformPlayer.position = transformEnter.position;
        transformPlayer.rotation = transformEnter.rotation;
    }

    public void ExitSitting()
    {
        if (!sitting) return;
        sitting = false;
        var position = exitPosition.transform.position;
        playerOrigin.transform.position = new Vector3(position.x, hightBeforeSitting, position.z);
    }
}