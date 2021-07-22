using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Border;
using InternLibrary.Vektors;

namespace SnakeGridless
{
    public class Snake : MonoBehaviour
    {
        [SerializeField] private float speed;
        private float currentSpeed; // TODO local
        Vector2 movement;
        public Vector2 movementDirection;
        Vector2 newPos;
        public ObjectBounds objBound;

        public ColliderManager colManager;


        public Rect gameScreenBorder;
        public List<GameObject> mySnakeList = new List<GameObject>();
        public int snakeListIterator = 0;
        public GameObject snakeTailPrefab;

        public List<Vector2> snakePath = new List<Vector2>();
        public List<int> snakePathIterator = new List<int>();
        public float snakeTailDistance = 0.5f;
        public bool slowDown;

        void Start()
        {
            mySnakeList.Add(gameObject);
            snakePathIterator.Add(-1);
        }

        // Update is called once per frame
        void Update()
        {
            // TODO
            /*
                if Input.KeyDown
                    slowDown = !slowDown
                    if slowDown
                        targetFps =    
                    else 
                        targetFps = 
                    
                    daha temiz
                    
                    targetFps = slowDown ?? 10 : 300 // daha da kisaltmak istersen 
             
             */
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!slowDown)
                {
                    Application.targetFrameRate = 10;
                    slowDown = true;
                }
                else
                {
                    Application.targetFrameRate = 300;
                    slowDown = false;
                }
            }

            newPos = transform.position;

            Vector2 worldPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movementDirection = worldPositionMouse - (Vector2)transform.position;
            if (Vector2.Distance(worldPositionMouse, (Vector2)transform.position) <= objBound.rect.width / 2) 
            {
                movementDirection = Vector2.zero;
            }// if height and width rects is equals and the mouse is in that area,it stops

            movementDirection.Normalize();
            currentSpeed = speed * Time.deltaTime; // TODO current speed degil bu movement


            movement = movementDirection.normalized * currentSpeed;
            newPos += movement;
            
            Debug.DrawLine(transform.position, newPos,Color.green, 100);

            if (movementDirection != Vector2.zero)
            {
                snakePath.Add(newPos);
                snakePathIterator[0]++;
            }

            if (newPos.x < gameScreenBorder.xMin)
            {
                newPos.x = gameScreenBorder.xMax;
            }
            else if (newPos.x > gameScreenBorder.xMax)
            {
                newPos.x = gameScreenBorder.xMin;
            }
            else if (newPos.y < gameScreenBorder.yMin)
            {
                newPos.y = gameScreenBorder.yMax;
            }
            else if (newPos.y > gameScreenBorder.yMax)
            {
                newPos.y = gameScreenBorder.yMin;
            }




            var hit = VektorProperties.FoundCollidedGameObject((Vector2)transform.position, movement, colManager.snakeCollideWithList, objBound);
            if (hit != null)//collider check
            {
                if (hit.objectTag.Equals(ObjectTagList.ObjectTags.Food))
                {
                    colManager.snakeCollideWithList.Remove(hit);
                    Destroy(hit.gameObject);

                  
                    var tail = Instantiate(snakeTailPrefab, mySnakeList[snakeListIterator].transform.position, Quaternion.identity);
                    mySnakeList.Add(tail);
                    colManager.snakeCollideWithList.Add(tail.GetComponent<ObjectBounds>());

                    snakeListIterator++;
                }

            }




            if (mySnakeList.Count >= 2) 
            {
                for (int i = 1; i < mySnakeList.Count; i++)
                {
                    if (movementDirection != Vector2.zero)
                    {
                        if (mySnakeList[i] == mySnakeList[snakeListIterator] && snakePathIterator.Count == mySnakeList.Count - 1)
                        {
                            if (Vector2.Distance(mySnakeList[snakeListIterator].transform.position, mySnakeList[snakeListIterator - 1].transform.position) < snakeTailDistance)
                            {
                                continue;
                            }
                            else
                            {
                                var newdistance = snakeTailDistance;
                                for (int j = snakePathIterator[snakeListIterator - 1]; j > 0; j--)
                                {
                                    Vector2 p1, p2;

                                    p1 = snakePath[j];
                                    p2 = snakePath[j - 1];

                                    if (Vector2.Distance(p1, p2) < newdistance)
                                    {
                                        newdistance -= Vector2.Distance(p1, p2);
                                        continue;
                                    }
                                    else
                                    {
                                        snakePathIterator.Add(j);

                                        break;
                                    }
                                }
                            }
                        }// for the newly created tail

                        // TODO karisik anlamadim 2 foodu ayni anda alinca patliyor - snakePathIterator neden var?
                        Vector2 newTailPos = Vector2.zero;

                        var distance = snakeTailDistance;

                        for (int k = snakePathIterator[i - 1]; k > 0; k--)
                        {
                            Vector2 p1, p2, direction;

                            p1 = snakePath[k];
                            p2 = snakePath[k - 1];

                            if (k == snakePathIterator[i - 1] && i != 1)
                            {
                                p1 = mySnakeList[i - 1].transform.position;
                            }
                            if (Vector2.Distance(p1, p2) < distance)
                            {
                                distance -= Vector2.Distance(p1, p2);
                                continue;
                            }
                            else
                            {
                                direction = p2 - p1;
                                var remainingDistance = direction.normalized * distance;
                                newTailPos = mySnakeList[i].transform.position;


                                if ((p2 + remainingDistance).x < gameScreenBorder.xMin ||
                                    (p2 + remainingDistance).x > gameScreenBorder.xMax ||
                                    (p2 + remainingDistance).y < gameScreenBorder.yMin ||
                                     (p2 + remainingDistance).y > gameScreenBorder.yMax)
                                {
                                    var diff = snakePath[k - 1] - snakePath[k - 2];
                                    newTailPos += currentSpeed * (diff.normalized);
                                }
                                else
                                {
                                    newTailPos = p1 + remainingDistance;
                                    snakePathIterator[i] = k;
                                }
                                break;
                            }
                        }
                        mySnakeList[i].transform.position = newTailPos;
                    }

                }

            }// tails movement

            transform.position = newPos;
        }


        private void OnDrawGizmos()
        {

            Vector2 worldPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movementDirection = worldPositionMouse - (Vector2)transform.position;

            //Debug.DrawRay((Vector2)transform.position + movementDirection.normalized, movementDirection.normalized * speed * Time.deltaTime, Color.red);
            Debug.DrawRay((Vector2)transform.position + (0.5f * movementDirection.normalized), movementDirection.normalized * speed, Color.red);

            Borders[] borders = new Borders[4];
            borders = objBound.UpdateBorderAndReturn();

            Debug.DrawRay(borders[0].p1, movementDirection.normalized * speed, Color.green);
            Debug.DrawRay(borders[1].p1, movementDirection.normalized * speed, Color.green);
            Debug.DrawRay(borders[2].p1, movementDirection.normalized * speed, Color.green);
            Debug.DrawRay(borders[3].p1, movementDirection.normalized * speed, Color.green);




        }


    }
}

