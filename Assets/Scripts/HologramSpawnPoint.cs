using System;

using UnityEngine;

public class HologramSpawnPoint : MonoBehaviour
{
    
    public event Action HologramEnteredFuniture;
    public event Action HologramExitFuniture;
    public event Action<FunitureWithPlaceArea> HologramEnteredFunitureTable;
    public event Action HologramExitFunitureTable;

    public void InvokeHologramEnteredFuniture() => HologramEnteredFuniture?.Invoke();
    
    public void InvokeHologramExitFuniture() => HologramExitFuniture?.Invoke();
    
    public void InvokeHologramEnteredFunitureTable(FunitureWithPlaceArea item) => HologramEnteredFunitureTable?.Invoke(item);
    
    public void InvokeHologramExitFunitureTable() => HologramExitFunitureTable?.Invoke();

    public bool insideFuniture = false;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected");
        if (collision.CompareTag("Funiture"))
        {
            Debug.Log("Entered Funiture");
            if (!insideFuniture)
            {
                Debug.Log("Entered Funiture first Time");
                InvokeHologramEnteredFuniture();
                insideFuniture = true;
            }
        }
        if (collision.CompareTag("FuniturePlacementArea"))
        {
            Debug.Log("Entered Funiture");
            if (!insideFuniture)
            {
                Debug.Log("Entered Funiture first Time");
                InvokeHologramEnteredFunitureTable(collision.transform.root.gameObject.GetComponent<FunitureWithPlaceArea>());
                insideFuniture = true;
            }
        }
    }
    
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Funiture"))
        {
            Debug.Log("Inside Funiture");
            if (!insideFuniture)
            {
                Debug.Log("Inside Funiture");
                InvokeHologramEnteredFuniture();
                insideFuniture = true;
            }
        }
        if (collision.CompareTag("FuniturePlacementArea"))
        {
            Debug.Log("Inside FunitureTable");
            if (!insideFuniture)
            {
                Debug.Log("Inside FunitureTable");
                InvokeHologramEnteredFunitureTable(collision.transform.root.gameObject.GetComponent<FunitureWithPlaceArea>());
                insideFuniture = true;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Collision end");
        if (collision.CompareTag("Funiture"))
        {
            Debug.Log("Exited Funiture");
            if (insideFuniture)
            {
                Debug.Log("Exited Funiture first Time");
                InvokeHologramExitFunitureTable();
                insideFuniture = false;
            }
        }
        if (collision.CompareTag("FuniturePlacementArea"))
        {
            Debug.Log("Exited Funiture");
            if (insideFuniture)
            {
                Debug.Log("Exited Funiture first Time");
                InvokeHologramExitFunitureTable();
                insideFuniture = false;
            }
        }
    }
    
}
