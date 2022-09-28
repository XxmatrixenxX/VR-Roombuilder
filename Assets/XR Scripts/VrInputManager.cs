using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VrInputManager : MonoBehaviour
{
    private InputDevice rightController;
    private InputDevice leftController;

    private bool pressedLeft = false;
    private bool pressedRight = false;


    //Right Controller
    InputDeviceCharacteristics rightControllerCharacteristics =
        InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

    //Left Controller
    InputDeviceCharacteristics leftControllerCharacteristics =
        InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

    public event Action RightControllerPrimary;
    public event Action RightControllerSecondary;
    public event Action RightControllerTrigger;
    public event Action LeftControllerPrimary;
    public event Action LeftControllerSecondary;
    public event Action LeftControllerTrigger;

    private void InvokeRightControllerPrimary() => RightControllerPrimary?.Invoke();
    private void InvokeRightControllerSecondary() => RightControllerSecondary?.Invoke();
    private void InvokeRightControllerTrigger() => RightControllerTrigger?.Invoke();
    private void InvokeLeftControllerPrimary() => LeftControllerPrimary?.Invoke();
    private void InvokeLeftControllerSecondary() => LeftControllerSecondary?.Invoke();
    private void InvokeLeftControllerTrigger() => LeftControllerTrigger?.Invoke();


    private void Start()
    {
        StartOptions();
        InputDevices.deviceConnected += InputDeviceConnected;
    }

    private void Awake()
    {
        StartOptions();
    }

    //Checks if Button is pressed for every Screen
    private void Update()
    {
        ButtonChecker(rightController, true);
        ButtonChecker(leftController, false);
    }

    //Should only run if Controllers are Online
    private void StartOptions()
    {
        List<InputDevice> devices = new List<InputDevice>();

        //Writes All Devices with the Characteristics in List
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        //Should have the right Controller in this List
        if (devices.Count > 0)
        {
            rightController = devices[0];
        }

        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            leftController = devices[0];
        }
    }
    
    /// <summary>
    /// If a new Device Connect, repeat the StartOptions
    /// </summary>
    /// <param name="inputDevice"></param>
    private void InputDeviceConnected(InputDevice inputDevice)
    {
        StartOptions();
    }

    /// <summary>
    /// Method to Invoke different Button Events
    /// </summary>
    /// <param name="device">Controller which is Used</param>
    /// <param name="right">Bool to decided if its a left or right Controller</param>
    private void ButtonChecker(InputDevice device, bool right)
    {
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButttonValue) &&
            primaryButttonValue)
        {
            if (right && !pressedRight)
            {
                Debug.Log("Invoke Right controller primary");
                InvokeRightControllerPrimary();
                pressedRight = true;
            }
            else if (!right && !pressedLeft)
            {
                Debug.Log("Invoke left controller primary");
                InvokeLeftControllerPrimary();
                pressedLeft = true;
            }
        }

        else if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) &&
                 triggerValue > 0.4f)
        {
            if (right && !pressedRight)
            {
                Debug.Log("Invoke Right controller trigger");
                InvokeRightControllerTrigger();
                pressedRight = true;
            }
            else if (!right && !pressedLeft)
            {
                Debug.Log("Invoke left controller trigger");
                InvokeLeftControllerTrigger();
                pressedLeft = true;
            }
        }

        else if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue) &&
                 secondaryButtonValue)
        {
            if (right && !pressedRight)
            {
                Debug.Log("Invoke Right controller secondary");
                InvokeRightControllerSecondary();
                pressedRight = true;
            }
            else if (!right && !pressedLeft)
            {
                Debug.Log("Invoke left controller secondary");
                InvokeLeftControllerSecondary();
                pressedLeft = true;
            }
        }

        else
        {
            if (right)
            {
                pressedRight = false;
            }
            else
            {
                pressedLeft = false;
            }
        }
    }
}