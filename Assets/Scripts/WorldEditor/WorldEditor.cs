using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldEditor : MonoBehaviour
{
    [SerializeField] private RoomPlaceHolder roomPlaceHolder;

    [Tooltip("top to bot size")] [SerializeField]
    public float width;

    [Tooltip("left to right size")] [SerializeField]
    public float length;


    [SerializeField] private int platformLength = 12;

    [SerializeField] public Material placeAble;
    [SerializeField] public Material notPlaceAble;
    [SerializeField] public Material normal; 

    [SerializeField] private List<RoomPlaceHolder> worldArea;

    [SerializeField] public GameObject placeHolderListObject;

    [SerializeField] public PrefabSaver prefabSaver;
    
    public SC_For_RoomList worldList;

    public enum FieldColor
    {
        Dark,
        Bright
    }

    private FieldColor colorStart;


    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<UIRoomChooser>() != null)
        {
            AreaBuilding();
        }
#if UnityEditor
        if (FindObjectOfType<PrefabSaver>() != null)
        {
            prefabSaver = FindObjectOfType<PrefabSaver>();
            prefabSaver.SavedObject += AddWorldPrefabToScriptableObject;
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<RoomPlaceHolder> ReturnPlaceHolderList()
    {
        return worldArea;
    }

    public List<RoomScript> ReturnRoomOfList()
    {
        List<RoomScript> rooms = new List<RoomScript>();
        foreach (var roomSlot in worldArea)
        {
            if (roomSlot.HasRoom())
            {
                rooms.Add(roomSlot.roomInside);
            }
        }

        return rooms;
    }

    private void AreaBuilding()
    {
        var locationColumn = 0;
        var locationRow = 0;
        var startColumn = 60;
        var startRow = -60;

        locationColumn = startColumn;
        //collumn is width
        for (float columns = 0f; columns <= width; columns++)
        {
            locationRow = startRow;
            if (columns % 2 == 0)
            {
                ColorStartBright(true);
            }

            if (columns % 2 == 1)
            {
                ColorStartBright(false);
            }

            //row is length
            for (float row = 0f; row <= length; row++)
            {
                Vector3 vector3 = createVector3(locationRow, locationColumn);
                RoomPlaceHolder room = createRoomSlot(vector3);
                room.ownColor = colorStart;

                if (columns == 0)
                {
                    room.neighborTop = false;
                }

                if (columns == width)
                {
                    room.neighborBot = false;
                }

                if (row == 0)
                {
                    room.neighborLeft = false;
                }

                if (row == length)
                {
                    room.neighborRight = false;
                }

                ColorInArea(room);
                worldArea.Add(room);
                room.transform.SetParent(placeHolderListObject.transform);

                ChangeColor();
                locationRow += platformLength;
            }

            locationColumn -= platformLength;
        }
    }


    private void ColorStartBright(bool bright)
    {
        if (bright)
        {
            colorStart = FieldColor.Bright;
        }
        else
            colorStart = FieldColor.Dark;
    }

    private Vector3 createVector3(int length, int width)
    {
        return new Vector3(length, 0, width);
    }

    private RoomPlaceHolder createRoomSlot(Vector3 location)
    {
        return Instantiate(roomPlaceHolder, location, Quaternion.identity);
    }

    private void ChangeColor()
    {
        if (colorStart == FieldColor.Bright)
        {
            colorStart = FieldColor.Dark;
        }
        else
        {
            colorStart = FieldColor.Bright;
        }
    }

    private void ColorInArea(RoomPlaceHolder roomPlaceHolder)
    {
        roomPlaceHolder.ChangeColor(normal);
    }
    
    private void AddWorldPrefabToScriptableObject(GameObject worldObject)
    {
#if UnityEditor
        SC_For_Menu world = ScriptableObject.CreateInstance<SC_For_Menu>();
        world.item = worldObject.gameObject;
        world.itemName = worldObject.name;
        
        AssetDatabase.CreateAsset(world, "Assets/ScriptableObject/WorldFolder/" +world.itemName +".asset");
        SC_For_Menu worldadd = (SC_For_Menu)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/WorldFolder/" +world.itemName +".asset", typeof(SC_For_Menu));
        SC_For_RoomList worldList = (SC_For_RoomList)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/WorldFolder/Worldlist.asset", typeof(SC_For_RoomList));
        worldList.roomObject.Add(worldadd);
        AssetDatabase.SaveAssets();
        
        //worldList.roomObject.Add(worldadd);
#endif
    }
}