using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomRoofHandler : MonoBehaviour
{
    //Todo Add Roomfunction
    
    public GameObject roof;
    public GameObject bottom;
    public RoomScript room;

    [SerializeField]
    private List<Material> materialList;

    public GameObject roofCanvas;
    public GameObject bottomCanvas;

    public GameObject roofTexture;
    public GameObject bottomTexture;
    
    [SerializeField] private UITextureHolder texturePrefab;

    private void Awake()
    {
        ChangeCanvasToRoof();
    }

    public void ChangeCanvasToRoof()
    {
        roofCanvas.SetActive(true);
        bottomCanvas.SetActive(false);
        LoadingTexturesBottomRoof(1);
    }
    
    public void ChangeCanvasToBottom()
    {
        roofCanvas.SetActive(false);
        bottomCanvas.SetActive(true);
        LoadingTexturesBottomRoof(2);
    }

    public void CloseCanvas()
    {
        roofCanvas.SetActive(false);
        bottomCanvas.SetActive(false);
    }

    public void ToggleRoof(Toggle value)
    {
        if (value)
        {
            roof.gameObject.SetActive(true);
            room.roofActivated = true;
        }
        if(!value)
        {
            roof.gameObject.SetActive(false); 
            room.roofActivated = false;
        }
    }
    
    
    public void LoadingTexturesBottomRoof(int number)
    {
        switch (number)
        {
            case 1 :
                FillingTextures(roofTexture, 1);
                break;
            case 2 :
                FillingTextures(bottomTexture, 2);
                break;
        }
    }

    public void FillingTextures(GameObject textureObject, int number)
    {
        Debug.Log("Loading Textures");
        foreach (Transform exampleDelete in textureObject.transform.GetChild(0).GetChild(0))
        {
            Destroy(exampleDelete.gameObject);
        }

        int counter = 0;
        foreach (var material in materialList)
        {
            UITextureHolder texture = Instantiate(texturePrefab, new Vector3(0, 0, 0), Quaternion.identity);

            texture.transform.SetParent(textureObject.transform.GetChild(0).GetChild(0), false);

            texture.title.text = material.name;

            texture.number = counter;

            if (number == 1)
            { 
                texture.button.onClick.AddListener(delegate { SwapDesignRoof(texture.number); });
            }
            else
            {
                texture.button.onClick.AddListener(delegate { SwapDesignBottom(texture.number); });
            }
            counter++;
        }
    }
    
    public void SwapDesignRoof(int number)
    {
        Debug.Log("SwapDesign: " + number);
        if (materialList.Count > number)
        {
            if (roof.transform.GetComponent<Renderer>())
            {
                Renderer render = roof.transform.GetComponent<Renderer>();

                render.sharedMaterial = materialList[number];
            }
        }
    }
    public void SwapDesignBottom(int number)
    {
        Debug.Log("SwapDesign: " + number);
        if (materialList.Count > number)
        {
            if (bottom.transform.GetComponent<Renderer>())
            {
                Renderer render = bottom.transform.GetComponent<Renderer>();

                render.sharedMaterial = materialList[number];
            }
        }
    }
    
    
}
