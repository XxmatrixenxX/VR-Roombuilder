using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInHand : MonoBehaviour
{
    [SerializeField] private GameObject itemSelected;

    [SerializeField] private GameObject itemInHand;

    [SerializeField] private GameObject itemInHandPosition;

    [SerializeField] private Hologram hologramObject;
    

    
    private void SetItemSelected(GameObject ItemToHand)
    {
        itemSelected = ItemToHand;
        InstantiateItemAtPosition();
    }

    private void InstantiateItemAtPosition()
    {
        if (itemInHand != null)
        {
            Destroy(itemInHand);
            itemInHand = new GameObject();
        }

        itemInHand = Instantiate(itemSelected, itemInHandPosition.transform.position, Quaternion.identity).gameObject;
        
        //Hologram Changed
        hologramObject.ChangeHologram(itemSelected);
    }
    
    private void StartBuildMode()
    {
        itemInHand.SetActive(true);
    }
    
    private void LeaveBuildMode()
    {
        itemInHand.SetActive(false);
    }
    
}
