using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class Player : MonoBehaviour, IMoveableObjects
    {
        public MyGridSystemXZ gridSystem;
        public Animator animator;
        public CommandManager commandManager;
        public MyGridXZ nextGrid = new MyGridXZ();
        public MyGridXZ currentGrid;

        //Mouse config
        private Vector2 startMousePos = new Vector2();
        private bool dragging;
        private enum MouseDirection { Left, Right, Up, Down, None }
        private MouseDirection mouseDirection = MouseDirection.None;
        //Movement
        public Vector3 movementDirection = Vector3.zero;
        public float speed;
        public Vector3 movement;
        public float roadTaken;
        //Commands
        public ICommand currentCommand;
        public bool turnEnding,playerIsDeath;
        public ObjectList.NextGridIs nextGridIs = ObjectList.NextGridIs.None;
        private Cubes draggingCube;
        //Ragdoll
        private Rigidbody[] ragdollBodies;
        private Collider[] ragdollColliders;
        private List<Vector3> ragdollInitialRotation = new List<Vector3>();
        private List<Vector3> ragdollInitialPosition = new List<Vector3>();

        private void Awake()
        {
            ragdollBodies = GetComponentsInChildren<Rigidbody>();
            ragdollColliders = GetComponentsInChildren<Collider>();
            FindInitialTransform();
            ToggleRagdoll(false);
        }


        void Start()
        {
            currentGrid = gridSystem.FindGridAccordingToWorldPos(transform.position);
            transform.position = currentGrid.worldPosition;
            nextGrid = new MyGridXZ();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && movementDirection == Vector3.zero)//to undo
            {
                if (playerIsDeath)
                {
                    ToggleRagdoll(false);
                    playerIsDeath = false;
                }
                
                commandManager.UndoCommands();
            }

            if (Input.GetMouseButtonUp(0))//to reset mouse movement
            {
                startMousePos = Vector2.zero;
                mouseDirection = MouseDirection.None;
                dragging = false;
            }

            if (movementDirection == Vector3.zero && !playerIsDeath) //to get mouse swipe
            {

                if (Input.GetMouseButtonDown(0))
                {
                    startMousePos = Input.mousePosition;
                    dragging = true;
                }

                if (Input.GetMouseButton(0) && dragging)
                {
                    var distance = (Vector2)Input.mousePosition - startMousePos;
                    if (distance.magnitude > 100)
                    {
                        float x = distance.x;
                        float y = distance.y;

                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            if (x < 0)
                            {
                                mouseDirection = MouseDirection.Left;
                            }
                            else
                            {
                                mouseDirection = MouseDirection.Right;
                            }
                        }
                        else
                        {
                            if (y < 0)
                            {
                                mouseDirection = MouseDirection.Down;
                            }
                            else
                            {
                                mouseDirection = MouseDirection.Up;
                            }
                        }
                        // mouseDirection = MouseDirection.None;
                        dragging = false;
                    }
                }

                if (mouseDirection.Equals(MouseDirection.Up))
                {
                    movementDirection = new Vector3(0, 0, 1);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (mouseDirection.Equals(MouseDirection.Down))
                {
                    movementDirection = new Vector3(0, 0, -1);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (mouseDirection.Equals(MouseDirection.Right))
                {
                    movementDirection = new Vector3(1, 0, 0);
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (mouseDirection.Equals(MouseDirection.Left))
                {
                    movementDirection = new Vector3(-1, 0, 0);
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                }


            } // Input Area

            if (nextGrid.gridPosition.x == -1 && movementDirection != Vector3.zero)//to check new grid position and colliding
            {
                var gridPos = currentGrid.gridPosition;
                gridPos += movementDirection;
                nextGrid = gridSystem.GetGrid(gridPos);
                ConfigureNextGridMove();
            }


            if (turnEnding)//to execute all object commands
            {
                commandManager.ExecuteAllCommand();
                turnEnding = false;
                
            }


            // Plays animations
            if (!nextGridIs.Equals(ObjectList.NextGridIs.None))
            {

                animator.SetBool("run", true);
                var pos = transform.position;
                roadTaken += speed * Time.deltaTime;
                movement = speed * Time.deltaTime * movementDirection;
                pos += movement;

                if (nextGridIs.Equals(ObjectList.NextGridIs.Wall))
                {
                    if (roadTaken >= gridSystem.gridSize / 2)
                    {
                        animator.SetTrigger("wallHit");
                        mouseDirection = MouseDirection.None;
                        ResetMoveAnimation();
                        pos = currentGrid.worldPosition;
                    }
                }
                else if (nextGridIs.Equals(ObjectList.NextGridIs.TurretBullet))
                {
                    if (roadTaken >= gridSystem.gridSize / 2)
                    {
                        mouseDirection = MouseDirection.None;
                        ResetMoveAnimation();
                        pos = currentGrid.worldPosition;
                    }
                }
                else if (nextGridIs.Equals(ObjectList.NextGridIs.SAW))
                {
                    if (roadTaken >= gridSystem.gridSize / 2)
                    {
                        mouseDirection = MouseDirection.None;
                        ResetMoveAnimation();
                        pos = currentGrid.worldPosition;
                    }
                }
                else if (nextGridIs.Equals(ObjectList.NextGridIs.Cube))
                {
                    if (draggingCube.nextGridIs.Equals(ObjectList.NextGridIs.EmptyGrid))
                    {
                        animator.SetBool("pushing", true);
                        draggingCube.PushMe(movement);
                        if (roadTaken >= gridSystem.gridSize)
                        {
                            draggingCube.OnEndPushAnimation();
                            pos = currentGrid.worldPosition;
                            ResetMoveAnimation();
                        }
                    }
                    else
                    {
                        if (roadTaken >= gridSystem.gridSize / 2)
                        {
                            animator.SetTrigger("wallHit");
                            mouseDirection = MouseDirection.None;
                            draggingCube.OnEndPushAnimation();
                            ResetMoveAnimation();
                            pos = currentGrid.worldPosition;
                        }
                    }
                }
                else
                {
                    if (roadTaken >= gridSystem.gridSize)
                    {
                        pos = currentGrid.worldPosition;
                        ResetMoveAnimation();

                    }
                }

                transform.position = pos;
            } 
         

        }

        #region Move Commands
        public void NextMoveOnGridSystem(MyGridXZ placedGrid)
        {
            gridSystem.RemoveSolidObject_Limited(currentGrid.gridPosition);
            currentGrid = placedGrid;
            gridSystem.PlaceSolidObj_Limited(placedGrid.gridPosition, gameObject, ObjectList.PLAYER);
            
        }

        public void UndoTheMove(MyGridXZ placedGrid)
        {
            NextMoveOnGridSystem(placedGrid);
            ResetMoveAnimation();
            transform.position = currentGrid.worldPosition;
        }

        public void Waiting()
        {
            Debug.Log("Player is waiting");
        }
        #endregion

        public void ResetMoveAnimation()
        {
            if (mouseDirection.Equals(MouseDirection.None))
            {
                animator.SetBool("run", false);
                animator.SetBool("pushing", false);
            }
            nextGridIs = ObjectList.NextGridIs.None;
            roadTaken = 0;
            movementDirection = Vector3.zero;
            nextGrid = new MyGridXZ();
        }


        public void ConfigureNextGridMove()
        {
            if (nextGrid.placedObjTag == ObjectList.WALL || nextGrid.placedObjTag == ObjectList.TURRET)
            {
                nextGridIs = ObjectList.NextGridIs.Wall;
            }
            else if(nextGrid.placedObjTag == ObjectList.TURRETBULLET)
            {
                var turretbullet = nextGrid.placedObj.GetComponent<TurretBullet>();
                Vector2 playerMovement = new Vector2(movementDirection.x, movementDirection.z);
                Vector2 turretbulletMovement = new Vector2(turretbullet.movementDirection.x, turretbullet.movementDirection.z);
                if (Vector2.Dot(playerMovement,turretbulletMovement) < 0)//if the opposite direction
                {
                    turretbullet.willHitPlayer = true;
                    nextGridIs = ObjectList.NextGridIs.TurretBullet;
                    currentCommand = new ReplaceCommand(this, currentGrid, currentGrid);
                    turnEnding = true;
                }
                else
                {
                    currentCommand = new ReplaceCommand(this, nextGrid, currentGrid);
                    turnEnding = true;
                    nextGridIs = ObjectList.NextGridIs.EmptyGrid;
                }
                
            }
            else if((nextGrid.placedObjTag == ObjectList.SAW))
            {
                
                var saw = nextGrid.placedObj.GetComponent<Saw>();
                Vector2 playerMovement = new Vector2(movementDirection.x, movementDirection.z);
                Vector2 sawMovement = new Vector2(saw.movementDirection[saw.lastMoveDirection].x, saw.movementDirection[saw.lastMoveDirection].z);
                if (Vector2.Dot(playerMovement, sawMovement) < 0)//if the opposite direction
                {
                    saw.willHitPlayer = true;
                    nextGridIs = ObjectList.NextGridIs.SAW;
                    currentCommand = new ReplaceCommand(this, currentGrid, currentGrid);
                    turnEnding = true;
                }
                else
                {
                    currentCommand = new ReplaceCommand(this, nextGrid, currentGrid);
                    turnEnding = true;
                    nextGridIs = ObjectList.NextGridIs.EmptyGrid;
                }
            }
            else if (nextGrid.placedObjTag == ObjectList.CUBE)
            {
                draggingCube = nextGrid.placedObj.GetComponent<Cubes>();
                draggingCube.GetNextGrid(movementDirection);
                if (draggingCube.nextGridIs.Equals(ObjectList.NextGridIs.EmptyGrid))
                {
                    currentCommand = new ReplaceCommand(this, nextGrid, currentGrid);
                    turnEnding = true;
                }
                nextGridIs = ObjectList.NextGridIs.Cube;
            }
            else
            {
                currentCommand = new ReplaceCommand(this, nextGrid, currentGrid);
                turnEnding = true;
                nextGridIs = ObjectList.NextGridIs.EmptyGrid;
            }// if next grid is open
        }


        #region For Raggdoll Methods

        public void FindInitialTransform()
        {
            foreach (Rigidbody rb in ragdollBodies)
            {
                ragdollInitialRotation.Add(rb.transform.localRotation.eulerAngles);
                ragdollInitialPosition.Add(rb.transform.localPosition);
            }
        }

        public void ToggleRagdoll(bool ctrl)
        {
            
            int counter = 0;
            foreach(Rigidbody rb in ragdollBodies)
            {
                rb.isKinematic = !ctrl;
                if (!ctrl)
                {
                    rb.gameObject.transform.localPosition = ragdollInitialPosition[counter];
                    rb.gameObject.transform.localRotation = Quaternion.Euler(ragdollInitialRotation[counter]);
                    
                    counter++;
                }
                
            }
            foreach (Collider coll in ragdollColliders)
            {
                coll.enabled = ctrl;
            }
            animator.enabled = !ctrl;
        }

        public void Die(Vector3 explosionPosition)
        {
            ToggleRagdoll(true);
            foreach(Rigidbody rb in ragdollBodies)
            {
                rb.AddForce(50f * explosionPosition, ForceMode.Impulse);
            }
            playerIsDeath = true;
        }

        #endregion

    }
}