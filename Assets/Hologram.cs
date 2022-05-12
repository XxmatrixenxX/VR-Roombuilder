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
    [SerializeField] public Transform objectToProject;
    private Transform hologramCopy;
    [SerializeField] private Material ghostMaterial;
    //[SerializeField] private GridBuildingSystem gridSystem;

    [SerializeField] private Transform hologramReferenzPosition;
    [SerializeField] private Transform hologramPivotPosition;
    
    [SerializeField] private Transform xr;
    
    public PrimaryButtonWatcher watcher;

    int interval = 1; 
    float nextTime = 0;

    private void Start()
    {
        watcher.primaryButtonPress.AddListener(CreateObject);
        //objectToProject = this.gameObject;
        //SettingColorToHologram();
        hologramCopy = CreatePreview(visual);

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
    
    // private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    // {
    //     RefreshVisual();
    // }

    private void SetObjectToProject(GameObject gameObject)
    {
        objectToProject = gameObject.transform;
        SettingColorToHologram();
    }

    //Wird nach Update aufgerufen
    // private void LateUpdate()
    // {
    //     if (Time.deltaTime >= nextTime)
    //     {
    //
    //         gridSystem.grid.GetXY(this.gameObject.transform.position, out int x, out int z);
    //         Vector3 targetPosition = gridSystem.grid.GetWorldPosition(x, z);
    //         targetPosition.y = 1f;
    //         visual.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
    //         // transform.rotation =
    //         //     Quaternion.Lerp(transform.rotation, transform.rotation, Time.deltaTime * 15f);
    //         //
    //         nextTime += interval; 
    //     }
    // }

    private void RefreshVisual()
    {
        visual = Instantiate(hologramCopy.transform, Vector3.zero, Quaternion.identity);
        visual.parent = transform;
        visual.localPosition = Vector3.zero;
        //visual.localEulerAngles = Vector3.zero;
    }

    private void SettingColorToHologram()
    {
        //hologramCopy = objectToProject;
        //RepeatUntilNoChildrenfound(hologramCopy.transform);
        //RefreshVisual();
    }

    //Gets Children of a Transform
    private List<Transform> GetChildren(Transform parent)
    {
       List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        return children;
    }

    private bool HasChildren(Transform parent)
    {
        if (parent.childCount > 0)
            return true;
        return false;
    }

    //Gets all Children and sets all Materials to ghost
    private void RepeatUntilNoChildrenfound(Transform parent)
    {
        //Liste um die Children zu speichern
        List<Transform> childrenOfParent = new List<Transform>();
        //Falls Kind gefunden wurde geht es weiter
        bool childrenfound = true;
        //Solange kein Kind gefunden wurde, bleibt es true
        bool noChild = true;

        childrenfound = HasChildren(parent);
        
        //läuft immer weiter bis es keine Children findet
        while (childrenfound)
        {
            
           childrenOfParent = GetChildren(parent);
            
           //Wenn es ein Kind gefunden hat
           if (childrenOfParent.Any())
           {
               noChild = false;
               //für jedes Kind wird diese Methode aufgerufen
               foreach (Transform child in childrenOfParent)
               {
                   //jedes Object wird durchsichtig gemacht
                   if (child.TryGetComponent(out Renderer childRenderer))
                   {
                       childRenderer.material = ghostMaterial;
                   }
                   if(HasChildren(child))
                    RepeatUntilNoChildrenfound(child);
               }
           }
           
           // Falls kein Kind gefunden wurde wird die Schleife gebrochen
           if (noChild)
           {
               childrenfound = false;
           }
        }
    }

    
    // public void GetItemGrabbed(GameObject grabbed)
    // {
    //     SetObjectToProject(grabbed);
    // }

    private void CreateObject(bool pressed)
    {

        if (pressed)
        {
            Instantiate(visual, hologramCopy.position, hologramCopy.rotation);
        }
    }
}
