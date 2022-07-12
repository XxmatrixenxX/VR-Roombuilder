using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class Funiture : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 360f)]
    public float directionRange;
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

    [SerializeField] private BuildingCharakter buildingCharakter;
    
    

    public Collider funitureCollider;

    private void Start()
    {
        SliderStartPosition();
        buildingCharakter.SelectedNewItem += SelectedNewItem;
    }

    private void Awake()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
        Accepted();
    }

    public void DestroyThisFuniture()
    {
        Destroy(this.gameObject);
    }

    public void SelectedNewItem(GameObject selectedGameObject)
    {
        if (selectedGameObject.Equals(this.gameObject))
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
        uiObject.SetActive(false);
    }

    /// <summary>
    /// Calculate the Float for the Range
    /// </summary>
    /// <param name="direction">Parameter for the direction the Funiture show to at the Moment</param>
    /// <param name="difference">Parameter for turn amount, should not greater then 360f and not smaller then -360f</param>
    private float CalculateDirection(float direction, float difference)
    {
        float result = direction + difference;

        if (result > 360f)
        {
            return result - 360f;
        }

        if (result == 0f)
        {
            return 0f;
        }
        return 360f + result;
    }

    /// <summary>
    /// Rotate the Object
    /// </summary>
    public void SetTransformRotation()
    {
        
            funiture.transform.rotation = Quaternion.Euler(0, directionRange, 0);
    }
    
    /// <summary>
    /// Changes Size of Object
    /// </summary>
    /// <param name="bigger"> If true it will be bigger.</param>
    private void ChangeSize(bool bigger)
    {
        if (bigger)
        {
            directionRange = CalculateSize(directionRange,sizeSteps );
        }
        else 
        {
            directionRange = CalculateSize(directionRange, -sizeSteps);
        }
    }
    
    /// <summary>
    /// Calculate the Float for the Range
    /// </summary>
    /// <param name="size">Parameter for the Size of the Funiture</param>
    /// <param name="sizeDifference">Parameter for size amount, should not greater then 10f and not smaller then -10f</param>
    private float CalculateSize(float size, float sizeDifference)
    {
        float result = size + sizeDifference;

        if (result > 2f)
        {
            return result - 2f;
        }

        if (result == 0.1f)
        {
            return 0.1f;
        }
        return 2f + result;
    }

    /// <summary>
    /// Changes Sizes of Object
    /// </summary>
    private void SetSize(float size)
    {
        funiture.transform.localScale = new Vector3(size, size, size);
    }

    public void SliderStartPosition()
    {
        Debug.Log("Funiture: SliderPosition Size: "+ size);
        sizeSlider.value = size * 10;
        rotationSlider.value = directionRange;
        SliderSize(sizeSlider);
        SliderRotation(rotationSlider);
    }

    public void SliderSize(Slider size)
    {
        this.size = size.value * sizeSteps;
        SetSize(this.size);
        ChangeSizeText();
    }
    
    public void SliderRotation(Slider rotation)
    {
        this.directionRange = rotation.value -180;
        SetTransformRotation();
        ChangeRotationText();
    }

    public void ChangeSizeText()
    {
        sizeText.text = size.ToString(CultureInfo.InvariantCulture);
    }

    public void ChangeRotationText()
    {
        rotationText.text = (directionRange).ToString(CultureInfo.InvariantCulture);
    }

    public void SelectedThisItemForSettings(GameObject gameObject)
    {
        Debug.Log("SelectedThisItemForSettings: Playermode: " +buildingCharakter.activeMode);
       if(buildingCharakter.activeMode == SC_For_Mode.Mode.buildingMode)
        buildingCharakter.InvokeSelectedNewItem(gameObject);
    }

    public void ChangeCanvasToTexture()
    {
        Debug.Log("ChangeCanvasToTexture Canvas");
        uiScaleCanvas.SetActive(false);
        uiTextureCanvas.SetActive(true);
        LoadingTextures();
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
    
    
    
    
}
