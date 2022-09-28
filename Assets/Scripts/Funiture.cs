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

    public GameObject objectHolder;

    [SerializeField] private List<Material> designs;

    [SerializeField] private Slider sizeSlider;
    public Slider rotationSlider;

    [SerializeField] private UITextureHolder texturePrefab;

    [SerializeField] private GameObject uiScaleCanvas;
    [SerializeField] private GameObject uiTextureCanvas;

    public BuildingCharakter buildingCharakter;
    
    public VrInputManager inputManager;

    [SerializeField] private String funitureString = "Funiture";
    
    

    public Collider funitureCollider;

    private void Start()
    {
        if (FindObjectOfType<BuildingCharakter>() != null)
        {
            StartOptions(); 
        }
    }

    private void Awake()
    {
    }

    public virtual void StartOptions()
    {
        funitureCollider = funiture.gameObject.GetComponent<Collider>();
        SliderStartPosition();
        inputManager = FindObjectOfType<VrInputManager>();
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
        buildingCharakter.SelectedNewItem += SelectedNewItem;
        ChangeTagOfChild();
        Accepted();
    }

    public void DestroyThisFurniture()
    {
        Debug.Log("Destroy Furniture");
        this.transform.parent.parent.GetComponent<RoomScript>().RemoveItemFromList(this.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// If GameObject is this, activate the UI
    /// If its a other GameObject close the UI
    /// </summary>
    private void SelectedNewItem(GameObject selectedGameObject)
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

    /// <summary>
    /// Sets Start Position of the Sliders
    /// Activate the Slider Methods
    /// </summary>
    /// Todo find a Way to make the same rotation for GrabFurniture
    private void SliderStartPosition()
    {
        Debug.Log("SliderStartPosition: Furniture: SliderPosition Size: "+ size + " RotationPosition Rotation" + directionRange);
        // Whole Numbers are better readable then 0.1 to 2 so it needs to be multiplied  
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

    private void ChangeSizeText()
    {
        Debug.Log("Size Text adjusted");
        sizeText.text = size.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// If the Object is Selected, Invoke Method of building Character
    /// </summary>
    public void SelectedThisItemForSettings(GameObject gameObject)
    {
        Debug.Log("SelectedThisItemForSettings: Playermode: " +buildingCharakter.activeMode);
       if(buildingCharakter.activeMode == SC_For_Mode.Mode.buildingMode)
        buildingCharakter.InvokeSelectedNewItem(gameObject);
    }

    /// <summary>
    /// Activate UIScalingCanvas
    /// </summary>
    public void ChangeCanvasToScaling()
    {
        Debug.Log("ChangeCanvasToScaling Canvas");
        uiScaleCanvas.SetActive(true);
        uiTextureCanvas.SetActive(false);
    }
    
    /// <summary>
    /// Gets the Renderer of the Funiture
    /// Changes the material to the Design of the Number
    /// </summary>
    private void SwapDesign(int number)
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

    /// <summary>
    /// Delete all Example Data out of the Scrollbar
    /// Then Create a UITextureHolder for each Material
    /// Fill UITextureHolder with Data
    /// Add Object to Scrollbar
    /// Add Method to Button
    /// </summary>
    private void LoadingTextures()
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

    /// <summary>
    /// Change the Text of the Rotation
    /// Value should be between -180 to 180 
    /// </summary>
    private void ChangeRotationText()
    {
        rotationText.text = (directionRange -180).ToString(CultureInfo.InvariantCulture);
        Debug.Log("Rotation Text Changed: directionRange: " + directionRange +"/ rotation Text: " + rotationText.text);
    }
    
    /// <summary>
    /// Rotate the Object
    /// </summary>
    private void SetTransformRotation()
    {
        funiture.transform.rotation = Quaternion.Euler(0, directionRange, 0);
        Debug.Log("Funiture rotated Quaternion: " + funiture.transform.rotation );  
    }
    
    /// <summary>
    /// Get Data of Slider and save it in directionRange
    /// Calls Methods for Rotation
    /// </summary>
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
