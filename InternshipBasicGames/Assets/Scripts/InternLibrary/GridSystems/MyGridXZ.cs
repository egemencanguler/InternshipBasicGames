using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyGridXZ
{
    public Vector3 worldPosition;
    public Vector3 gridPosition;

    public GameObject placedObj;
    public string placedObjTag;
    public GameObject placedUnsolidObj;
    public string placedUnsolidObjTag;
    public bool openGrid;

    public MyGridXZ(float posX,float posY , float posZ, Vector3 gridPos)
    {
        worldPosition.x = posX;
        worldPosition.y = posY;
        worldPosition.z = posZ;
        gridPosition = gridPos;
        openGrid = true;
    }

    public MyGridXZ() //for null
    {
        gridPosition.x = -1;
    }
}
