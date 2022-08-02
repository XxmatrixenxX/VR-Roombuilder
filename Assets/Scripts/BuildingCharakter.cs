using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuildingCharakter : MonoBehaviour
{
   public event Action ModeChanged;

   public event Action<GameObject> SelectedNewItem;
   
   public WristMenuHandler wristMenu;

   public SC_For_Mode.Mode activeMode = SC_For_Mode.Mode.playerMode;

   public void InvokeSelectedNewItem(GameObject item) => SelectedNewItem?.Invoke(item);
   
   public void InvokeModeChanged() => ModeChanged?.Invoke();
   
   public void ChangeMode(SC_For_Mode.Mode mode)
   {
      switch (mode)
      {
         case SC_For_Mode.Mode.buildingMode:
            ChangeToBuildingMode();
            break;
         case SC_For_Mode.Mode.playerMode:
            ChangeToPlayerMode();
            break;
         case SC_For_Mode.Mode.chooseBuildingMode:
            ChangeToChoosingMode();
            break;
         case SC_For_Mode.Mode.topBuildingMode:
            ChangeToTopMode();
            break;
         case SC_For_Mode.Mode.wallBuildingMode:
            ChangeToWallbuildingMode();
            break;
      }
      InvokeModeChanged();
   }

   private void ChangeToWallbuildingMode()
   {
      if (activeMode.Equals( SC_For_Mode.Mode.chooseBuildingMode))
      {
         DisableBuildingSelect();
      }

      activeMode = SC_For_Mode.Mode.wallBuildingMode;
   }

   public void ChangeToBuildingMode()
   {
      if (activeMode.Equals( SC_For_Mode.Mode.chooseBuildingMode))
      {
         DisableBuildingSelect();
      }

      activeMode = SC_For_Mode.Mode.buildingMode;
   }
   
   public void ChangeToChoosingMode()
   {
      activeMode = SC_For_Mode.Mode.chooseBuildingMode;

      EnableBuildingSelect();
      wristMenu.ActivateItemSelectCanvas();
   }
   
   public void ChangeToTopMode()
   {
      if (activeMode.Equals( SC_For_Mode.Mode.chooseBuildingMode))
      {
         DisableBuildingSelect();
      }
      activeMode = SC_For_Mode.Mode.topBuildingMode;
   }
   
   public void ChangeToPlayerMode()
   {
      if (activeMode.Equals( SC_For_Mode.Mode.chooseBuildingMode))
      {
         DisableBuildingSelect();
      }

      activeMode = SC_For_Mode.Mode.playerMode;
   }

   private void EnableBuildingSelect()
   {
      wristMenu.OpenMenus();
   }
   
   private void DisableBuildingSelect()
   {
      wristMenu.CloseMenus();
   }

   private void CameraChangeTopDown()
   {
      
   }

   private void ChangeToOtherVRControll()
   {
      
   }
   

}
