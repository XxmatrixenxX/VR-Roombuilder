using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabPlacementItem : MonoBehaviour
{
   //Controller Trigger and what they do 
   //Todo making own Animator method
   
   /**
    * If you have an Item in the Hand you should set it down with this Trigger
    */
   [SerializeField] private InputActionReference controllerTrigger;
   
   /**
    * Grib with the Trigger for taking items in the Hand
    */
   [SerializeField] private InputActionReference controllerGrib;

   /**
    * Removing the Item in the Hand
    */
   [SerializeField] private InputActionReference controllerButton;


   private Animator _handAnimator;

   private float transperent = 0.5f;
  
   private void Awake()
   {
    controllerGrib.action.performed += GribPress;
    controllerTrigger.action.performed += TriggerPress;
    controllerButton.action.performed += ButtonPress;
   }

   private void ButtonPress(InputAction.CallbackContext obj)
   {
    //_handAnimator.SetFloat("Button", obj.ReadValue<float>());
   }

   private void TriggerPress(InputAction.CallbackContext obj)
   { 
    //_handAnimator.SetFloat("Trigger", obj.ReadValue<float>());
   }


   private void GribPress(InputAction.CallbackContext obj)
   {
    //_handAnimator.SetFloat("Grib", obj.ReadValue<float>());
   }

   /// <summary>
   /// Changes the opacity of Object in the Hand
   /// </summary>
   /// <param name="Inhand">Object inside of the Hand (Grabbed).</param>
   private void changeOpacityInHand(GameObject Inhand)
   {
    var color  = Inhand.GetComponent<Renderer>().material.color;
    color.a = transperent;
   }

   
   /// <summary>
   /// Changes the opacity of Object placed to 1
   /// </summary>
   /// <param name="Inhand">Object inside of the Hand (Grabbed).</param>
   private void changeOpacityIfPlaced(GameObject Inhand)
   {
    var color  = Inhand.GetComponent<Renderer>().material.color;
    color.a = 1;
   }

   /// <summary>
   /// Destroy if not Placed, but cancled 
   /// </summary>
   private void DestroyObject()
   {
    Destroy(this);
   }


   /// <summary>
   /// If Placed sets a Copy at the Location and Destroy Object in Hand (Only if not MultiMode)
   /// Activate Set Opacity to 1 of the Object
   /// </summary>
   private void Placed()
   {
    
   }
   
   /// <summary>
   /// Toggle for MultiMode, to set Many Objects without Destroying
   /// </summary>
   private void ChangeToMultiMode()
   {
    
   }
   
   
}
