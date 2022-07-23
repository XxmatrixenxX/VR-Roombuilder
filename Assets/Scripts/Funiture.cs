using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Funiture : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 360f)]
    public float directionRange = 0;
    public Text rotationText;

    private float sizeSteps = 0.1f;
    
    [SerializeField]
    [Range(0.1f, 2f)]
    private float size = 1;

    [SerializeField] private Text sizeText;
    

    private Vector3 location;

    [SerializeField] private GameObject uiObject;

    public GameObject funiture;

    [SerializeField] private GameObject objectHolder;

    [SerializeField] private List<Material> designs;

    [SerializeField] private Slider sizeSlider;
    public Slider rotationSlider;

    [SerializeField] private UITextureHolder texturePrefab;

    [SerializeField] private GameObject uiScaleCanvas;
    [SerializeField] private GameObject uiTextureCanvas;

    public BuildingCharakter buildingCharakter;

    [SerializeField] private String funitureString = "Funiture";
    
    

    public Collider funitureCollider;

    private void Start()
    {
       StartOptions();
    }

    private void Awake()
    {
    }

    public virtual void StartOptions()
    {
        funitureCollider = funiture.gameObject.GetComponent<Collider>();
        SliderStartPosition();
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
        buildingCharakter.SelectedNewItem += SelectedNewItem;
        ChangeTagOfChild();
        Accepted();
    }

    public void DestroyThisFuniture()
    {
        Debug.Log("Destroy Funiture");
        Destroy(gameObject);
    }

    public void SelectedNewItem(GameObject selectedGameObject)
    {
        Debug.Log("Selected a new item");
        
        if (selectedGameObject.Equals(gameObject))
        {
            UiActive();
            ChangeCanvasToScaling();
        }
        else
        {
            Accepted();
        }
    }

    private void UiActive()
    {
        uiObject.SetActive(true);
    }

    public void Accepted() 
    {
        if(uiObject != null)
            uiObject.SetActive(false);
    }
    
    /// <summary>
    /// Changes Sizes of Object
    /// </summary>
    private void SetSize(float size)
    {
        Debug.Log("Changed Size of funiture");
        funiture.transform.localScale = new Vector3(size, size, size);
    }

    public void SliderStartPosition()
    {
        Debug.Log("SliderStartPosition: Funiture: SliderPosition Size: "+ size + " RotationPosition Rotation" + directionRange);
        sizeSlider.value = size * 10;
        if (funiture.transform.eulerAngles.y > 360)
        {
            directionRange = 45;
        }
        else
            directionRange = funiture.transform.eulerAngles.y ;
        rotationSlider.value = directionRange;
        SliderSize(sizeSlider);
        SliderRotation(rotationSlider);
    }

    public void SliderSize(Slider size)
    {
        Debug.Log("Scale Slider adjusted");
        this.size = size.value * sizeSteps;
        SetSize(this.size);
        ChangeSizeText();
    }

    public void ChangeSizeText()
    {
        Debug.Log("Size Text adjusted");
        sizeText.text = size.ToString(CultureInfo.InvariantCulture);
    }

    public void SelectedThisItemForSettings(GameObject gameObject)
    {
        Debug.Log("SelectedThisItemForSettings: Playermode: " +buildingCharakter.activeMode);
       if(buildingCharakter.activeMode == SC_For_Mode.Mode.buildingMode)
        buildingCharakter.InvokeSelectedNewItem(gameObject);
    }

    public void ChangeCanvasToScaling()
    {
        Debug.Log("ChangeCanvasToScaling Canvas");
        uiScaleCanvas.SetActive(true);
        uiTextureCanvas.SetActive(false);
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

    public void LoadingTextures()
    {
        Debug.Log("Loading Textures");
        foreach (Transform exampleDelete in uiTextureCanvas.transform.GetChild(0).GetChild(0))
        {
            Destroy(exampleDelete.gameObject);
        }

        int counter = 0;
        foreach (var material in designs)
        {
            UITextureHolder texture = Instantiate(texturePrefab, new Vector3(0, 0, 0), Quaternion.identity);

            texture.transform.SetParent(uiTextureCanvas.transform.GetChild(0).GetChild(0), false);

            texture.title.text = material.name;

            texture.number = counter;
           
            texture.button.onClick.AddListener(delegate { SwapDesign(texture.number); });
            counter++;
        }
    }

    public void ChangeCanvasToTexture()
    {
        Debug.Log("ChangeCanvasToTexture Canvas");
        uiScaleCanvas.SetActive(false);
        uiTextureCanvas.SetActive(true);
        LoadingTextures();
    }

    public void ChangeRotationText()
    {
        rotationText.text = (directionRange -180).ToString(CultureInfo.InvariantCulture);
        Debug.Log("Rotation Text Changed: directionRange: " + directionRange +"/ rotation Text: " + rotationText.text);
    }
    
    /// <summary>
    /// Rotate the Object
    /// </summary>
    public void SetTransformRotation()
    {
        funiture.transform.rotation = Quaternion.Euler(0, directionRange, 0);
        Debug.Log("Funiture rotated Quaternion: " + funiture.transform.rotation );  
    }
    
    public void SliderRotation(Slider rotation)
    {
        Debug.Log("SliderRotation changed RotationValue: " + rotation.value);
        directionRange = rotation.value;
        SetTransformRotation();
        ChangeRotationText();
    }
    
    public void ChangeTagOfChild()
    {
        Debug.Log("Changed Tag of all children");
        foreach (Transform transform in gameObject.transform)
        {
            transform.tag = funitureString;
        }
    }
    
    
    
    
}
