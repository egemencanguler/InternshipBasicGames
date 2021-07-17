using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class Wall : MonoBehaviour
    {
        public MyGridSystemXZ gridSystem;
        public MyGridXZ currentGrid;
        public Vector3 offSet;
        // Start is called before the first frame update
        void Start()
        {
            currentGrid = gridSystem.FindGridAccordingToWorldPos(transform.position);
            gridSystem.PlaceSolidObj_Limited(currentGrid.gridPosition, gameObject, "wall");
            transform.position = currentGrid.worldPosition + offSet;
        }


    }
}

