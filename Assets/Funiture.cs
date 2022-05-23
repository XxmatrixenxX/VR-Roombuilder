using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Funiture : MonoBehaviour
{

    private float turnAngles = 30f;

    [SerializeField]
    [Range(0f, 360f)]
    private float directionRange;
    [SerializeField] private Text rotationText;

    private float sizeSteps = 0.1f;
    
    [SerializeField]
    [Range(0f, 10f)]
    private float size;

    [SerializeField] private Text sizeText;
    

    private Vector3 location;

    [SerializeField] private GameObject uiObject;

    [SerializeField] private GameObject funiture;

    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Slider rotationSlider;

    public Collider funitureCollider;

    private void Start()
    {
        SliderStartPosition();
       // Accepted();
    }

    private void Awake()
    {
        SliderStartPosition();
        //Accepted();
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
    /// Turns the Object in a Direction
    /// </summary>
    /// <param name="left">If true it will turn to the left, false to right.</param>
    private void TurnFuniture(bool left)
    {
        if (left)
        {
           directionRange = CalculateDirection(directionRange, turnAngles);
        }
        else 
        {
            directionRange = CalculateDirection(directionRange, -turnAngles);
        }
    }
    
    /// <summary>
    /// Calculate the Float for the Range
    /// </summary>
    /// <param name="direction">Parameter for the direction the Funiture show to at the Moment</param>
    /// <param name="difference">Parameter for turn amount, should not greater then 360f and not smaller then -360f</param>
    private float CalculateDirection(float direction, float difference)
    {
        float result = direction + difference;

        if (result > 360f)
        {
            return result - 360f;
        }

        if (result == 0f)
        {
            return 0f;
        }
        return 360f + result;
    }

    /// <summary>
    /// Rotate the Object
    /// </summary>
    private void SetTransformRotation()
    {
        if(funiture.transform.GetChild(0) != null)
        {
            funiture.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, directionRange, 0);
        }
    }
    
    /// <summary>
    /// Changes Size of Object
    /// </summary>
    /// <param name="bigger"> If true it will be bigger.</param>
    private void ChangeSize(bool bigger)
    {
        if (bigger)
        {
            directionRange = CalculateSize(directionRange,sizeSteps );
        }
        else 
        {
            directionRange = CalculateSize(directionRange, -sizeSteps);
        }
    }
    
    /// <summary>
    /// Calculate the Float for the Range
    /// </summary>
    /// <param name="size">Parameter for the Size of the Funiture</param>
    /// <param name="sizeDifference">Parameter for size amount, should not greater then 10f and not smaller then -10f</param>
    private float CalculateSize(float size, float sizeDifference)
    {
        float result = size + sizeDifference;

        if (result > 10f)
        {
            return result - 10f;
        }

        if (result == 0f)
        {
            return 10f;
        }
        return 10f + result;
    }

    /// <summary>
    /// Changes Sizes of Object
    /// </summary>
    private void SetSize(float size)
    {
        funiture.transform.localScale = new Vector3(size, size, size);
    }

    public void SliderStartPosition()
    {
        sizeSlider.value = size;
        rotationSlider.value = directionRange;
    }

    public void SliderSize(Slider size)
    {
        this.size = size.value * sizeSteps;
        SetSize(this.size);
        ChangeSizeText();
    }
    
    public void SliderRotation(Slider rotation)
    {
        this.directionRange = rotation.value * turnAngles;
        SetTransformRotation();
        ChangeRotationText();
    }

    public void ChangeSizeText()
    {
        sizeText.text = size.ToString(CultureInfo.InvariantCulture);
    }

    public void ChangeRotationText()
    {
        rotationText.text = directionRange.ToString(CultureInfo.InvariantCulture);
    }
    
}
