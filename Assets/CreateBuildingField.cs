using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBuildingField : MonoBehaviour
{
    [SerializeField] private GameObject buildingObject;
    [SerializeField] private float amountOfFields;
    [SerializeField] private float value;


    private void Start()
    {
        value = DistanceToSpawn(amountOfFields);
        CreateField(amountOfFields);
    }

    private float DistanceToSpawn(float amountOfFields)
    {
        float distance = amountOfFields / 2f;
        distance = Mathf.Round(distance);
        return distance;
    }

    private void CreateField(float amountOfFields)
    {
        float startFloat = DistanceToSpawn(amountOfFields);
        Vector3 startPoint = new Vector3(-startFloat, 0, -startFloat);
        
        for (float counterX = 0; counterX <= amountOfFields; counterX++)
        {
            Instantiate(buildingObject, new Vector3(-startFloat + counterX, 0, -startFloat), Quaternion.identity);
            for (float counterZ = 0; counterZ <= amountOfFields; counterZ++)
            {
                Instantiate(buildingObject, new Vector3(-startFloat + counterX, 0, -startFloat + counterZ), Quaternion.identity);
            }
        }
    }
}
