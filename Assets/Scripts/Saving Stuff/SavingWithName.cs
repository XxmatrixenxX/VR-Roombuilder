using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SavingWithName : MonoBehaviour
{
    public string name;

    public GameObject nameCanvas;
    public GameObject allreadyTakenCanvas;
    public GameObject objectToSave;
    private PrefabSaver prefabSaver;

    void Start()
    {
        prefabSaver = GetComponent<PrefabSaver>();

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

    public void SetName(string name)
    {
        this.name = name;
    }


    public void SaveObject()
    {
        if (objectToSave == null)
            return;
        //if name is open it will go trough
        if (prefabSaver.SaveAsPrefabWithName(objectToSave, name, false))
        {
            return;
        }
        ActivateAllreadyTakenCanvas();
    }

    public void OverrideSave()
    {
        if (objectToSave == null)
            return;
        
        prefabSaver.SaveAsPrefabWithName(objectToSave, name, true);
    }
    
    public void SameNameWithNumber()
    {
        if (objectToSave == null)
            return;
        prefabSaver.SaveAsPrefab(objectToSave, name, false);
    }

    public void SetGameObejctToSaveObject(GameObject objectToBe)
    {
        objectToSave = objectToBe;
    }
}
