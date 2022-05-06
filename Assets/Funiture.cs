using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Funiture : MonoBehaviour
{

    private float turnAngles = 30f;

    [Range(0f, 360f)]
    private float directionRange;

    private float sizeSteps = 0.1f;
    
    [Range(0f, 10f)]
    private float size;

    private Vector3 location;


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
        this.transform.rotation = Quaternion.Euler(0, directionRange, 0);
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
    private void SetSize()
    {
        this.transform.localScale = new Vector3(size, size, size);
    }
    
    


}
