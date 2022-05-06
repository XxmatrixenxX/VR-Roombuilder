using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hologram : MonoBehaviour
{
    private Transform visual;
    [SerializeField]private GameObject objectToProject;
    private GameObject hologramCopy;
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private GridBuildingSystem gridSystem;
    
    int interval = 1; 
    float nextTime = 0;

    private void Start()
    {
        objectToProject = this.gameObject;
        SettingColorToHologram();

    }

    // private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    // {
    //     RefreshVisual();
    // }

    private void SetObjectToProject(GameObject gameObject)
    {
        objectToProject = gameObject;
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
        hologramCopy = objectToProject;
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

   
}
