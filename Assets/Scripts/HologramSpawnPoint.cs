using System;

using UnityEngine;

public class HologramSpawnPoint : MonoBehaviour
{
    
    public event Action HologramEnteredFuniture;
    public event Action HologramExitFuniture;

    public void InvokeHologramEnteredFuniture() => HologramEnteredFuniture?.Invoke();
    
    public void InvokeHologramExitFuniture() => HologramExitFuniture?.Invoke();

    public bool insideFuniture = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Funiture")
        {
            Debug.Log("Entered Funiture");
            if (!insideFuniture)
            {
                Debug.Log("Entered Funiture first Time");
                InvokeHologramEnteredFuniture();
                insideFuniture = true;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Funiture")
        {
            Debug.Log("Exited Funiture");
            if (insideFuniture)
            {
                Debug.Log("Exited Funiture first Time");
                InvokeHologramExitFuniture();
                insideFuniture = false;
            }
        }
    }
    
}
