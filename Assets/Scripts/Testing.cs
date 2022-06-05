using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    //Create a new Gridfield

    private Gridfield<bool> grid;
    void Start()
    {
        grid = new Gridfield<bool>(20, 10, 10f , new Vector3(10,0), (Gridfield<bool> g, int x, int y) => new bool());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetGridfieldObject(WorldText.GetMouseWorldPosition(), true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(WorldText.GetMouseWorldPosition()));
        }
    }
}
