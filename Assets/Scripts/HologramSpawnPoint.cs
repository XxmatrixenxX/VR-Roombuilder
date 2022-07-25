using System;

using UnityEngine;

public class HologramSpawnPoint : MonoBehaviour
{
    
    public event Action HologramEnteredFuniture;
    public event Action HologramExitFuniture;
    public event Action<FunitureWithPlaceArea> HologramEnteredFunitureTable;
    public event Action HologramExitFunitureTable;

    public event Action TopHologramEnteredFuniture;

    public event Action TopHologramExit;

    public void InvokeHologramEnteredFuniture() => HologramEnteredFuniture?.Invoke();
    
    public void InvokeHologramExitFuniture() => HologramExitFuniture?.Invoke();
    
    public void InvokeHologramEnteredFunitureTable(FunitureWithPlaceArea item) => HologramEnteredFunitureTable?.Invoke(item);
    
    public void InvokeHologramExitFunitureTable() => HologramExitFunitureTable?.Invoke();

    public void InvokeTopHologramExit() => TopHologramExit?.Invoke();

    public void InvokeTopHologramEntered() => TopHologramEnteredFuniture?.Invoke();
    

    public bool insideFuniture = false;

    public bool OnTable = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (OnTable)
        {
            if (collision.CompareTag("Funiture") || collision.CompareTag("FuniturePlacementArea") || collision.CompareTag("GrabableFuniture") || collision.CompareTag("SitableFuniture"))
            {
                if (!insideFuniture)
                {
                    EnteredOnTable();
                }
            }
        }
        
        else
        {
            Debug.Log("Collision detected");
            if (collision.CompareTag("Funiture"))
            {
                Debug.Log("Entered Funiture");
                if (!insideFuniture)
                {
                    EnteredFuniture();
                }
            }
            if (collision.CompareTag("FuniturePlacementArea"))
            {
                Debug.Log("Entered Funiture");
                if (!insideFuniture)
                {
                    EnteredTableFuniture(collision);
                }
            }
        }
    }

    public void SetToOnTable()
    {
        OnTable = true;
    }

    public void SetToOffTable()
    {
        OnTable = false;
    }
    
    private void OnTriggerStay(Collider collision)
    {
        if (OnTable)
        {
            if (collision.CompareTag("Funiture") || collision.CompareTag("FuniturePlacementArea") ||
                collision.CompareTag("GrabableFuniture") || collision.CompareTag("SitableFuniture"))
            {
                if (!insideFuniture)
                {
                    EnteredOnTable();
                }
            }
        }
        else
        {
            if (collision.CompareTag("Funiture"))
            {
                Debug.Log("Inside Funiture");
                if (!insideFuniture)
                {
                    EnteredFuniture();
                }
            }

            if (collision.CompareTag("FuniturePlacementArea"))
            {
                Debug.Log("Inside FunitureTable");
                if (!insideFuniture)
                {
                    EnteredTableFuniture(collision);
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (OnTable)
        {
            if (collision.CompareTag("Funiture") || collision.CompareTag("FuniturePlacementArea") ||
                collision.CompareTag("GrabableFuniture") || collision.CompareTag("SitableFuniture"))
            {
                if (!insideFuniture)
                {
                    ExitedOnTable();
                }
            }
        }
        else
        {
            Debug.Log("Collision end");
            if (collision.CompareTag("Funiture"))
            {
                Debug.Log("Exited Funiture from Funiture");
                if (insideFuniture)
                {
                    Exited();
                }
            }

            if (collision.CompareTag("FuniturePlacementArea"))
            {
                Debug.Log("Exited Funiture from Placement");
                if (insideFuniture)
                {
                    Exited();
                }
            }
        }
    }


    private void EnteredFuniture()
    {
        Debug.Log("Entered Funiture first Time");
        InvokeHologramEnteredFuniture();
        insideFuniture = true;
    }

    private void EnteredTableFuniture(Collider collision)
    {
        Debug.Log("Entered Funiture first Time");
        InvokeHologramEnteredFunitureTable(collision.transform.root.gameObject.GetComponent<FunitureWithPlaceArea>());
        insideFuniture = true;
    }
    

    private void Exited()
    {
        Debug.Log("Exited Funiture first Time");
        InvokeHologramExitFunitureTable();
        insideFuniture = false;
    }

    
    private void ExitedOnTable()
    {
        Debug.Log("ExitedFuniture on the Table");
        InvokeTopHologramExit();
        insideFuniture = false;
    }

    private void EnteredOnTable()
    {
        Debug.Log("EnteredFuniture on the Table");
        InvokeTopHologramEntered();
        insideFuniture = true;
    }
}
