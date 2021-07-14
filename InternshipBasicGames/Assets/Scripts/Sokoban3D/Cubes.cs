using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class Cubes : MonoBehaviour, IMoveableObjects
    {
        public MyGridSystemXZ gridSystem;
        public float yOffset;
        public MyGridXZ nextGrid, currentGrid;
        public Material placedMAT;
        public Material normalMAT;

        public ICommand currentCommand;
        public ObjectList.NextGridIs nextGridIs;

        // Start is called before the first frame update
        void Start()
        {
            var gridPos = gridSystem.WorldPositionToGrid(transform.position);
            currentGrid = gridSystem.GetCurrentGrid(gridPos);
            gridSystem.PlaceSolidObj_Limited(gridPos, yOffset, gameObject, ObjectList.CUBE);
            nextGrid = new MyGridXZ();
            currentCommand = new WaitingCommand(this);
        }


        public void PushMe(Vector3 pushMovement)
        {
            var pos = transform.position;
            var movement = pushMovement;
            pos += movement;
            transform.position = pos;

        }

        public void GetNextGrid(Vector3 pushDirection)
        {
            if (nextGrid.gridPosition.x == -1)
            {
                var gridPos = currentGrid.gridPosition;
                gridPos += pushDirection;
                nextGrid = gridSystem.GetCurrentGrid(gridPos);

                if (nextGrid.openGrid)
                {
                    nextGridIs = ObjectList.NextGridIs.EmptyGrid;
                    currentCommand = new ReplaceCommand(this, nextGrid, currentGrid);
                }
                else
                {
                    nextGridIs = ObjectList.NextGridIs.Wall;
                }

            }


        }



        public void NextMoveOnGridSystem(MyGridXZ placedGrid)
        {
            gridSystem.RemoveSolidObject_Limited(currentGrid.gridPosition);
            currentGrid = placedGrid;
            gridSystem.PlaceSolidObj_Limited(placedGrid.gridPosition, yOffset, gameObject, ObjectList.CUBE);
        }

        public void UndoTheMove(MyGridXZ placedGrid)
        {
            NextMoveOnGridSystem(placedGrid);
            OnEndPushAnimation();
            transform.position = currentGrid.worldPosition;
        }

        public void OnEndPushAnimation()
        {
            if (currentGrid.placedUnsolidObj != null && currentGrid.placedUnsolidObjTag == ObjectList.CUBEHOLDER)
            {
                GetComponent<MeshRenderer>().material = placedMAT;
            }
            else
            {
                GetComponent<MeshRenderer>().material = normalMAT;
            }
            nextGridIs = ObjectList.NextGridIs.None;
            currentCommand = new WaitingCommand(this);
            nextGrid = new MyGridXZ();
        }

        public void Waiting()
        {
            Debug.Log(gameObject.name + " is waiting");
        }
    }

}

