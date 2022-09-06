using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class PrefabSaver : MonoBehaviour
{

    public event Action<GameObject> SavedObject;
    public void SaveObjectInvoke(GameObject prefab) => SavedObject?.Invoke(prefab);

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
        string localPath = "Assets/Prefabs/SavedPrefabs/" + name ;
    
        var loadedObject = Resources.Load(localPath);
        if (loadedObject == null)
        {
            return false;
        }

        return true;
    }


    //creates Prefab with the name
    public void SaveAsPrefab(GameObject gameObjectToPrefab, string name, bool replace)
    {
        string localPath = "Assets/Prefabs/SavedPrefabs/" + name + ".prefab";
        
        if(!replace)
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAsset(gameObjectToPrefab, localPath);
        PrefabGetting(localPath);
    }
    
    //creates Prefab with the GameObject name
    public void SaveAsPrefab(GameObject gameObjectToPrefab, bool replace)
    {
        string localPath = "Assets/Prefabs/SavedPrefabs/" + gameObjectToPrefab.name + ".prefab";

        if(!replace)
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAsset(gameObjectToPrefab, localPath);
        PrefabGetting(localPath);
    }


    public void PrefabGetting(string localPath)
    {
        
        var loadedObject = AssetDatabase.LoadAssetAtPath(localPath, typeof(UnityEngine.Object));
        if (loadedObject == null)
        {
            throw new FileNotFoundException("File not found with the Name");
        }

        SaveObjectInvoke(loadedObject.GameObject());
    }
    
    public void SaveWorld(GameObject gameObjectToPrefab, bool replace)
    {
        string localPath = "Assets/Prefabs/SavedPrefabs/World/" + "World.prefab";
        
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAsset(gameObjectToPrefab, localPath);
        PrefabGetting(localPath);
    }
    
}
