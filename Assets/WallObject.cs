using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class WallObject : MonoBehaviour
{
    [SerializeField] private Transform rightWallPoint;
    [SerializeField] private Transform leftWallPoint;
    [SerializeField] private Transform topWallPoint;

    [SerializeField]
    [Range(0f, 360f)] private float rotation;
    
    
    [SerializeField] private GameObject uiObject;

    [SerializeField] private GameObject wall;
    
    [SerializeField] private Slider rotationSlider;
    
    [SerializeField] private Text rotationText;

    [SerializeField] private GameObject rightWallAdditon;
    [SerializeField] private GameObject leftWallAddition;

    private void Awake()
    {
        rightWallAdditon.SetActive(false);
        leftWallAddition.SetActive(false);
    }

    public void DestroyThisFuniture()
    {
        Destroy(this.gameObject);
    }

    private void UiActive()
    {
        uiObject.SetActive(true);
    }

    public void Accepted()
    {
        uiObject.SetActive(false);
    }
    
    /// <summary>
    /// Rotate the Object
    /// </summary>
    private void SetTransformRotation()
    {
        if(wall.transform.GetChild(0) != null)
        {
            wall.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -rotation, 0);
        }
    }
    
    public void SliderRotation(Slider rotation)
    {
        this.rotation = rotation.value;
        SetTransformRotation();
        ChangeRotationText();
    }
    
    public void ChangeRotationText()
    {
        rotationText.text = rotation.ToString(CultureInfo.InvariantCulture);
    }

    public void AddWall(string position)
    {
        switch (position)
        {
            case "left":
                leftWallAddition.SetActive(true);
                Instantiate(wall.transform, leftWallPoint.position, Quaternion.Euler(0, -rotation +180, 0));
                break;
            case "top":
                Instantiate(wall.transform, topWallPoint.position, Quaternion.Euler(0, -rotation, 0));
                break;
            case "right":
                rightWallAdditon.SetActive(true);
                Instantiate(wall.transform, rightWallPoint.position, Quaternion.Euler(0, -rotation, 0));
                break;
        }
    }
    
}
