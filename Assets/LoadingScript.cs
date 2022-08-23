using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour
{
    public GameObject loadingObject;

    public LoadingType loadingtype = LoadingType.Nothing;
    public enum LoadingType
    {
        Room,
        World,
        Nothing
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRoom(GameObject room)
    {
        loadingtype = LoadingType.Room;
        loadingObject = room;
    }
    
    public void AddWorld(GameObject world)
    {
        loadingtype = LoadingType.World;
        loadingObject = world;
    }
    
}
