using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGridSystemXZ : MonoBehaviour
{
    public int rows;
    public int cols;
    public float gridSize = 1;
    public MyGridXZ[,] myGrid = new MyGridXZ[0, 0];
    float Xoffset, Zoffset;

    private void Awake()
    {
        myGrid = new MyGridXZ[rows, cols];
        Xoffset = transform.position.x - ((cols / 2) * gridSize);
        Zoffset = transform.position.z - ((rows / 2) * gridSize);
        GenerateGridMap();
    }


    public void GenerateGridMap()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int k = 0; k < cols; k++)
            {
                myGrid[i, k] = new MyGridXZ(Xoffset + (k * gridSize) + gridSize / 2,0, Zoffset + (i * gridSize) + gridSize / 2, new Vector3(k,0, i));
            }
        }

    }

    public MyGridXZ GetGrid(Vector3 gridPos)
    {
        if (gridPos.x >= cols)
        {
            gridPos.x = cols-1;
        }
        else if (gridPos.x < 0)
        {
            gridPos.x = 0;
        }

        if (gridPos.z >= rows)
        {
            gridPos.z = rows-1;
        }
        else if (gridPos.z < 0)
        {
            gridPos.z = 0;
        }

        MyGridXZ newGrid = myGrid[(int)gridPos.z, (int)gridPos.x];

        return newGrid;
    }

    public MyGridXZ GetCurrentGridInfinitly(ref Vector3 pos)
    {
        if (pos.x >= cols)
        {
            pos.x = pos.x % cols;
        }
        else if (pos.x < 0)
        {
            pos.x += cols;
        }

        if (pos.z >= rows)
        {
            pos.z = pos.z % rows;
        }
        else if (pos.z < 0)
        {
            pos.z += rows;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];

        return newGrid;
    }

    public Vector3 WorldPosToGridPos(Vector3 worldPos)
    {

        Vector3 gridPos = new Vector3((int)(((worldPos.x - (Xoffset) - gridSize / 2)) / gridSize), 0, (int)(((worldPos.z - (Zoffset) - gridSize / 2)) / gridSize));
        return gridPos;
    }

    public MyGridXZ FindGridAccordingToWorldPos(Vector3 worldPos)
    {
        Vector3 gridPos = new Vector3((int)(((worldPos.x - (Xoffset) - gridSize / 2)) / gridSize), 0, (int)(((worldPos.z - (Zoffset) - gridSize / 2)) / gridSize));
        return GetGrid(gridPos);
    }

    #region Infinityle Area Methods

    public void PlaceSolidObj_Infinitly(Vector3 pos, GameObject obj, string tag)
    {
        if (pos.x >= cols)
        {
            pos.x = pos.x % cols;
        }
        else if (pos.x < 0)
        {
            pos.x += cols;
        }

        if (pos.z >= rows)
        {
            pos.z = pos.z % rows;
        }
        else if (pos.z < 0)
        {
            pos.z += rows;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];

        newGrid.placedObj = obj;
        newGrid.placedObjTag = tag;
        newGrid.openGrid = false;
    }

    public void PlaceUnsolidObj_Infinitly(Vector3 pos, GameObject obj, string tag)
    {
        if (pos.x >= cols)
        {
            pos.x = pos.x % cols;
        }
        else if (pos.x < 0)
        {
            pos.x += cols;
        }

        if (pos.z >= rows)
        {
            pos.z = pos.z % rows;
        }
        else if (pos.z < 0)
        {
            pos.z += rows;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];
        newGrid.placedUnsolidObj = obj;
        newGrid.placedUnsolidObjTag = tag;
    }

    public void RemoveSolidObject_Infinitly(Vector3 pos)
    {
        if (pos.x >= cols)
        {
            pos.x = pos.x % cols;
        }
        else if (pos.x < 0)
        {
            pos.x += cols;
        }

        if (pos.z >= rows)
        {
            pos.z = pos.z % rows;
        }
        else if (pos.z < 0)
        {
            pos.z += rows;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];
        newGrid.worldPosition.y = 0;
        newGrid.placedObjTag = null;
        newGrid.placedObj = null;
        newGrid.openGrid = true;
    }

    public void RemoveUnsolidObject_Infinitly(Vector3 pos)
    {
        if (pos.x >= cols)
        {
            pos.x = pos.x % cols;
        }
        else if (pos.x < 0)
        {
            pos.x += cols;
        }

        if (pos.z >= rows)
        {
            pos.z = pos.z % rows;
        }
        else if (pos.z < 0)
        {
            pos.z += rows;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];
        newGrid.worldPosition.y = 0;
        newGrid.placedUnsolidObjTag = null;
        newGrid.placedUnsolidObj = null;
    }

    #endregion

    #region Limited Area Methods

    public void PlaceSolidObj_Limited(Vector3 pos, GameObject obj, string tag)
    {
        if (pos.x >= cols)
        {
            pos.x = cols-1;
        }
        else if (pos.x < 0)
        {
            pos.x = 0;
        }

        if (pos.z >= rows)
        {
            pos.z = rows -1;
        }
        else if (pos.z < 0)
        {
            pos.z = 0;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];

        newGrid.placedObj = obj;
        newGrid.placedObjTag = tag;
        newGrid.openGrid = false;

    }
   
    public void PlaceUnsolidObj_Limited(Vector3 pos, GameObject obj, string tag)
    {
        if (pos.x >= cols)
        {
            pos.x = cols - 1;
        }
        else if (pos.x < 0)
        {
            pos.x = 0;
        }

        if (pos.z >= rows)
        {
            pos.z = rows - 1;
        }
        else if (pos.z < 0)
        {
            pos.z = 0;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];
        newGrid.placedUnsolidObj = obj;
        newGrid.placedUnsolidObjTag = tag;
    }

    public void RemoveSolidObject_Limited(Vector3 pos)
    {
        if (pos.x >= cols)
        {
            pos.x = cols - 1;
        }
        else if (pos.x < 0)
        {
            pos.x = 0;
        }

        if (pos.z >= rows)
        {
            pos.z = rows - 1;
        }
        else if (pos.z < 0)
        {
            pos.z = 0;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];
        newGrid.worldPosition.y = 0;
        newGrid.placedObjTag = null;
        newGrid.placedObj = null;
        newGrid.openGrid = true;
    }

    public void RemoveUnsolidObject_Limited(Vector3 pos)
    {
        if (pos.x >= cols)
        {
            pos.x = cols - 1;
        }
        else if (pos.x < 0)
        {
            pos.x = 0;
        }

        if (pos.z >= rows)
        {
            pos.z = rows - 1;
        }
        else if (pos.z < 0)
        {
            pos.z = 0;
        }

        MyGridXZ newGrid = myGrid[(int)pos.z, (int)pos.x];
        newGrid.worldPosition.y = 0;
        newGrid.placedUnsolidObjTag = null;
        newGrid.placedUnsolidObj = null;
    }

    #endregion



    private void OnDrawGizmos()
    {

        Xoffset = transform.position.x - ((cols / 2) * gridSize);
        Zoffset = transform.position.z - ((rows / 2) * gridSize);


        for (int i = 0; i <= rows; i++)
        {
            for (int k = 0; k <= cols; k++)
            {
                Debug.DrawLine(new Vector3(Xoffset,0, Zoffset + (i * gridSize)), new Vector3(Xoffset + (cols * gridSize),0, Zoffset + (i * gridSize)));
                Debug.DrawLine(new Vector3(Xoffset + (k * gridSize),0, Zoffset), new Vector3(Xoffset + (k * gridSize),0, Zoffset + (rows * gridSize)));
                //Debug.DrawLine(new Vector3(Xoffset+(k * gridSize), 0, Zoffset + (i * gridSize)), new Vector3(Xoffset+(k * gridSize), gridSize, Zoffset + (i * gridSize))); //for y axis lines
            }

        }

    }
}

