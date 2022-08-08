using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabSaver : MonoBehaviour
{

    /// <summary>
    /// Creates a Prefab if the path of the name doesn't exist
    /// </summary>
    ///  /// <param name="replace"> if this is checked the Name can be overridden</param>
    public bool SaveAsPrefabWithName(GameObject gameObjectToPrefab, string name, bool replace)
    {
        if (!replace)
        {
            //if it is allready defined give false back
            if (LookForName(name))
            {
                return false;
            }
            
        }
        
        SaveAsPrefab(gameObjectToPrefab, name, replace);
        return true;
    }

    private bool LookForName(string name)
    {
        string localPath = "Assets/Prefabs/SavedPrefabs" + name + ".prefab";
        
        return AssetDatabase.IsValidFolder(localPath);
    }

    //creates Prefab with the name
    public void SaveAsPrefab(GameObject gameObjectToPrefab, string name, bool replace)
    {
        string localPath = "Assets/Prefabs/SavedPrefabs" + name + ".prefab";
        
        if(!replace)
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObjectToPrefab, localPath, InteractionMode.UserAction);
    }
    
    //creates Prefab with the GameObject name
    public void SaveAsPrefab(GameObject gameObjectToPrefab, bool replace)
    {
        string localPath = "Assets/Prefabs/SavedPrefabs" + gameObjectToPrefab.name + ".prefab";

        if(!replace)
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObjectToPrefab, localPath, InteractionMode.UserAction);
    }
}
