using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class TurretBullet : MonoBehaviour, IMoveableObjects
    {
        public MyGridSystemXZ gridSystem;
        public Vector3 offSet;
        public MyGridXZ nextGrid, currentGrid;
        public GameObject turretBullet;

        public Vector3 movementDirection = Vector3.zero;
        public float speed;
        public Vector3 movement;
        public float roadTaken;

        public ICommand currentCommand;
        public ObjectList.NextGridIs nextGridIs;
        public bool willHitPlayer;
        private Player player;


        private void OnEnable()
        {
            turretBullet = transform.GetChild(0).gameObject;
            gridSystem = FindObjectOfType<MyGridSystemXZ>();
            var gridPos = gridSystem.WorldPosToGridPos(transform.position);
            gridPos += movementDirection;
            currentGrid = gridSystem.GetGrid(gridPos);
            if (currentGrid.placedObjTag == ObjectList.PLAYER)// for the first movement
            {
                currentCommand = new WaitingCommand(this);
                player = currentGrid.placedObj.GetComponent<Player>();
                nextGridIs = ObjectList.NextGridIs.Player;
            }
            else if (currentGrid.openGrid)
            {
                nextGridIs = ObjectList.NextGridIs.EmptyGrid;
                ReplaceTheObject(currentGrid, false);
                var nextPlacedGrid = gridSystem.GetGrid(gridPos + movementDirection);
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
            }
            else if (!nextGrid.openGrid)
            {
                currentCommand = new WaitingCommand(this);
                ReplaceTheObject(currentGrid, true);
                nextGridIs = ObjectList.NextGridIs.Wall;
            }
        }

        void Update()
        {
            if (!nextGridIs.Equals(ObjectList.NextGridIs.None))//animation section
            {
                var pos = transform.position;
                roadTaken += speed * Time.deltaTime;
                movement = speed * Time.deltaTime * movementDirection;
                pos += movement;
                if (nextGridIs.Equals(ObjectList.NextGridIs.Wall))
                {
                    if (roadTaken >= gridSystem.gridSize / 2)
                    {
                        pos = currentGrid.worldPosition + offSet;
                        ResetMoveAnimation();
                        turretBullet.SetActive(false);
                    }
                }
                else if (nextGridIs.Equals(ObjectList.NextGridIs.Player) || willHitPlayer)
                {
                    if (roadTaken >= gridSystem.gridSize / 2)
                    {
                        pos = currentGrid.worldPosition + offSet;
                        ResetMoveAnimation();
                        turretBullet.SetActive(false);

                        //player hit animation
                        player.Die(movementDirection);
                        //
                        willHitPlayer = false;
                    }
                }
                else
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



        public void NextMoveOnGridSystem(MyGridXZ placedGrid)
        {
            nextGrid = placedGrid;

            
            if (nextGrid.placedObjTag == ObjectList.PLAYER || willHitPlayer)//coliding check
            {
                currentCommand = new WaitingCommand(this);
                ReplaceTheObject(currentGrid, true);
                player = nextGrid.placedObj.GetComponent<Player>();
                nextGridIs = ObjectList.NextGridIs.Player;
                Debug.Log("Player is death");
            }
            else if(!nextGrid.openGrid)
            {
                currentCommand = new WaitingCommand(this);
                ReplaceTheObject(currentGrid, true);
                nextGridIs = ObjectList.NextGridIs.Wall;
            }
            else
            {
                var nextPlacedGrid = gridSystem.GetGrid(nextGrid.gridPosition + movementDirection);
                ReplaceTheObject(nextGrid, false);
                currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
                nextGridIs = ObjectList.NextGridIs.EmptyGrid;
            }


        }

        public void Waiting()
        {
            Debug.Log(gameObject.name + " is waiting");
        }

        public void UndoTheMove(MyGridXZ placedGrid)
        {
            if (!turretBullet.activeSelf)
            {
                turretBullet.SetActive(true);
            }
            var nextPlacedGrid = gridSystem.GetGrid(placedGrid.gridPosition + movementDirection);
            ReplaceTheObject(placedGrid, false);
            currentCommand = new ReplaceCommand(this, nextPlacedGrid, currentGrid);
            ResetMoveAnimation();
            transform.position = currentGrid.worldPosition + offSet;
        }

        public void ResetMoveAnimation()
        {
            nextGridIs = ObjectList.NextGridIs.None;
            roadTaken = 0;
            nextGrid = new MyGridXZ();
        }

        public void ReplaceTheObject(MyGridXZ placedGrid, bool isDestroyed)
        {

            gridSystem.RemoveSolidObject_Limited(currentGrid.gridPosition);
            currentGrid = placedGrid;
            if (!isDestroyed)
            {
                gridSystem.PlaceSolidObj_Limited(placedGrid.gridPosition, gameObject, ObjectList.TURRETBULLET);
            }
        }
    }

}

