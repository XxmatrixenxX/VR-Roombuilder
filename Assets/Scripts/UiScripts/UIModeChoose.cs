using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModeChoose : MonoBehaviour
{
    [SerializeField] private SCList_Mode modeList;

    [SerializeField] private UIModeHolder modeUI;

    [SerializeField] private GameObject canvasCollectionOfModes;


    [SerializeField] private BuildingCharakter buildingCharakter;


    private void Awake()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
        AddModeToCollection();
    }
    
    public void LoadingCanvas()
    {
        AddModeToCollection();
    }

    public void AddModeToCollection()
    {
        foreach (Transform exampleDelete in canvasCollectionOfModes.transform)
        {
            Destroy(exampleDelete.gameObject);
        }
        
        foreach (var sc_mode in modeList.modeList)
        {
            UIModeHolder mode = Instantiate(modeUI, new Vector3(0, 0, 0), Quaternion.identity);

            mode.transform.SetParent(canvasCollectionOfModes.transform, false);

            mode.mode = sc_mode.mode;
            mode.description.text = sc_mode.description;
            mode.title.text = sc_mode.modeString;
            mode.button.onClick.AddListener(delegate { ButtonEvent(mode.mode); });
        }
    }

    public void ButtonEvent(SC_For_Mode.Mode mode)
    {
        buildingCharakter.ChangeMode(mode);
    }
}