using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hologram : MonoBehaviour
{
    //Todo Add Roomfunction

    public RoomScript room;
    
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject hologramCopy;
    [SerializeField] private GameObject hologramCopyForTables;
    [SerializeField] private Material ghostMaterial;

    [SerializeField] private Material blockedMaterial;
    //[SerializeField] private GridBuildingSystem gridSystem;

    [SerializeField] private Transform hologramReferenzPosition;
    [SerializeField] private Transform visualCreationPoint;

    [SerializeField] private Transform xr;

    [SerializeField] private bool blocked;

    [SerializeField] private float hightOfObject = 0f;

    public PrimaryButtonWatcher watcher;

    public VrInputManager inputManager;
    [SerializeField] private BuildingCharakter buildingCharakter;

    private void Awake()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
        inputManager = FindObjectOfType<VrInputManager>();
    }

    private void Start()
    {
        //watcher.primaryButtonPress.AddListener(CreateObject);
        ChangeHologram(visual);

        buildingCharakter.ModeChanged += DisableHologram;

        //Input for Controller
        inputManager.RightControllerPrimary += CreateObject;
        inputManager.LeftControllerPrimary += CreateObject;
    }

    private void Update()
    {
        HologramMover();
        HologramForTablesMover();
    }

    /// <summary>
    /// Adds Listeners to Object
    /// If it is onTable it only adds OnTable Events
    /// Else all Listeners get added
    /// </summary>
    /// <param name="hologramSpawnPointObject"></param>
    private void AddListeners(GameObject hologramSpawnPointObject)
    {
        if (hologramSpawnPointObject.GetComponent(typeof(HologramSpawnPoint)) != null)
        {
            Debug.Log("Added HologramSpawnPoint Listeners");
            HologramSpawnPoint spawnpoint = hologramSpawnPointObject.GetComponent<HologramSpawnPoint>();
            if (spawnpoint.OnTable)
            {
                //for Table Hologram
                spawnpoint.TopHologramEnteredFuniture += EnteredTable;
                spawnpoint.TopHologramExit += ExitTable;
            }
            else
            {
                //for Basic Furniture
                spawnpoint.HologramEnteredFuniture += EnteredNormal;
                spawnpoint.HologramExitFuniture += ExitNormal;
                //for Tables
                spawnpoint.HologramEnteredFunitureTable += FurnitureWithPlacementArea;
                spawnpoint.HologramExitFunitureTable += DestroyHologramForTables;
            }
        }
    }

    
    /// <summary>
    /// Create Transform where the Material is Lightblue transparent
    /// </summary>
    /// <param name="aPrefab"> Object that will be copied</param>
    /// <returns>Created GameObject</returns>
    public Transform CreatePreview(Transform aPrefab)
    {
        Debug.Log("Create Preview");

        Transform obj = (Transform) Instantiate(aPrefab);
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
            renderer.sharedMaterial = ghostMaterial;

        foreach (var collider in obj.GetComponentsInChildren<Collider>(true))
            collider.tag = "Untagged";

        obj.tag = "Untagged";
        return obj;
    }

    /// <summary>
    /// If there is a Hologram that can move
    /// Move it with the XR Character
    /// </summary>
    private void HologramMover()
    {
        if (hologramCopy != null)
        {
            hologramCopy.transform.position = hologramReferenzPosition.position;
            var eulerAngles = hologramReferenzPosition.transform.eulerAngles;
           
            //90 different for the Transform
            hologramCopy.transform.eulerAngles =
                    new Vector3(eulerAngles.x, xr.transform.eulerAngles.y + 90, eulerAngles.z);
        }
    }

    /// <summary>
    /// Creates Hologram for Table
    /// </summary>
    private void FurnitureWithPlacementArea(FunitureWithPlaceArea item)
    {
        hightOfObject = item.gettingHightOfFuniture();
        CreateHologramForTables();
        PlacementBlocked(hologramCopy);
    }

    //Creates a Visual on Top of the Table
    private void HologramForTablesMover()
    {
        if (blocked)
        {
            if (hologramCopyForTables != null)
            {
                var position = hologramReferenzPosition.position;
                hologramCopyForTables.transform.position =
                    new Vector3(position.x, position.y + hightOfObject, position.z);
                var eulerAngles = hologramReferenzPosition.transform.eulerAngles;
                
                //90 different for the Transform
                hologramCopyForTables.transform.eulerAngles =
                        new Vector3(eulerAngles.x, xr.transform.eulerAngles.y + 90, eulerAngles.z);
            }
        }
    }

    //Clones HologramCopy for a similar Object 
    private void CreateHologramForTables()
    {
        hologramCopyForTables = Instantiate(hologramCopy);
        hologramCopyForTables.GetComponent<HologramSpawnPoint>().SetToOnTable();
        hologramCopyForTables.GetComponent<HologramSpawnPoint>().insideFuniture = false;
        AddListeners(hologramCopyForTables);

        PlacementOpen(hologramCopyForTables);
    }

    private void DestroyHologramForTables()
    {
        if (hologramCopyForTables != null)
        {
            Destroy(hologramCopyForTables);
        }

        PlacementOpen(hologramCopy);
    }

    /// <summary>
    /// Creates a new Object on the Point of the Hologram
    /// </summary>
    private void CreateObject()
    {
        GameObject createdObject;
        Debug.Log("Create Object");
        if (buildingCharakter.activeMode == SC_For_Mode.Mode.buildingMode ||
            buildingCharakter.activeMode == SC_For_Mode.Mode.chooseBuildingMode)
            if (!blocked)
            {
              createdObject = Instantiate(visual, visualCreationPoint.position, hologramCopy.transform.rotation);
              createdObject.layer = 8;
              if(room != null)
                room.SetItemHolderAsParent(createdObject);
            }
            // if Hologram is blocked, check for TableHologram
            else
            {
                if (hologramCopyForTables == null) return;

                if (hologramCopyForTables.GetComponent<HologramSpawnPoint>().insideFuniture) return;
                var position = visualCreationPoint.position;
                createdObject = Instantiate(visual,
                    new Vector3(position.x, position.y + hightOfObject,
                        position.z), hologramCopyForTables.transform.rotation);
                createdObject.layer = 8;
                if(room != null)
                    room.SetItemHolderAsParent(createdObject);
            }
    }

    /// <summary>
    /// Creates a new Hologram
    /// Adds RigidBody and Hologramspawnpoint
    /// </summary>
    /// <param name="newObjectTransform"></param>
    public void ChangeHologram(GameObject newObjectTransform)
    {
        Debug.Log("ChangeHologram");
        visual = newObjectTransform;
        if (hologramCopy != null)
        {
            Destroy(hologramCopy);
            hologramCopy = new GameObject();
        }

        if (hologramCopyForTables != null)
        {
            Destroy(hologramCopyForTables);
        }

        Transform hologramDummy;

        if (visual.GetComponent(typeof(Funiture)))
        {
            Debug.Log("ChangeHologram has Funiture");
            Funiture funitureobject = visual.gameObject.GetComponent<Funiture>();
            hologramDummy = CreatePreview(funitureobject.funiture.transform);
            hologramCopy = hologramDummy.gameObject;
            hologramCopy.AddComponent(typeof(Rigidbody));
            var rigidbody = hologramCopy.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            hologramCopy.AddComponent(typeof(HologramSpawnPoint));
            if (hologramCopy.GetComponent(typeof(Collider)) != null)
            {
                hologramCopy.GetComponent<Collider>().isTrigger = true;
            }
            else
            {
                hologramCopy.transform.GetChild(0).GetComponent<Collider>().isTrigger = true;
            }

            AddListeners(hologramCopy);
        }
        else //If the Object does not have a Furniture Object
        {
            hologramDummy = CreatePreview(visual.transform);
            hologramCopy = hologramDummy.gameObject;
            hologramCopy.AddComponent(typeof(Rigidbody));
            var rigidbody = hologramCopy.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            hologramCopy.AddComponent(typeof(HologramSpawnPoint));
            if (hologramCopy.GetComponent(typeof(Collider)) != null)
            {
                hologramCopy.GetComponent<Collider>().isTrigger = true;
            }
            else
            {
                hologramCopy.transform.GetChild(0).GetComponent<Collider>().isTrigger = true;
            }

            AddListeners(hologramCopy);
        }
    }

    private void EnteredNormal()
    {
        PlacementBlocked(hologramCopy);
    }

    private void EnteredTable()
    {
        PlacementBlocked(hologramCopyForTables);
    }

    private void ExitNormal()
    {
        PlacementOpen(hologramCopy);
    }

    private void ExitTable()
    {
        PlacementOpen(hologramCopyForTables);
    }


    /// <summary>
    /// Changes to BlockedMaterial
    /// </summary>
    /// <param name="objectItem"></param>
    private void PlacementBlocked(GameObject objectItem)
    {
        Debug.Log("Blocked Color for Copy");
        foreach (var renderer in objectItem.GetComponentsInChildren<Renderer>(true))
            renderer.sharedMaterial = blockedMaterial;

        blocked = true;
    }

    /// <summary>
    /// Changes to GhostMaterial
    /// </summary>
    /// <param name="objectItem"></param>
    private void PlacementOpen(GameObject objectItem)
    {
        Debug.Log("Ghost Color for Copy");
        foreach (var renderer in objectItem.GetComponentsInChildren<Renderer>(true))
            renderer.sharedMaterial = ghostMaterial;

        blocked = false;
    }
    
    private void DisableHologram()
    {
        Debug.Log("Disabled Hologram");
        if (buildingCharakter.activeMode != SC_For_Mode.Mode.buildingMode &&
            buildingCharakter.activeMode != SC_For_Mode.Mode.chooseBuildingMode)
        {
            hologramCopy.gameObject.SetActive(false);
        }
        else
        {
            hologramCopy.gameObject.SetActive(true);
        }
    }
}