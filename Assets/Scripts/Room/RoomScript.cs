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

    public GameObject itemHolder;

    public GameObject bottom;
    public GameObject roof;

    public bool roofActivated = false;

    public WallRow.Direction direction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateLeftDirection()
    {
        switch (direction)
        {
            case WallRow.Direction.North:
                direction = WallRow.Direction.West;
                break;
            case WallRow.Direction.East:
                direction = WallRow.Direction.North;
                break;
            case WallRow.Direction.South:
                direction = WallRow.Direction.East;
                break;
            case WallRow.Direction.West:
                direction = WallRow.Direction.South;
                break;
        }
    }
   
    public void RotateRightDirection()
    {
        switch (direction)
        {
            case WallRow.Direction.North:
                direction = WallRow.Direction.East;
                break;
            case WallRow.Direction.East:
                direction = WallRow.Direction.South;
                break;
            case WallRow.Direction.South:
                direction = WallRow.Direction.West;
                break;
            case WallRow.Direction.West:
                direction = WallRow.Direction.North;
                break;
        }
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
            //right
            case 2:
                return GetDirection(WallRow.Direction.East);
            //down
            case 3:
                return GetDirection(WallRow.Direction.South);
            //left
            case 4:
                return GetDirection(WallRow.Direction.West);
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
        if(itemList.Contains(item))
            itemList.Remove(item);
    }

    public void ChangeBottomDesign(Material material)
    {
        Renderer render = bottom.transform.GetComponent<Renderer>();

        render.sharedMaterial = material;
    }

    public void SetItemHolderAsParent(GameObject item)
    {
        AddItemToList(item);
        item.transform.SetParent(itemHolder.transform);
    }
}
