using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private LoadingScript loadingScript;

    [SerializeField] private GameObject player;
    void Start()
    {
         loadingScript = FindObjectOfType<LoadingScript>();
         if(loadingScript.loadingtype != LoadingScript.LoadingType.Nothing)
            LoadObject(loadingScript.loadingObject);
    }

    public void LoadObject(GameObject savedObject)
    {
        Instantiate(savedObject, Vector3.zero, quaternion.identity);
        float spawnX = 0;
        float spawnZ = 0;
        if (loadingScript.loadingtype == LoadingScript.LoadingType.World)
        {
            bool spawnFound = false;
            foreach (var roomPlaceHolder in FindObjectOfType<WorldEditor>().ReturnPlaceHolderList())
            {
                if (roomPlaceHolder.HasSpawnPoint())
                {
                    var position = roomPlaceHolder.spawn.transform.position;
                    spawnX = position.x;
                    spawnZ = position.z;
                    spawnFound = true;
                }

                if (spawnFound)
                {
                    break;
                }
            }

            player.transform.position = new Vector3(spawnX, player.transform.position.y, spawnZ);
        }
    }

   
}
