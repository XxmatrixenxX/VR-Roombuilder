using Unity.Mathematics;
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
        RotateObjectHolder();
    }
     
     

     public bool CanGrab()
     {
         return buildingCharakter.activeMode == modeWhereCanGrab;
     }

     public void RotateObjectHolder()
     {
         Debug.Log("Rotated GrabableObject");
         float y = objectHolder.transform.rotation.y;
         objectHolder.transform.rotation = quaternion.Euler(0,y +90 , 0);
     }
     
     
     
    
}


