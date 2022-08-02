using System;
using System.Collections.Generic;
using System.Diagnostics;
using Photon.Pun.UtilityScripts;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Debug = UnityEngine.Debug;

public class WallScript : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject wallWindow;
    [SerializeField] private GameObject wallDoor;

    [SerializeField] private Material ghostMaterial;

    [SerializeField] private GameObject preview;
    [SerializeField] private GameObject placed;
    [SerializeField] private GameObject objectHolder;

    [SerializeField] private State state = State.Preview;
    public BuildingCharakter buildingCharakter;

    //UI Items
    
    private List<Material> designs;
    [SerializeField] private UITextureHolder texturePrefab;
    [SerializeField] private UIItemHolder wallItemUi;
    [SerializeField] private GameObject canvasTexture;
    [SerializeField] private GameObject canvasItem;
    [SerializeField] private SCList_TypeList wallTypes;
    [SerializeField] private GameObject ui;
    

    private void Start()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();

        buildingCharakter.ModeChanged += WallModeCheck;
     
        ChangePreview(0);
        AddTypesToCanvas(wallTypes);
        WallModeCheck();
        ui.SetActive(false);
    }

    private enum State
    {
        NotVisible,
        Preview,
        PlacedWall,
        PlacedDoor,
        PlacedWindow
    }

    private void WallModeCheck()
    {
        if (buildingCharakter.activeMode == SC_For_Mode.Mode.wallBuildingMode)
        {
            if (state == State.NotVisible)
                state = State.Preview;
            ChangePreview(0);
        }
        else
        {
            ToNotVisible();
        }
    }
    private void GhostTexture(GameObject toGhostTexture)
    {
        Debug.Log("Change to GhostTexture");
        foreach (var renderer in toGhostTexture.GetComponentsInChildren<Renderer>(true))
            renderer.sharedMaterial = ghostMaterial;
    }

    private void ChangePreview(int number)
    {
        if (state != State.Preview)
        {
            return;
        }

        if (number > 2)
        {
            return;
        }

        if (preview != null)
            Destroy(preview);

        var position = this.transform.position;

        preview = number switch
        {
            0 => Instantiate(wall, position, quaternion.identity),
            1 => Instantiate(wallWindow, position, quaternion.identity),
            2 => Instantiate(wallDoor, position, quaternion.identity),
            _ => preview
        };
        GhostTexture(preview);
        preview.transform.parent = objectHolder.transform;
        preview.AddComponent<XRSimpleInteractable>().activated.AddListener(delegate { ChangeWallType(number); });
    }

    private void ChangeWallType(int number)
    {
        if (buildingCharakter.activeMode != SC_For_Mode.Mode.wallBuildingMode)
        {
            return;
        }
        if (number > 2)
        {
            return;
        }

        if (placed != null)
            Destroy(placed);

        if (preview != null)
        {
            Destroy(preview);
        }

        var position = this.transform.position;

        switch (number)
        {
            case 0:
                placed = Instantiate(wall, position, quaternion.identity);
                state = State.PlacedWall;
                break;
            case 1:
                placed = Instantiate(wallWindow, position, quaternion.identity);
                state = State.PlacedWindow;
                break;
            case 2:
                placed = Instantiate(wallDoor, position, quaternion.identity);
                state = State.PlacedDoor;
                break;
        }
        placed.transform.parent = objectHolder.transform;
        placed.AddComponent<XRSimpleInteractable>().activated.AddListener(delegate { ToggleUI(true); });
    }

    private void ToNotVisible()
    {
        if (state != State.Preview) return;

        if (preview != null)
            Destroy(preview);

        state = State.NotVisible;
    }

    public void DestroyWall()
    {
        Destroy(placed);
        Destroy(preview);

        state = State.NotVisible;
    }
    
    // UI Part
    
    public void ToggleUI(bool toggle)
    {
        if (buildingCharakter.activeMode != SC_For_Mode.Mode.wallBuildingMode)
        {
            return;
        }
        ui.SetActive(toggle);
    }

    public void ChangeToCanvasItem()
    {
        canvasItem.SetActive(true);
        canvasTexture.SetActive(false);
        AddTypesToCanvas(wallTypes);
    }
    
    public void ChangeToCanvasTexture()
    {
        canvasItem.SetActive(false);
        canvasTexture.SetActive(true);
        LoadingTextures();
    }
    
    private void AddTypesToCanvas(SCList_TypeList itemList)
    {
        foreach (Transform exampleDelete in canvasItem.transform.GetChild(0).GetChild(0))
        {
            Destroy(exampleDelete.gameObject);
        }

        int counter = 0;
        foreach (var sc_typeItem in itemList.menuItemList)
        {
            UIItemHolder mode = Instantiate(wallItemUi, new Vector3(0, 0, 0), Quaternion.identity);

            mode.transform.SetParent(canvasItem.transform.GetChild(0).GetChild(0), false);

            mode.description.text = sc_typeItem.itemName;

            var counter1 = counter;
            mode.button.onClick.AddListener(delegate { ChangeWallType(counter1); });
            counter++;
        }
    }
    
    public void LoadingTextures()
    {
        Debug.Log("Loading Textures");
        foreach (Transform exampleDelete in canvasTexture.transform.GetChild(0).GetChild(0))
        {
            Destroy(exampleDelete.gameObject);
        }

        int counter = 0;
        foreach (var material in designs)
        {
            UITextureHolder texture = Instantiate(texturePrefab, new Vector3(0, 0, 0), Quaternion.identity);

            texture.transform.SetParent(canvasTexture.transform.GetChild(0).GetChild(0), false);

            texture.title.text = material.name;

            texture.number = counter;
           
            texture.button.onClick.AddListener(delegate { SwapDesign(texture.number); });
            counter++;
        }
    }
    
    public void SwapDesign(int number)
    {
        Debug.Log("SwapDesign: " + number);
        if (designs.Count > number)
        {
            if (objectHolder.transform.childCount > 0)
            {
                Renderer render = objectHolder.transform.GetChild(0).GetComponent<Renderer>();

                render.sharedMaterial = designs[number];
            }
        }
    }
}