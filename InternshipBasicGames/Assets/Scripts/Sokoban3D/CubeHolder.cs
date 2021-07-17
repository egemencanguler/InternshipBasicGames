using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class CubeHolder : MonoBehaviour
    {
        public Vector3 offSet;
        public MyGridSystemXZ gridSystem;
        public MyGridXZ currentGrid;
        void Start()
        {
            currentGrid = gridSystem.FindGridAccordingToWorldPos(transform.position);
            gridSystem.PlaceUnsolidObj_Limited(currentGrid.gridPosition, gameObject, "cubeholder");
            transform.position = currentGrid.worldPosition + offSet;
        }


    }
}

