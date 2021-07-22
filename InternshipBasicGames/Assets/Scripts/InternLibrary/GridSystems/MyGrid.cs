using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyGrid 
{
    public Vector2 worldPosition;
    public Vector2 gridPosition; // TODO hic kullanilmamis
    
    public GameObject placedObj;
    public string placedObjTag;

    public MyGrid(float posX,float posY, Vector2 gridPos )
    {
        worldPosition.x = posX;
        worldPosition.y = posY;
        gridPosition = gridPos;
    }
}
