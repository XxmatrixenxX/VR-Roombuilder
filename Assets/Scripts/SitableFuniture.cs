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

    public override void StartOptions()
    {
        base.StartOptions();
        playerOrigin = FindObjectOfType<XROrigin>();
    }

    private void Update()
    {
        SitDown();
    }


    public void SittingOnChair()
    {
        if(buildingCharakter.activeMode == SC_For_Mode.Mode.playerMode)
            sitting = true;
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
        sitting = false;
        playerOrigin.transform.position = exitPosition.transform.position;
    }
}