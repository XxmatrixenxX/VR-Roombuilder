using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FunitureGrabable : Funiture
{

    [SerializeField] private SC_For_Mode.Mode modeWhereCanGrab = SC_For_Mode.Mode.playerMode;

     public override void StartOptions()
    {
        buildingCharakter = FindObjectOfType<BuildingCharakter>();
        ChangeTagOfChild();
    }

     public bool CanGrab()
     {
         return buildingCharakter.activeMode == modeWhereCanGrab;
     }
     
     
     
    
}


