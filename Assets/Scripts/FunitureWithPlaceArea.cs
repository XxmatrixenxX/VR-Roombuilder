using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunitureWithPlaceArea : Funiture
{
    [SerializeField] private Transform placeArea;

    public float gettingHightOfFuniture()
    {
        return placeArea.position.y;
    }
}
    
