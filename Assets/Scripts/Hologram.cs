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
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private Material blockedMaterial;
    //[SerializeField] private GridBuildingSystem gridSystem;

    [SerializeField] private Transform hologramReferenzPosition;
    [SerializeField] private Transform visualCreationPoint;

    [SerializeField] private Transform xr;

    [SerializeField] private bool blocked;
    
    public PrimaryButtonWatcher watcher;
    [SerializeField] private BuildingCharakter buildingCharakter;

    private void Awake()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
    }

    private void Start()
    {
        watcher.primaryButtonPress.AddListener(CreateObject);
        //objectToProject = this.gameObject;
        //SettingColorToHologram();
        ChangeHologram(visual);

        buildingCharakter.ModeChanged += disableHologram;
    }

    private void Update()
    {
        hologramMover();
    }

    private void AddListeners(GameObject hologramSpawnPointObject)
    {
        if (hologramSpawnPointObject.GetComponent(typeof(HologramSpawnPoint)) != null)
        {
            hologramSpawnPointObject.GetComponent<HologramSpawnPoint>().HologramEnteredFuniture += placementBlocked;
            hologramSpawnPointObject.GetComponent<HologramSpawnPoint>().HologramExitFuniture += placementOpen;
        }
       
    }

    public Transform CreatePreview (Transform aPrefab)
    {
        
        Debug.Log("Create Preview");
        
        Transform obj = (Transform)Instantiate(aPrefab);
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
            //90 different for the Transform
            hologramCopy.transform.eulerAngles = new Vector3(eulerAngles.x, xr.transform.eulerAngles.y + 90, eulerAngles.z);
            
        }
    }
    
    private void CreateObject(bool pressed)
    {

        if (pressed)
        {
            Debug.Log("Create Object");
            if(buildingCharakter.activeMode == SC_For_Mode.Mode.buildingMode || buildingCharakter.activeMode == SC_For_Mode.Mode.chooseBuildingMode)
                if (!blocked)
                {
                    Instantiate(visual, visualCreationPoint.position, hologramCopy.transform.rotation);
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

        Transform hologramDummy;
        
        if (visual.GetComponent(typeof(Funiture)))
        {
            Debug.Log("ChangeHologram has Funiture");
            Funiture funitureobject = visual.gameObject.GetComponent<Funiture>();
            hologramDummy = CreatePreview(funitureobject.funiture.transform);
            hologramCopy = hologramDummy.gameObject;
            hologramCopy.AddComponent(typeof(HologramSpawnPoint));
            AddListeners(hologramCopy);
        }
        else
        {
            hologramDummy = CreatePreview(visual.transform);
            hologramCopy = hologramDummy.gameObject;
            hologramCopy.AddComponent(typeof(HologramSpawnPoint));
            AddListeners(hologramCopy);
        }
    }

    private void placementBlocked()
    {
        Debug.Log("Blocked Color for Copy");
        foreach (var renderer in hologramCopy.GetComponentsInChildren<Renderer>(true))
            renderer.sharedMaterial = blockedMaterial;
        
        blocked = true;
    }
    
    private void placementOpen()
    {
        Debug.Log("Ghost Color for Copy");
        foreach (var renderer in hologramCopy.GetComponentsInChildren<Renderer>(true))
            renderer.sharedMaterial = ghostMaterial;

        blocked = false;
    }

    private void disableHologram()
    {
        Debug.Log("Disabled Hologram");
        if (buildingCharakter.activeMode != SC_For_Mode.Mode.buildingMode && buildingCharakter.activeMode != SC_For_Mode.Mode.chooseBuildingMode)
        {
            hologramCopy.gameObject.SetActive(false);
        }
        else
        {
            hologramCopy.gameObject.SetActive(true);
        }
    }

    
}
