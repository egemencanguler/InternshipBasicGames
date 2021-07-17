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

        public bool willHitPlayer;
        private Player player;
        void Start()
        {
            currentGrid = gridSystem.FindGridAccordingToWorldPos(transform.position);
            ReplaceTheObject(currentGrid);
            transform.position = currentGrid.worldPosition + offSet;

            var gridPos = currentGrid.gridPosition;
            var nextPlacedGrid = gridSystem.GetGrid(gridPos + movementDirection[lastMoveDirection]);
            currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);

            nextGrid = new MyGridXZ();
        }

        // Update is called once per frame
        void Update()
        {
            //Plays Animation
            if (!nextGridIs.Equals(ObjectList.NextGridIs.None))
            {
                var pos = transform.position;

                roadTaken += speed * Time.deltaTime;
                movement = speed * Time.deltaTime * movementDirection[lastMoveDirection];
                pos += movement;

                if (nextGridIs.Equals(ObjectList.NextGridIs.Wall))
                {
                    pos = currentGrid.worldPosition + offSet;
                    if (willHitPlayer)
                    {
                        player.Die(-player.movementDirection);

                        willHitPlayer = false;
                    }
                    ResetMoveAnimation();
                }

                else if (nextGridIs.Equals(ObjectList.NextGridIs.Player) || willHitPlayer)
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


                transform.position = pos;
            }
        }

        public void ReplaceTheObject(MyGridXZ placedGrid)
        {

            gridSystem.RemoveSolidObject_Limited(currentGrid.gridPosition);
            currentGrid = placedGrid;
            gridSystem.PlaceSolidObj_Limited(placedGrid.gridPosition, gameObject, ObjectList.SAW);

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

            if (nextGrid.placedObjTag == ObjectList.PLAYER || willHitPlayer)//coliding check
            {
                movementDirection.Add(movementDirection[lastMoveDirection]);
                lastMoveDirection++;
                var nextPlacedGrid = gridSystem.GetGrid(currentGrid.gridPosition + movementDirection[lastMoveDirection]);
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
                player = nextGrid.placedObj.GetComponent<Player>();
                ReplaceTheObject(nextGrid);

                nextGridIs = ObjectList.NextGridIs.Player;
                Debug.Log("Player is death");
            }
            else if (nextGrid.placedObjTag == ObjectList.WALL || nextGrid.placedObjTag == ObjectList.CUBE)//here checking the collisions of the cube and the wall and their bounces
            {
                movementDirection.Add(-movementDirection[lastMoveDirection]);
                lastMoveDirection++;
                nextGrid = gridSystem.GetGrid(currentGrid.gridPosition + movementDirection[lastMoveDirection]);
                if (nextGrid.placedObjTag == ObjectList.CUBE || nextGrid.placedObjTag == ObjectList.WALL)// if there is a cube or wall behind it
                {
                    nextGrid = currentGrid;
                    if (nextGrid.placedObjTag == ObjectList.PLAYER)//if the player is the on of obj
                    {
                        player = currentGrid.placedObj.GetComponent<Player>();
                        willHitPlayer = true;
                    }

                    currentCommand = new WaitingCommand(this);
                    nextGridIs = ObjectList.NextGridIs.Wall;
                }
                else
                {
                    if (nextGrid.placedObjTag == ObjectList.PLAYER)//if the player is behind of obj
                    {
                        player = nextGrid.placedObj.GetComponent<Player>();
                        willHitPlayer = true;
                    }
                    else if (currentGrid.placedObjTag == ObjectList.PLAYER)//if the player came in the opposite direction after the saw's bounce
                    {
                        player = currentGrid.placedObj.GetComponent<Player>();
                        Vector2 playerMovement = new Vector2(player.movementDirection.x, player.movementDirection.z);
                        Vector2 sawMovement = new Vector2(movementDirection[lastMoveDirection].x, movementDirection[lastMoveDirection].z);
                        if (Vector2.Dot(playerMovement, sawMovement) < 0)
                        {

                            willHitPlayer = true;
                        }
                    }
                    nextGridIs = ObjectList.NextGridIs.EmptyGrid;
                }

                var nextPlacedGrid = gridSystem.GetGrid(nextGrid.gridPosition + movementDirection[lastMoveDirection]);
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, nextGrid);
                ReplaceTheObject(nextGrid);



            }
            else
            {
                movementDirection.Add(movementDirection[lastMoveDirection]);
                lastMoveDirection++;
                var nextPlacedGrid = gridSystem.GetGrid(nextGrid.gridPosition + movementDirection[lastMoveDirection]);
                ReplaceTheObject(nextGrid);
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
                nextGridIs = ObjectList.NextGridIs.EmptyGrid;
            }
        }

        public void UndoTheMove(MyGridXZ placedGrid)
        {
            lastMoveDirection--;
            movementDirection.RemoveAt(movementDirection.Count - 1);
            var nextPlacedGrid = gridSystem.GetGrid(placedGrid.gridPosition + movementDirection[lastMoveDirection]);
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
