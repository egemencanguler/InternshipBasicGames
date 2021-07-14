using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class Saw : MonoBehaviour, IMoveableObjects
    {
        public MyGridSystemXZ gridSystem;
        public Vector3 offSet;

        public MyGridXZ nextGrid, currentGrid;

        public List<Vector3> movementDirection = new List<Vector3>();
        public int lastMoveDirection = 0;

        public float speed;
        public Vector3 movement;
        public float roadTaken;

        public ICommand currentCommand;
        public ObjectList.NextGridIs nextGridIs;

        public bool willHitPlayer, inversed;
        private Player player;
        void Start()
        {
            var gridPos = gridSystem.WorldPositionToGrid(transform.position);
            currentGrid = gridSystem.GetCurrentGrid(gridPos);
            ReplaceTheObject(currentGrid);
            transform.position = currentGrid.worldPosition + offSet;


            var nextPlacedGrid = gridSystem.GetCurrentGrid(gridPos + movementDirection[lastMoveDirection]);
            currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);

            nextGrid = new MyGridXZ();
        }

        // Update is called once per frame
        void Update()
        {
            if (!nextGridIs.Equals(ObjectList.NextGridIs.None))
            {
                var pos = transform.position;
                if (nextGridIs.Equals(ObjectList.NextGridIs.Wall))
                {

                    roadTaken += speed * Time.deltaTime;
                    if (roadTaken <= gridSystem.gridSize / 2)
                    {
                        movement = speed * Time.deltaTime * movementDirection[lastMoveDirection - 1];
                        pos += movement;
                    }
                    else if (roadTaken > gridSystem.gridSize / 2 && roadTaken <= gridSystem.gridSize)
                    {
                        movement = speed * Time.deltaTime * movementDirection[lastMoveDirection];
                        pos += movement;

                    }
                    else
                    {
                        if (willHitPlayer)
                        {
                            player.Die(movementDirection[lastMoveDirection]);
                            //
                            willHitPlayer = false;
                        }
                        pos = currentGrid.worldPosition + offSet;
                        ResetMoveAnimation();
                    }
                }
                else
                {
                    roadTaken += speed * Time.deltaTime;
                    movement = speed * Time.deltaTime * movementDirection[lastMoveDirection];
                    pos += movement;
                    if (nextGridIs.Equals(ObjectList.NextGridIs.Player) || willHitPlayer)
                    {
                        if (roadTaken >= gridSystem.gridSize / 2)
                        {
                            pos = currentGrid.worldPosition + offSet;
                            ResetMoveAnimation();
                            //player hit animation
                            player.Die(-player.movementDirection);
                            //
                            willHitPlayer = false;
                        }
                    }
                    else //grid is empty
                    {
                        if (roadTaken >= gridSystem.gridSize)
                        {
                            pos = currentGrid.worldPosition + offSet;
                            ResetMoveAnimation();

                        }
                    }
                }

                transform.position = pos;
            }
        }

        public void ReplaceTheObject(MyGridXZ placedGrid)
        {

            gridSystem.RemoveSolidObject_Limited(currentGrid.gridPosition);
            currentGrid = placedGrid;
            gridSystem.PlaceSolidObj_Limited(placedGrid.gridPosition, offSet.y, gameObject, ObjectList.SAW);

        }


        public void ResetMoveAnimation()
        {
            nextGridIs = ObjectList.NextGridIs.None;
            roadTaken = 0;
            nextGrid = new MyGridXZ();
        }

        public void NextMoveOnGridSystem(MyGridXZ placedGrid)
        {
            nextGrid = placedGrid;

            if (nextGrid.placedObjTag == ObjectList.PLAYER || willHitPlayer)
            {
                movementDirection.Add(movementDirection[lastMoveDirection]);
                lastMoveDirection++;
                var nextPlacedGrid = gridSystem.GetCurrentGrid(currentGrid.gridPosition + movementDirection[lastMoveDirection]);
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
                player = nextGrid.placedObj.GetComponent<Player>();
                ReplaceTheObject(nextGrid);

                nextGridIs = ObjectList.NextGridIs.Player;
                Debug.Log("Player is death");
            }
            else if (nextGrid.placedObjTag == ObjectList.WALL)
            {
                movementDirection.Add(-movementDirection[lastMoveDirection]);
                lastMoveDirection++;
                var nextPlacedGrid = gridSystem.GetCurrentGrid(currentGrid.gridPosition + movementDirection[lastMoveDirection]);
                if (currentGrid.placedObjTag == ObjectList.PLAYER)
                {
                    player = currentGrid.placedObj.GetComponent<Player>();
                    willHitPlayer = true;
                    Debug.Log("Player is death");
                }

                nextGridIs = ObjectList.NextGridIs.Wall;
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
                ReplaceTheObject(currentGrid);
            }
            else
            {
                movementDirection.Add(movementDirection[lastMoveDirection]);
                lastMoveDirection++;
                var nextPlacedGrid = gridSystem.GetCurrentGrid(nextGrid.gridPosition + movementDirection[lastMoveDirection]);
                ReplaceTheObject(nextGrid);
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
                nextGridIs = ObjectList.NextGridIs.EmptyGrid;
            }
        }

        public void UndoTheMove(MyGridXZ placedGrid)
        {
            lastMoveDirection--;
            movementDirection.RemoveAt(movementDirection.Count - 1);
            var nextPlacedGrid = gridSystem.GetCurrentGrid(placedGrid.gridPosition + movementDirection[lastMoveDirection]);
            ReplaceTheObject(placedGrid);
            currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
            ResetMoveAnimation();
            transform.position = currentGrid.worldPosition + offSet;
        }

        public void Waiting()
        {
            Debug.Log(gameObject.name + "is waiting");
        }
    }

}
