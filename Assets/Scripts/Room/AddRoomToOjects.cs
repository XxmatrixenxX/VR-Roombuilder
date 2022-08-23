using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoomToOjects : MonoBehaviour
{
    public BottomRoofHandler bottomRoofHandler;
    public SavingWithName savingWithName;
    public Hologram hologram;

    public RoomChecker roomChecker;
    public RoomScript room;

    public void Start()
    {
        if (roomChecker != null)
        {
            roomChecker.RoomChanged += ChangeRoom; 
        }
       
    }

    private void ChangeRoom(RoomScript obj)
    {
        room = obj;
        AddRoomToScripts();
    }

    public void AddRoomToScripts()
    {
        bottomRoofHandler.bottom = room.bottom;
        bottomRoofHandler.roof = room.roof;
        bottomRoofHandler.room = room;
        hologram.room = room;
        savingWithName.objectToSave = room.gameObject;
    }
}
