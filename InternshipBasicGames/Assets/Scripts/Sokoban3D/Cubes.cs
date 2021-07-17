using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class Cubes : MonoBehaviour, IMoveableObjects
    {
        public MyGridSystemXZ gridSystem;
        public Vector3 offSet;
        public MyGridXZ nextGrid, currentGrid;
        public Material placedMAT;
        public Material normalMAT;

        public ICommand currentCommand;
        public ObjectList.NextGridIs nextGridIs;

        // Start is called before the first frame update
        void Start()
        {
            currentGrid = gridSystem.FindGridAccordingToWorldPos(transform.position);
            gridSystem.PlaceSolidObj_Limited(currentGrid.gridPosition, gameObject, ObjectList.CUBE);
            OnEndPushAnimation();

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
            if (nextGrid.gridPosition.x == -1)// colliding check
            {
                var gridPos = currentGrid.gridPosition;
                gridPos += pushDirection;
                nextGrid = gridSystem.GetGrid(gridPos);

                if(nextGrid.placedObjTag == ObjectList.SAW)
                {
                    var saw = nextGrid.placedObj.GetComponent<Saw>();
                    Vector2 cubeMovement = new Vector2(pushDirection.x, pushDirection.z);
                    Vector2 sawMovement = new Vector2(saw.movementDirection[saw.lastMoveDirection].x, saw.movementDirection[saw.lastMoveDirection].z);
                    if (Vector2.Dot(cubeMovement, sawMovement) >= 0)//to move the cube despite the saw 
                    {
                        nextGridIs = ObjectList.NextGridIs.EmptyGrid;
                        currentCommand = new ReplaceCommand(this, nextGrid, currentGrid);
                    }
                    else
                    {
                        nextGridIs = ObjectList.NextGridIs.Wall;
                    }
                }
                else if(nextGrid.placedObjTag == ObjectList.TURRETBULLET)
                {
                    var bullet = nextGrid.placedObj.GetComponent<TurretBullet>();
                    Vector2 cubeMovement = new Vector2(pushDirection.x, pushDirection.z);
                    Vector2 bulletMovement = new Vector2(bullet.movementDirection.x, bullet.movementDirection.z);
                    if (Vector2.Dot(cubeMovement, bulletMovement) >= 0)//to move the cube despite the bullet
                    {
                        nextGridIs = ObjectList.NextGridIs.EmptyGrid;
                        currentCommand = new ReplaceCommand(this, nextGrid, currentGrid);
                    }
                    else
                    {
                        nextGridIs = ObjectList.NextGridIs.Wall;
                    }
                }
                else if (nextGrid.openGrid)
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
            gridSystem.PlaceSolidObj_Limited(placedGrid.gridPosition, gameObject, ObjectList.CUBE);
        }

        public void UndoTheMove(MyGridXZ placedGrid)
        {
            NextMoveOnGridSystem(placedGrid);
            OnEndPushAnimation();
            transform.position = currentGrid.worldPosition+offSet;
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
            transform.position = currentGrid.worldPosition+offSet;
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

