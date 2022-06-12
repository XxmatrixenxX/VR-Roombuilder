using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GroundMash : MonoBehaviour
{
   private List<MashDragablePoint> vertices;

   
   public void CreateBaseLine(Vector3 startPosition)
   {
      Vector3 position = startPosition;
      for (int a = 0; a < 3; a++)
      {
         position = CalculateBasePosition(position, a);
         vertices.Add(CreateMashDragablePoint(position));
      }
   }

   public MashDragablePoint CreateMashDragablePoint(Vector3 location)
   {
      return gameObject.AddComponent<MashDragablePoint>();
   }

   
   public Vector3 CalculateBasePosition(Vector3 basePosition, int spawnNumber)
   {
      
      switch (spawnNumber)
      {
         case 0:
            return basePosition;
         case 1:
            return basePosition + new Vector3(0, 0, 1);
         case 2:
            return basePosition + new Vector3(1, 0, 0);
         case 3:
            return basePosition + new Vector3(0, 0, -1);
         default:
            return basePosition;
      }
   }

   public void CreateTriangles()
   {

      List<int[]> Triangles;
      
      int actualNumber;
      int number = vertices.Count;
      
      //Start, start +1 , start +2 
      //Start +2, Start+3 , Start
      //(a , b , c)
      
      // b , a , b+1 (c) 
      // c, b , c+1






   }

   public int[] Triangle(int begin, int second, int third)
   {
      return new int[] {begin, second, third};
   }
   
   
}
