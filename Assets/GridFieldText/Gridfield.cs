using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Gridfield<TGridfieldobject>
{
      public event EventHandler<OnGridfieldValueChangedEventArgs> OnGridfieldValueChanged;

      public class OnGridfieldValueChangedEventArgs : EventArgs
      {
            public int x;
            public int y;
      }
      
      //width of the Gridfield
      private int width;
      //length of the Gridfield
      private int length;
      //Size of the Gridfield
      private float cellSize;
      //Location of the Gridfield
      private Vector3 originPosition;
      
      private TGridfieldobject[,] gridfieldArray;
      private TextMesh[,] debugTextArray;

      //Creation of the Gridfield
      //Func Parameter - Gridfield referenze - grid x location - grid y location - Object inside
      public Gridfield(int width, int length, float cellSize, Vector3 originPosition, Func<Gridfield<TGridfieldobject>, int , int , TGridfieldobject> createGridfieldObject)
      {
            this.width = width;
            this.length = length;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            
            gridfieldArray = new TGridfieldobject[width, length];

            for (int x = 0; x < gridfieldArray.GetLength(0); x++)
            {
                  for (int y = 0; y < gridfieldArray.GetLength(1); y++)
                  {
                        gridfieldArray[x, y] = createGridfieldObject(this, x, y);
                  }
            }
            
            bool showDebug = true;
            if (showDebug)
            {
                  debugTextArray = new TextMesh[width, length];


                  //Shows the Creation
                  for (int x = 0; x < gridfieldArray.GetLength(0); x++)
                  {
                        for (int y = 0; y < gridfieldArray.GetLength(1); y++)
                        {
                              debugTextArray[x, y] = WorldText.CreateWorldText(gridfieldArray[x, y]?.ToString(), null,
                                    GetWorldPosition(x, y) + new Vector3(cellSize,0 , cellSize) * .5f, 20, Color.white,
                                    TextAnchor.MiddleCenter);
                              Debug.Log("width" + x + ", length" + y);
                              //Draw Lines between grid length
                              Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                              //Draw Lines between grid width
                              Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                        }
                  }
                  //Drawing lines for the edges
                  Debug.DrawLine(GetWorldPosition(0, length), GetWorldPosition(width, length), Color.white, 100f);
                  Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, length), Color.white, 100f);

                  OnGridfieldValueChanged += (object sender, OnGridfieldValueChangedEventArgs eventArgs) =>
                  {
                        debugTextArray[eventArgs.x, eventArgs.y].text =
                              gridfieldArray[eventArgs.x, eventArgs.y]?.ToString();
                  };
            }
      }

      //Change from Horizontal to Vertical Change y With z
      public Vector3 GetWorldPosition(int x, int y)
      {
            return new Vector3(x,0 , y) * cellSize + originPosition;
      }

      //Change from Horizontal to Vertical Change y With z
      public void GetXY(Vector3 worldPosition, out int x, out int y)
      {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
      }
      
      public void SetGridfieldObject(int x, int y, TGridfieldobject value)
      {
            if (x >= 0 && y >= 0 && x < width && y < length)
            {
                  gridfieldArray[x, y] = value;
                  if (OnGridfieldValueChanged != null)
                        OnGridfieldValueChanged(this, new OnGridfieldValueChangedEventArgs
                        {
                              x = x, y = y
                        });

            //debugTextArray[x, y].text = gridfieldArray[x, y]?.ToString();
            }
      }

      public void TriggerGridObjectChanged(int x, int y)
      {
            if (OnGridfieldValueChanged != null)
                  OnGridfieldValueChanged(this, new OnGridfieldValueChangedEventArgs
                  {
                        x = x, y = y
                  });
      }

      public void SetGridfieldObject(Vector3 worldPosition, TGridfieldobject value)
      {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridfieldObject(x, y, value);
      }

      public TGridfieldobject GetValue(int x, int y)
      {
            if (x >= 0 && y >= 0 && x < width && y < length)
            {
                  return gridfieldArray[x, y];
            }
            else
            {
                  return default(TGridfieldobject);
            }
      }

      public TGridfieldobject GetValue(Vector3 worldPosition)
      {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
      }
}
