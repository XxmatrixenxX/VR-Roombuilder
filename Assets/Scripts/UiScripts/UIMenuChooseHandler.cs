using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuChooseHandler : MonoBehaviour
{
    [SerializeField] private SCList_Menu typeList;

    [SerializeField] private UIMenuItem menuItem;

    [SerializeField] private UIItemHolder typeItem;
    
    [SerializeField] private GameObject canvasCollectionOfType;

    [SerializeField] private GameObject canvasObjectsOfChoosedType;

    [SerializeField] private BuildingCharakter buildingCharakter;
    
    [SerializeField] private Hologram hologram;

    private void Awake()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();

        hologram = FindObjectOfType<Hologram>();
        
        AddTypeToCollection();
    }

    public void LoadingCanvas()
    {
        AddTypeToCollection();
    }
    
    public void AddTypeToCollection()
    {
        
        foreach (Transform exampleDelete in canvasCollectionOfType.transform)
        {
            GameObject.Destroy(exampleDelete.gameObject);
        }
        
        foreach (var sc_menu in typeList.menuList)
        {
            UIMenuItem menu = Instantiate(menuItem, new Vector3(0, 0, 0), Quaternion.identity);

            menu.transform.SetParent(canvasCollectionOfType.transform, false);
            
            menu.title.text = sc_menu.ListTypeName;
            menu.button.onClick.AddListener(delegate { ClickedType(sc_menu.menuItemList); });
        }
    }

    public void ClickedType(SC_For_Menu[] itemList)
    {
        AddTypesToCollection(itemList);
    }

    public void AddTypesToCollection(SC_For_Menu[] itemList)
    {
        foreach (Transform exampleDelete in canvasObjectsOfChoosedType.transform)
        {
            Destroy(exampleDelete.gameObject);
        }
        
        foreach (var sc_typeItem in itemList)
        {
            UIItemHolder mode = Instantiate(typeItem, new Vector3(0, 0, 0), Quaternion.identity);

            mode.transform.SetParent(canvasObjectsOfChoosedType.transform, false);

            mode.description.text = sc_typeItem.description;
            if(!sc_typeItem.itemName.Equals(null))
                mode.title.text = sc_typeItem.itemName;
            mode.button.onClick.AddListener(delegate { ClickedTypeItem(sc_typeItem.item); });
        }
    }

    public void ClickedTypeItem(GameObject item)
    {
        buildingCharakter.ChangeMode(SC_For_Mode.Mode.buildingMode);
        hologram.ChangeHologram(item.transform);
    }
}