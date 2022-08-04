using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public List<WallRow> wallRowList;

    public List<GameObject> itemList;

    public GameObject bottom;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateRoomRight()
    {
        transform.Rotate(0f, 90f, 0f);
        foreach (var wallRow in wallRowList)
        {
            wallRow.RotateRight();
        }
    }
    public void RotateRoomLeft()
    {
        transform.Rotate(0f, -90f, 0f);
        foreach (var wallRow in wallRowList)
        {
            wallRow.RotateLeft();
        }
    }

    /// <summary>
    /// Needs a Number to Know if its above, beside or underneath
    /// </summary>
    ///  /// <param name="number"> 1 up, 2 right, 3 down, 4 left</param>
    public WallRow GetRowBesideNewRoom(int number)
    {
        switch (number)
        {
            //up
            case 1:
                return GetDirection(WallRow.Direction.North);
                break;
            //right
            case 2:
                return GetDirection(WallRow.Direction.East);
                break;
            //down
            case 3:
                return GetDirection(WallRow.Direction.South);
                break;
            //left
            case 4:
                return GetDirection(WallRow.Direction.West);
                break;
        }

        return null;
    }

    /// <summary>
    /// Returns row with the desired direction
    /// </summary>
    public WallRow GetDirection(WallRow.Direction direction)
    {
        foreach (var row in wallRowList)
        {
            if (row.direction == direction)
            {
                return row;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds a Item or GameObject of the Room to the List
    /// Is for the Save
    /// </summary>
    public void AddItemToList(GameObject item)
    {
        itemList.Add(item);
    }

    /// <summary>
    /// Removes the Item from the Room if its deleted
    /// </summary>
    public void RemoveItemFromList(GameObject item)
    {
        itemList.Remove(item);
    }

    public void ChangeBottomDesign(Material material)
    {
        Renderer render = bottom.transform.GetComponent<Renderer>();

        render.sharedMaterial = material;
    }
}
