using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using Unity.Mathematics;
using UnityEngine;

public class UIRoomChooser : MonoBehaviour
{
  [SerializeField] private GameObject roomHolder;
  [SerializeField] private GameObject containerOfRoom;

  [SerializeField] private UIMenuItem roomUI;
  
  [SerializeField] private SC_For_RoomList roomList;

  [SerializeField] private WorldEditor worldEditor;

  [SerializeField] private GameObject spawnObject;

  public bool roofVisualToggle = true;

  private Vector3 rotationNorth = new Vector3(-60, 0, 0);
  private Vector3 rotationEast = new Vector3(0, 90, -60);
  private Vector3 rotationSouth = new Vector3(60, 180, 0);
  private Vector3 rotationWest = new Vector3(0, 270, 60);

  public WorldEditorMode mode = WorldEditorMode.Destroy;

  public enum WorldEditorMode
  {
    Destroy,
    Build,
    SpawnPoint
  }

  public void Start()
  {
    worldEditor = FindObjectOfType<WorldEditor>();
    FillRoomInContainer();
  }

  private void FillRoomInContainer()
  {
    foreach (Transform exampleDelete in containerOfRoom.transform)
    {
      GameObject.Destroy(exampleDelete.gameObject);
    }
        
    foreach (var sc_menu in roomList.roomObject)
    {
      UIMenuItem menu = Instantiate(roomUI, new Vector3(0, 0, 0), Quaternion.identity);

      menu.transform.SetParent(containerOfRoom.transform, false);
            
      menu.title.text = sc_menu.itemName;
      menu.button.onClick.AddListener(delegate { ClickedRoom(sc_menu.item); });
    }
  }

  public void RotateLeft()
  {
   roomHolder.transform.GetChild(0).gameObject.GetComponent<RoomScript>().RotateRoomLeft();
   if (mode == WorldEditorMode.Build)
   {
     BuildingRooms();
   }
  }

  public void RotateRight()
  {
    roomHolder.transform.GetChild(0).gameObject.GetComponent<RoomScript>().RotateRoomRight(); 
    if (mode == WorldEditorMode.Build)
    {
      BuildingRooms();
    }
  }

  public void ToggleRoofShow()
  {
    roofVisualToggle = !roofVisualToggle;
    RoofOfObjects(roofVisualToggle);
  }

  public void RoofOfObjects(bool toggle)
  {
    List<RoomScript> rooms = worldEditor.ReturnRoomOfList();

    foreach (var room in rooms)
    {
      if (room.roofActivated)
      {
        room.roof.SetActive(toggle);
      }
    }
  }

  public void DestroyRooms()
  {
    mode = WorldEditorMode.Destroy;
    foreach (var roomPlaceHolder in worldEditor.ReturnPlaceHolderList())
    {
      roomPlaceHolder.ChangeColor(worldEditor.normal);
    }
  }
  
  public void SpawnPoint()
  {
    mode = WorldEditorMode.SpawnPoint;
  }

  public void BuildingRooms()
  {
    mode = WorldEditorMode.Build;
    foreach (var roomPlaceHolder in worldEditor.ReturnPlaceHolderList())
    {
      if (roomPlaceHolder.CheckNeighbors(roomHolder.transform.GetChild(0).gameObject))
      {
        roomPlaceHolder.ChangeColor(worldEditor.placeAble);
      }
      else
      {
        roomPlaceHolder.ChangeColor(worldEditor.notPlaceAble);
      }
    }
  }

  private void ClickedRoom(GameObject room)
  {
    foreach (Transform exampleDelete in roomHolder.transform)
    {
      GameObject.Destroy(exampleDelete.gameObject);
    }

    GameObject roomClicked = Instantiate(room, Vector3.zero, quaternion.identity);
    
    roomClicked.transform.SetParent(roomHolder.transform,false);
    roomClicked.transform.localScale = new Vector3(10, 10, 10);
    
    if (mode == WorldEditorMode.Build)
    {
      BuildingRooms();
    }
    
  }

  public GameObject ReturnRoomObject()
  {
    return roomHolder.transform.GetChild(0).gameObject;
  }

  public void SpawnPoint(RoomPlaceHolder room)
  {
    foreach (var roomInEditor in worldEditor.ReturnPlaceHolderList())
    {
      if (roomInEditor.HasSpawnPoint())
      { 
        roomInEditor.RemoveSpawn();
      }
    }
    GameObject spawn = Instantiate(spawnObject, room.transform, false);
    room.spawn = spawn;
  }

  public void SaveWorld()
  {
#if UnityEditor
    FindObjectOfType<PrefabSaver>().SaveWorld(worldEditor.gameObject, false);
#endif
  }
  
  
  
  

}
