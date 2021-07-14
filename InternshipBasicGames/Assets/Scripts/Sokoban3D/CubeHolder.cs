using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class CubeHolder : MonoBehaviour
    {
        public Vector3 gridPos;
        public float Yoffset;
        public MyGridSystemXZ gridSystem;
        void Start()
        {
            gridPos = gridSystem.WorldPositionToGrid(transform.position);
            Yoffset = transform.position.y;
            gridSystem.PlaceUnsolidObj_Limited(gridPos, Yoffset, gameObject, "cubeholder");
        }


    }
}

