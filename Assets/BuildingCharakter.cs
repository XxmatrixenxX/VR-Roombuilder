using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCharakter : MonoBehaviour
{
   public bool buildingMode = false;
   
   public bool chooseBuildingMode = false;

   public bool topBuildingMode = false;

   public bool playerMode = true;

   public void ChangeToBuildingMode()
   {
      if (chooseBuildingMode)
      {
         DisableBuildingSelect();
      }
      buildingMode = true;
      chooseBuildingMode = false;
      topBuildingMode = false;
      playerMode = false;
   }
   
   public void ChangeToChoosingMode()
   {
      buildingMode = false;
      chooseBuildingMode = true;
      topBuildingMode = false;
      playerMode = false;

      EnableBuildingSelect();
   }
   
   public void ChangeToTopMode()
   {
      if (chooseBuildingMode)
      {
         DisableBuildingSelect();
      }
      buildingMode = false;
      chooseBuildingMode = false;
      topBuildingMode = true;
      playerMode = false;
   }
   
   public void ChangeToPlayerMode()
   {
      if (chooseBuildingMode)
      {
         DisableBuildingSelect();
      }
      buildingMode = false;
      chooseBuildingMode = false;
      topBuildingMode = false;
      playerMode = true;
   }

   private void EnableBuildingSelect()
   {
      
   }
   
   private void DisableBuildingSelect()
   {
      
   }

   private void CameraChangeTopDown()
   {
      
   }

   private void ChangeToOtherVRControll()
   {
      
   }
   
}
