using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class Wall : MonoBehaviour
    {
        public Vector3 gridPos;
        public MyGridSystemXZ gridSystem;
        public float yOffset;
        // Start is called before the first frame update
        void Start()
        {
            gridPos = gridSystem.WorldPositionToGrid(transform.position);
            yOffset = transform.position.y;
            gridSystem.PlaceSolidObj_Limited(gridPos, yOffset, gameObject, "wall");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

