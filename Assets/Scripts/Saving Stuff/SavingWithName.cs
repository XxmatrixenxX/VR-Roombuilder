using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavingWithName : MonoBehaviour
{
    //Todo Add Roomfunction

    public string name;

    public GameObject nameCanvas;
    public GameObject allreadyTakenCanvas;
    public GameObject objectToSave;
    public Text nameText;
    private PrefabSaver prefabSaver;
    public SC_For_RoomList roomList;


    void Start()
    {
#if UnityEditor
        prefabSaver = FindObjectOfType<PrefabSaver>();
        prefabSaver.SavedObject += AddRoomPrefabToScriptableObject;
#endif
    }

    public void ActivateNameCanvas()
    {
        nameCanvas.SetActive(true);
        allreadyTakenCanvas.SetActive(false);
    }

    public void ActivateAllreadyTakenCanvas()
    {
        nameCanvas.SetActive(false);
        allreadyTakenCanvas.SetActive(true);
    }

    public void SetTextToName()
    {
        this.name = nameText.text;
    }


    public void SaveObject()
    {
#if UnityEditor
        if (objectToSave == null)
            return;
        //if name is open it will go trough
        if (prefabSaver.SaveAsPrefabWithName(objectToSave, name, false))
        {
            FindObjectOfType<WristMenuHandler>().ActivateModeSelect();
            return;
        }
        ActivateAllreadyTakenCanvas();
#endif
    }

    public void OverrideSave()
    {
#if UnityEditor
        if (objectToSave == null)
            return;
        
        prefabSaver.SaveAsPrefabWithName(objectToSave, name, true);
#endif
    }

    public void SameNameWithNumber()
    {
#if UnityEditor
        if (objectToSave == null)
            return;
        prefabSaver.SaveAsPrefab(objectToSave, name, false);
#endif
    }

    public void SetGameObejctToSaveObject(GameObject objectToBe)
    {
        objectToSave = objectToBe;
    }

    public void AddRoomPrefabToScriptableObject(GameObject roomObject)
    {
#if UnityEditor
        SC_For_Menu room = ScriptableObject.CreateInstance<SC_For_Menu>();
        room.item = roomObject;
        room.itemName = roomObject.name;

        AssetDatabase.CreateAsset(room, "Assets/ScriptableObject/RoomFolder/" + room.itemName + ".asset");
        SC_For_Menu roomadd =
            (SC_For_Menu) AssetDatabase.LoadAssetAtPath(
                "Assets/ScriptableObject/RoomFolder/" + room.itemName + ".asset", typeof(SC_For_Menu));
        //roomList.roomObject.Add(roomadd);
        SC_For_RoomList roomlist =
            (SC_For_RoomList) AssetDatabase.LoadAssetAtPath("Assets/ScriptableObject/RoomFolder/RoomList.asset",
                typeof(SC_For_RoomList));
        roomlist.roomObject.Add(roomadd);
        AssetDatabase.SaveAssets();
#endif
    }
}