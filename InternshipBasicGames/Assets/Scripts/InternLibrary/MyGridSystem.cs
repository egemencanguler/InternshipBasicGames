using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGridSystem : MonoBehaviour
{
    public static MyGridSystem instance;
    public Vector2 gridStartPoint;
    public int rows;
    public int cols;
    public float gridSize = 1;
    public MyGrid[,] myGrid = new MyGrid[0, 0];

    private void Awake()
    {
        instance = this;
        myGrid = new MyGrid[rows, cols];
        GenerateGridMap();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGridMap()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int k = 0; k < cols; k++)
            {
                myGrid[i, k] = new MyGrid(gridStartPoint.x + k + gridSize / 2, gridStartPoint.y + i + gridSize / 2, new Vector2(k, i));
            }
        }

    }

    public MyGrid GetCurrentGrid(Vector2 pos)
    {


        MyGrid newGrid = myGrid[(int)pos.y, (int)pos.x];

        return newGrid;
    }

    public MyGrid GetCurrentGridInfinitly(ref Vector2 pos)
    {
        if (pos.x >= cols)
        {
            pos.x = pos.x % cols;
        }
        else if (pos.x < 0)
        {
            pos.x += cols;
        }

        if (pos.y >= rows)
        {
            pos.y = pos.y % rows;
        }
        else if (pos.y < 0)
        {
            pos.y += rows;
        }

        MyGrid newGrid = myGrid[(int)pos.y, (int)pos.x];

        return newGrid;
    }

    public void PlaceTheObjToGrid(Vector2 pos, GameObject obj)
    {
        if (pos.x >= cols)
        {
            pos.x = pos.x % cols;
        }
        else if (pos.x < 0)
        {
            pos.x += cols;
        }

        if (pos.y >= rows)
        {
            pos.y = pos.y % rows;
        }
        else if (pos.y < 0)
        {
            pos.y += rows;
        }

        MyGrid newGrid = myGrid[(int)pos.y, (int)pos.x];

        newGrid.placedObj = obj;
        obj.transform.position = newGrid.worldPosition;
    }

    public Vector2 WorldPositionToGrid(Vector2 worldPos)
    {
        Vector2 gridPos = new Vector2((int)(worldPos.x - (gridStartPoint.x) - gridSize / 2), (int)(worldPos.y - (gridStartPoint.y) - gridSize / 2));
        return gridPos;
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i <= rows; i++)
        {
            for (int k = 0; k <= cols; k++)
            {
                Debug.DrawLine(new Vector2(transform.position.x , transform.position.y + i), new Vector2(transform.position.x + cols, transform.position.y + i));
                Debug.DrawLine(new Vector2(transform.position.x + k, transform.position.y), new Vector2(transform.position.x + k, transform.position.y + rows));
            }

        }

    }
}
