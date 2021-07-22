using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGridSystem : MonoBehaviour
{
    public static MyGridSystem instance;
    // TODO column row yerine sizeX, sizeY tercih ediyorum daha az karisiyor
    public int rows;
    public int cols;
    public float gridSize = 1;
    public MyGrid[,] myGrid = new MyGrid[0, 0];
    float Xoffset, Yoffset; // TODO xOffset, yOffset ya da Vector2 offset;

    private void Awake()
    {
        instance = this;
        myGrid = new MyGrid[rows, cols];
        Xoffset = transform.position.x - ((cols / 2) * gridSize);
        Yoffset = transform.position.y - ((rows / 2) * gridSize);
        GenerateGridMap();
    }

    // TODO bos fonksyonlar
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
                myGrid[i, k] = new MyGrid(Xoffset + (k*gridSize) + gridSize / 2, Yoffset + (i*gridSize) + gridSize / 2, new Vector2(k, i));
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
            pos.x = pos.x % cols; // TODO float ve mod operatoru ne donuyor inan bilmiyorum
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

    public void PlaceTheObjToGrid(Vector2 pos, GameObject obj,string tag)
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
        newGrid.placedObjTag = tag;
        obj.transform.position = newGrid.worldPosition;
    }

    public void RemoveTheObjectFromGrid(Vector2 pos)
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
        newGrid.placedObjTag = null;
        newGrid.placedObj = null;
    }

    public Vector2 WorldPositionToGrid(Vector2 worldPos)
    {
        
        Vector2 gridPos = new Vector2((int)((worldPos.x - (Xoffset) - gridSize / 2)/gridSize), (int)((worldPos.y - (Yoffset) - gridSize / 2))/gridSize);
        return gridPos;
    }


    private void OnDrawGizmos()
    {

        Xoffset = transform.position.x - ((cols / 2) * gridSize);
        Yoffset = transform.position.y - ((rows / 2) * gridSize);


        for (int i = 0; i <= rows; i++)
        {
            for (int k = 0; k <= cols; k++)
            {
                Debug.DrawLine(new Vector2(Xoffset, Yoffset + (i * gridSize)), new Vector2(Xoffset + (cols * gridSize), Yoffset + (i * gridSize)));
                Debug.DrawLine(new Vector2(Xoffset + (k * gridSize), Yoffset), new Vector2(Xoffset + (k * gridSize), Yoffset + (rows * gridSize)));

                //Debug.DrawLine(new Vector2(transform.position.x , transform.position.y + i), new Vector2(transform.position.x + cols, transform.position.y + i));
                //Debug.DrawLine(new Vector2(transform.position.x + k, transform.position.y), new Vector2(transform.position.x + k, transform.position.y + rows));
            }

        }

    }
}
