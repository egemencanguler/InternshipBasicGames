using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGrid
{
    public class SnakeHead : MonoBehaviour
    {
        public Vector2 pos;
        public MyGrid nextGrid;
        public Vector2 direction;
        public float speed;
        public float timeToStep;

        public List<GameObject> mySnake = new List<GameObject>();
        public int snakeListIterator;
        public GameObject snakeTailPrefab;
        public GameObject foodPrefab;

        private void Awake()
        {
            mySnake.Add(gameObject);
        }
        void Start()
        {
            nextGrid = MyGridSystem.instance.GetCurrentGrid(pos);
            transform.position = nextGrid.worldPosition;
            GenerateFood();
        }


        void Update()
        {
            pos = MyGridSystem.instance.WorldPositionToGrid(transform.position);


            if (direction.x != 0 && Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0)
            {

                direction.y = Input.GetAxisRaw("Vertical");
                direction.x = 0;
            }
            else if (direction.y != 0 && Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                direction.x = Input.GetAxisRaw("Horizontal");
                direction.y = 0;
            }

            timeToStep += speed * Time.deltaTime;

            if (timeToStep >= 1)
            {

                pos += direction.normalized;
                timeToStep = 0;


                nextGrid = MyGridSystem.instance.GetCurrentGridInfinitly(ref pos);



                if (nextGrid.placedObj != null)//collider check
                {
                    if (nextGrid.placedObjTag == "food")
                    {
                        Destroy(nextGrid.placedObj);
                        var tailPos = mySnake[snakeListIterator].transform.position;
                        var tail = Instantiate(snakeTailPrefab, tailPos, Quaternion.identity);
                        snakeListIterator++;
                        mySnake.Add(tail);
                        GenerateFood();
                    }
                    else if (nextGrid.placedObjTag == "tail")
                    {
                        Time.timeScale = 0;
                    }

                }

                if (mySnake.Count >= 2)//tail movement
                {
                    for (int i = mySnake.Count - 1; i >= 1; i--)
                    {
                        if (i == mySnake.Count - 1)
                        {
                            var gPos = MyGridSystem.instance.WorldPositionToGrid(mySnake[i].transform.position);
                            MyGridSystem.instance.RemoveTheObjectFromGrid(gPos);
                        }
                        ReplaceTails(mySnake[i], mySnake[i - 1]);
                    }
                }

                transform.position = nextGrid.worldPosition;

            }



        }

        public void GenerateFood()
        {
            Vector2 foodPos;
            foodPos.x = Random.Range(0, MyGridSystem.instance.cols);
            foodPos.y = Random.Range(0, MyGridSystem.instance.rows);

            while (MyGridSystem.instance.GetCurrentGrid(foodPos).placedObj != null)
            {
                foodPos.x = Random.Range(0, MyGridSystem.instance.cols);
                foodPos.y = Random.Range(0, MyGridSystem.instance.rows);

            }
            var food = Instantiate(foodPrefab);
            MyGridSystem.instance.PlaceTheObjToGrid(foodPos, food, "food");

        }

        public void ReplaceTails(GameObject currentTail, GameObject prevTail)
        {
            var prevTailGPos = MyGridSystem.instance.WorldPositionToGrid(prevTail.transform.position);
            //var currentTailGPos = MyGridSystem.instance.WorldPositionToGrid(currentTail.transform.position);
            MyGridSystem.instance.RemoveTheObjectFromGrid(prevTailGPos);
            MyGridSystem.instance.PlaceTheObjToGrid(prevTailGPos, currentTail, "tail");

        }
    }


}
