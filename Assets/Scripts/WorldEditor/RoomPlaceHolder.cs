using System;
using Photon.Pun.UtilityScripts;
using Unity.VisualScripting;
using UnityEngine;

public class RoomPlaceHolder : MonoBehaviour
{
    public WorldEditor.FieldColor ownColor;

    public RoomScript roomInside;
    
    public GameObject spawn;

    public GameObject beforeRoom;
    
    public GameObject interactCollider;

    private WorldEditor worldEditor;

    private UIRoomChooser uiRoomChooser;

    private Material basicMaterial;

    private Material placeholderMaterial;

    private bool placeAble = false;
    
    [SerializeField] private float colorChange = 0.25f;

    public bool
        neighborTop = true,
        neighborRight = true,
        neighborBot = true,
        neighborLeft = true;


    private void Start()
    {
        if (FindObjectOfType<UIRoomChooser>() == null && !HasRoom())
        {
            DisableBeforeRoom();
        }
        uiRoomChooser = FindObjectOfType<UIRoomChooser>();
        if(basicMaterial != null)
            ChangeColor(basicMaterial);
        worldEditor = FindObjectOfType<WorldEditor>();
    }

    public bool HasRoom()
    {
        if (roomInside == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public RoomScript ReturnRoom()
    {
        return roomInside;
    }

    //If room is not null
    public void DisableBeforeRoom()
    {
        beforeRoom.SetActive(false);
    }

    //If room is null
    public void EnableBeforeRoom()
    {
        beforeRoom.SetActive(true);
    }

    public void InteractWithPlaceHolder()
    {
        if (uiRoomChooser.mode == UIRoomChooser.WorldEditorMode.Destroy)
        {
            RemoveRoom();
        }
        else if (uiRoomChooser.mode == UIRoomChooser.WorldEditorMode.Build)
        {
            if(roomInside == null)
                AddRoom(uiRoomChooser.ReturnRoomObject());
        }
        else if (uiRoomChooser.mode == UIRoomChooser.WorldEditorMode.SpawnPoint)
        {
            if (roomInside != null)
            {
             uiRoomChooser.SpawnPoint(this);   
            }
        }
    }
    

    public void AddRoom(GameObject room)
    {
        if (!placeAble) return;
        if (worldEditor.ReturnRoomOfList().Count <= 0)
        {
            uiRoomChooser.SpawnPoint(this);   
        }
        GameObject roomClone = Instantiate(room);
        roomClone.transform.localScale = new Vector3(1, 1, 1);
        roomInside = roomClone.GetComponent<RoomScript>();
        roomInside.transform.SetParent(this.transform, false);
        DisableBeforeRoom();
        uiRoomChooser.BuildingRooms();
    }

    public void RemoveRoom()
    {
        Destroy(roomInside.gameObject);
        uiRoomChooser.DestroyRooms();
        EnableBeforeRoom();
    }

    public int ReturnListIndex()
    {
        return worldEditor.ReturnPlaceHolderList().IndexOf(this);
    }

    //if it returns true, it can be builded
    public bool CheckNeighbors(GameObject room)
    {
        RoomScript newRoomCouldBuild = room.GetComponent<RoomScript>();
        float thisIndex = ReturnListIndex();
        float neighborIndex;
        RoomPlaceHolder neighborPlaceHolder;
        if (neighborTop)
        {
            neighborIndex = thisIndex - worldEditor.width -1;
            neighborPlaceHolder = worldEditor.ReturnPlaceHolderList()[((int) neighborIndex)];
            if (neighborPlaceHolder.roomInside == null)
            {
                //nothing
            }
            else if (!CheckNeighborWall(neighborPlaceHolder, 1,newRoomCouldBuild))
            {
                placeAble = false;
                return false;
            }
        }
        if (neighborRight)
        {
            neighborIndex = thisIndex  + 1;
            neighborPlaceHolder = worldEditor.ReturnPlaceHolderList()[((int) neighborIndex)];
            if (neighborPlaceHolder.roomInside == null)
            {
                //nothing
            }
            else if (!CheckNeighborWall(neighborPlaceHolder, 2,newRoomCouldBuild))
            {
                placeAble = false;
                return false;
            }
        }
        if (neighborBot)
        {
            neighborIndex = thisIndex + worldEditor.width +1;
            neighborPlaceHolder = worldEditor.ReturnPlaceHolderList()[((int) neighborIndex)];
            if (neighborPlaceHolder.roomInside == null)
            {
                //nothing
            }
            else if (!CheckNeighborWall(neighborPlaceHolder, 3,newRoomCouldBuild))
            {
                placeAble = false;
                return false;
            }
                
        }
        if (neighborLeft)
        {
            neighborIndex = thisIndex  - 1;
            neighborPlaceHolder = worldEditor.ReturnPlaceHolderList()[((int) neighborIndex)];
            if (neighborPlaceHolder.roomInside == null)
            {
                //nothing
            }
            else if (!CheckNeighborWall(neighborPlaceHolder, 4,newRoomCouldBuild))
            {
                placeAble = false;
                return false;
            }
        }
        placeAble = true;
        return true;
    }
    /// <summary>
    /// Needs a Number to Know if its above, beside or underneath
    /// </summary>
    ///  /// <param name="number"> 1 up, 2 right, 3 down, 4 left</param>
    public bool CheckNeighborWall(RoomPlaceHolder roomPlaceHolder, int number, RoomScript roomThatMaybeBuild)
    {
        //ConvertNumber to other sideNumber
        int otherDirection = 1;
        if (number == 1) otherDirection = 3;
        if (number == 2) otherDirection = 4;
        if (number == 3) otherDirection = 1;
        if (number == 4) otherDirection = 2;

        WallRow thisRoomWallRow = roomThatMaybeBuild.GetRowBesideNewRoom(number);
        WallRow otherRoomWallRow = roomPlaceHolder.roomInside.GetRowBesideNewRoom(otherDirection);
        
        return CheckWallRow(thisRoomWallRow, otherRoomWallRow);
    }

    
    //if the WallTypes are not the Same it returns false
    public bool CheckWallRow(WallRow firstRow, WallRow secondRow)
    {
        int rowCounter = 1;
        foreach (var wall1 in firstRow.wallRow)
        {
            WallPart wallfirst = wall1;
            
            WallPart wallSecond =  secondRow.wallRow[secondRow.wallRow.Count - rowCounter];

            if (wallfirst.wallBuilded != wallSecond.wallBuilded)
            {
                return false;
            }
            rowCounter++;
        }

        return true;
    }

    public void ChangeColor(Material material)
    {
        basicMaterial = material;
        
        if (roomInside != null)
        {
            return;
        }

        placeholderMaterial = Instantiate(material as Material);
        beforeRoom.GetComponent<Renderer>().material = placeholderMaterial;

        if (ownColor == WorldEditor.FieldColor.Dark)
        {
            DarkMaterial(beforeRoom.GetComponent<Renderer>().material);
        }
    }

    public void DarkMaterial(Material material)
    {
        colorChange = Mathf.Clamp01(colorChange);
        material.color = new Color(material.color.r * (1 - colorChange),
            material.color.g * (1 - colorChange), material.color.b * (1 - colorChange),
            material.color.a);
    }

    public void NormalPlayNoEditor()
    {
        interactCollider.SetActive(false);
        beforeRoom.SetActive(false);
    }
    
    public void EditorMode()
    {
        interactCollider.SetActive(true);
        if(roomInside == null)
            beforeRoom.SetActive(true);
    }

    public bool HasSpawnPoint()
    {
        if (spawn == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void RemoveSpawn()
    {
        if (spawn != null)
        {
            Destroy(spawn);
        }
    }


}