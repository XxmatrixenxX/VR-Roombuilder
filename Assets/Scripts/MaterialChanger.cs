using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
   [SerializeField] private Material _material;

   /// <summary>
   /// Gets Object which is touching theObject
   /// </summary>
   /// <param name="Inhand">Object inside of the Hand (Grabbed).</param>
   private void GetObject()
   {
      return;
   }

   /// <summary>
   /// Checks if the Collision can Change the Material
   /// Deletes this Object if it can Change the Color
   /// </summary>
   /// <param name="other">Object that gets in contact</param>
   private void OnCollisionEnter(Collision other)
   {
      var canChange = false;
      GameObject otherObject = other.gameObject;

      if (IsObjectOf(otherObject))
      {
         otherObject.GetComponent<Renderer>().material = GetMaterial(this.gameObject);
         DestroyMe();
      }
      
      //DoNothing
      return;
   }

   /// <summary>
   /// Gets Material of GameObject
   /// </summary>
   /// <param name="gameObject">GameObject where you want to have the Material</param>
   /// <returns>Material of the GameObject </returns>
   private Material GetMaterial(GameObject gameObject)
   {
      return gameObject.GetComponent<Renderer>().material;
   }


   /// <summary>
   /// Method to Destroy the Object
   /// </summary>
   private void DestroyMe()
   {
      Destroy(this);
   }

   /// <summary>
   /// Check if Object has a materialChange Script
   /// </summary>
   /// <param name="gameobject">Object to Check</param>
   /// <returns>Bool if the Object has Script </returns>
   private bool IsObjectOf(GameObject gameobject)
   {
      return false;
   }

   /// <summary>
   /// Object will be Destroyed if thrown and not Hitting anything after time
   /// Deletes this Object if it can Change the Color
   /// </summary>
   private void DestroyAfterTime()
   {
      var destroyTime = 15;
      Invoke("DestroyMe", destroyTime);
   }
}
