using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    public RoomScript roomObject;

    public event Action<RoomScript> RoomChanged;
    
    public void InvokeRoomChanged(RoomScript room) => RoomChanged?.Invoke(room);
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Roomchecker: Entered");
        if (collision.CompareTag("Room"))
        {
            if (roomObject == null)
            {
                roomObject = collision.gameObject.GetComponent<RoomScript>();;
                InvokeRoomChanged(roomObject);
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("Roomchecker: Inside");
        if (collision.CompareTag("Room"))
        {
            if (roomObject == null)
            {
                roomObject = collision.gameObject.GetComponent<RoomScript>();;
                InvokeRoomChanged(roomObject);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Roomchecker: Exit");
        if (collision.CompareTag("Room"))
        {
            if (roomObject == collision.gameObject)
            {
                roomObject = null;
                InvokeRoomChanged(roomObject);
            }
        }
    }
}
