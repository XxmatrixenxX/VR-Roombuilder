using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunitureWithPlaceArea : Funiture
{
    [SerializeField] private Transform placeArea;

    [SerializeField] private Collider placeAreaCollider;

    public void SetObjectOnTop()
    {
        
    }

    public float LiftObject()
    {
        return placeArea.transform.position.y;
    }

    public float lowerObject()
    {
        return -placeArea.transform.position.y;
    }

    public void OnCollisionExit(Collision other)
    {
        lowerObject();
    }
}
    
