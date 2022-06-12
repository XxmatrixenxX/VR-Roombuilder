using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashDragablePoint : MonoBehaviour
{
    public Vector3 location;

    public MashDragablePoint(Vector3 locationInstance)
    {
        location = locationInstance;
    }

    [SerializeField]
    private Transform objectToInteractWith;

    public event Action LocationChanged;
    
    public void InvokeLocationChanged() => LocationChanged?.Invoke();

    

    public void DragObjectToPoint(Vector3 newLocation)
    {
        location = newLocation;
        InvokeLocationChanged();
    }
}
