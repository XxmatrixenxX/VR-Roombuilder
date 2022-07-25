using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hologram : MonoBehaviour
{
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
    [SerializeField] private BuildingCharakter buildingCharakter;

    private void Awake()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
    }

    private void Start()
    {
        watcher.primaryButtonPress.AddListener(CreateObject);
        ChangeHologram(visual);

        buildingCharakter.ModeChanged += DisableHologram;
    }

    private void Update()
    {
        hologramMover();
        HologramForTablesMover();
    }

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
                //for Basic Funiture
                spawnpoint.HologramEnteredFuniture += EnteredNormal;
                spawnpoint.HologramExitFuniture += ExitNormal;
                //for Tables
                spawnpoint.HologramEnteredFunitureTable += FunitureWithPlacementArea;
                spawnpoint.HologramExitFunitureTable += DestroyHologramForTables;
            }
        }
    }

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

    public void hologramMover()
    {
        if (hologramCopy != null)
        {
            hologramCopy.transform.position = hologramReferenzPosition.position;
            var eulerAngles = hologramReferenzPosition.transform.eulerAngles;
            //Normal if Grabable
            if (visual.GetComponent(typeof(FunitureGrabable)) != null)
            {
                hologramCopy.transform.eulerAngles =
                    new Vector3(eulerAngles.x, xr.transform.eulerAngles.y, eulerAngles.z);
            }
            //90 different for the Transform
            else
                hologramCopy.transform.eulerAngles =
                new Vector3(eulerAngles.x, xr.transform.eulerAngles.y + 90, eulerAngles.z);
        }
    }

    public void FunitureWithPlacementArea(FunitureWithPlaceArea item)
    {
        hightOfObject = item.gettingHightOfFuniture();
        CreateHologramForTables();
        PlacementBlocked(hologramCopy);
    }

    //Creates a Visual on Top of the Table
    public void HologramForTablesMover()
    {
        if (blocked)
        {
            if (hologramCopyForTables != null)
            {
                var position = hologramReferenzPosition.position;
                hologramCopyForTables.transform.position = new Vector3(position.x, position.y + hightOfObject, position.z);
                var eulerAngles = hologramReferenzPosition.transform.eulerAngles;
                //Normal if Grabable
                if (visual.GetComponent(typeof(FunitureGrabable)) != null)
                {
                    hologramCopy.transform.eulerAngles =
                        new Vector3(eulerAngles.x, xr.transform.eulerAngles.y, eulerAngles.z);
                }
                //90 different for the Transform
                else
                    hologramCopyForTables.transform.eulerAngles =
                    new Vector3(eulerAngles.x, xr.transform.eulerAngles.y + 90, eulerAngles.z);
            }
        }
    }

    //Clones HologramCopy for a similar Object 
    public void CreateHologramForTables()
    {
        hologramCopyForTables = Instantiate(hologramCopy);
        hologramCopyForTables.GetComponent<HologramSpawnPoint>().SetToOnTable();
        hologramCopyForTables.GetComponent<HologramSpawnPoint>().insideFuniture = false;
        AddListeners(hologramCopyForTables);
        
       PlacementOpen(hologramCopyForTables);
    }

    public void DestroyHologramForTables()
    {
        if (hologramCopyForTables != null)
        {
            Destroy(hologramCopyForTables);
        }

        PlacementOpen(hologramCopy);
    }


    private void CreateObject(bool pressed)
        {
            if (pressed)
            {
                Debug.Log("Create Object");
                if (buildingCharakter.activeMode == SC_For_Mode.Mode.buildingMode ||
                    buildingCharakter.activeMode == SC_For_Mode.Mode.chooseBuildingMode)
                    if (!blocked)
                    {
                        Instantiate(visual, visualCreationPoint.position, hologramCopy.transform.rotation);
                    }
                    else
                    {
                        if (hologramCopyForTables == null) return;

                        if (hologramCopyForTables.GetComponent<HologramSpawnPoint>().insideFuniture) return;
                        var position = visualCreationPoint.position;
                        Instantiate(visual,
                            new Vector3(position.x, position.y + hightOfObject,
                                position.z), hologramCopyForTables.transform.rotation);
                    }
            }
        }

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
                if(hologramCopy.GetComponent(typeof(Collider)) != null)
                {
                    hologramCopy.GetComponent<Collider>().isTrigger = true;
                }
                else
                {
                    hologramCopy.transform.GetChild(0).GetComponent<Collider>().isTrigger = true;
                }
                AddListeners(hologramCopy);
            }
            else //If the Object does not have a Funiture Object
            {
                hologramDummy = CreatePreview(visual.transform);
                hologramCopy = hologramDummy.gameObject;
                hologramCopy.AddComponent(typeof(Rigidbody));
                var rigidbody = hologramCopy.GetComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                hologramCopy.AddComponent(typeof(HologramSpawnPoint));
                if(hologramCopy.GetComponent(typeof(Collider)) != null)
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
        

        private void PlacementBlocked(GameObject objectItem)
        {
            Debug.Log("Blocked Color for Copy");
            foreach (var renderer in objectItem.GetComponentsInChildren<Renderer>(true))
                renderer.sharedMaterial = blockedMaterial;

            blocked = true;
        }

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