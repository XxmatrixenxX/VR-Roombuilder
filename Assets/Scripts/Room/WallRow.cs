
using System;
using System.Collections.Generic;
using UnityEngine;

public class WallRow : MonoBehaviour
{
   [Tooltip("Put the Wall left to right in")]
   public List<WallPart> wallRow;

   public Direction direction;

   public enum Direction
   {
      North,
      East,
      South,
      West
   }

   public void RotateLeft()
   {
      switch (direction)
      {
         case Direction.North:
            direction = Direction.West;
            break;
         case Direction.East:
            direction = Direction.North;
            break;
         case Direction.South:
            direction = Direction.East;
            break;
         case Direction.West:
            direction = Direction.South;
            break;
      }
   }
   
   public void RotateRight()
   {
      switch (direction)
      {
         case Direction.North:
            direction = Direction.East;
            break;
         case Direction.East:
            direction = Direction.South;
            break;
         case Direction.South:
            direction = Direction.West;
            break;
         case Direction.West:
            direction = Direction.North;
            break;
      }
   }
}