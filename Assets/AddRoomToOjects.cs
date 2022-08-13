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
        roomChecker.RoomChanged += ChangeRoom;
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
        hologram.room = room;
        savingWithName.objectToSave = room.gameObject;
    }
}
