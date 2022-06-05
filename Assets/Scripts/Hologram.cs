using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hologram : MonoBehaviour
{
    [SerializeField] private Transform visual;
    private Transform hologramCopy;
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private Material blockedMaterial;
    //[SerializeField] private GridBuildingSystem gridSystem;

    [SerializeField] private Transform hologramReferenzPosition;

    [SerializeField] private HologramSpawnPoint _hologramSpawnPoint;
    [SerializeField] private Transform hologramPivotPosition;
    
    [SerializeField] private Transform xr;

    [SerializeField] private bool blocked;
    
    public PrimaryButtonWatcher watcher;
    

    private void Start()
    {
        watcher.primaryButtonPress.AddListener(CreateObject);
        //objectToProject = this.gameObject;
        //SettingColorToHologram();
        hologramCopy = CreatePreview(visual);

        _hologramSpawnPoint.HologramEnteredFuniture += placementBlocked;
        _hologramSpawnPoint.HologramExitFuniture += placementOpen;
        
    }

    private void Update()
    {
        hologramMover();
    }


    public Transform CreatePreview (Transform aPrefab)
    {
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
            hologramCopy.position = hologramReferenzPosition.position;
            hologramCopy.transform.eulerAngles = new Vector3(hologramReferenzPosition.transform.eulerAngles.x, xr.transform.eulerAngles.y + 90, hologramReferenzPosition.transform.eulerAngles.z);
            //Vector3 newRotation = new Vector3(hologramReferenzPosition.transform.eulerAngles.x, mainCam.transform.eulerAngles.y, hologramReferenzPosition.transform.eulerAngles.z);
            //hologramPivotPosition.transform.eulerAngles = newRotation;
            //hologramCopy.transform.eulerAngles = newRotation;
            
        }
    }
    
    private void CreateObject(bool pressed)
    {

        if (pressed)
        {
            if(!blocked)
            Instantiate(visual, hologramCopy.position, hologramCopy.rotation);
        }
    }

    public void ChangeHologram(Transform newObjectTransform)
    {
        visual = newObjectTransform;
        if (hologramCopy != null)
        {
            Destroy(hologramCopy);
        }
        hologramCopy = CreatePreview(visual);
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

    
}
