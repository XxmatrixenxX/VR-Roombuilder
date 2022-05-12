using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class GridBuildingSystem : MonoBehaviour
{
    public Gridfield<GridObject> grid;
    public PrimaryButtonWatcher watcher;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private Transform testTransform;

    void Start()
    {
        watcher.primaryButtonPress.AddListener(CreateObject);
    }
    private void Awake()
    {
        int gridWidth = 10;
        int gridHight = 10;
        float cellSize = 4f;
        grid = new Gridfield<GridObject>(gridWidth, gridHight, cellSize, Vector3.zero,
            (Gridfield<GridObject> g, int x, int z) => new GridObject(g, x, z));
    }

    public class GridObject
    {
        private Gridfield<GridObject> grid;
        private int x;
        private int z;
        private Transform transform;

        public GridObject(Gridfield<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
        }
        
        public override string ToString()
        {
            return x + ", " + z;
        }
        
        
    }

    private void CreateObject(bool pressed)
    {

        if (pressed)
        {
            grid.GetXY(leftHand.gameObject.transform.position, out int x, out int z);
            Instantiate(testTransform, grid.GetWorldPosition(x, z), Quaternion.identity);
        }
    }
    
    
    
    
}
