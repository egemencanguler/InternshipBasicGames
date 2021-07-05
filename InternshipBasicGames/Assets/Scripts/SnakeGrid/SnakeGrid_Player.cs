using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGrid_Player : MonoBehaviour
{
    public Vector2 pos;
    public MyGrid nextGrid;
    public Vector2 newDirection;
    public Vector2 direction;
    public float speed;
    public Vector2 velocityLimit;
    void Start()
    {
        nextGrid = MyGridSystem.instance.GetCurrentGrid(pos);
        transform.position = nextGrid.worldPosition;
    }

    // Update is called once per frame
    void Update()
    {
        pos = MyGridSystem.instance.WorldPositionToGrid(transform.position);

        if (direction.x != 0 && Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            newDirection.y = Input.GetAxisRaw("Vertical");
            newDirection.x = 0;
            direction = newDirection;
            velocityLimit = Vector2.zero;
        }
        else if (direction.y != 0 && Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            newDirection.x = Input.GetAxisRaw("Horizontal");
            newDirection.y = 0;
            direction = newDirection;
            velocityLimit = Vector2.zero;
        }
        else
        {

            velocityLimit += speed * direction.normalized * Time.deltaTime;
        }

       





        if (Mathf.Abs( velocityLimit.x) >= MyGridSystem.instance.gridSize || Mathf.Abs(velocityLimit.y) >= MyGridSystem.instance.gridSize)
        {
            if(Mathf.Abs(velocityLimit.x) >= 1)
            {
                pos.x += direction.normalized.x;
            }
            else
            {
                pos.y += direction.normalized.y;
            }
            velocityLimit = Vector2.zero;


            nextGrid = MyGridSystem.instance.GetCurrentGridInfinitly(ref pos);

            if (nextGrid.placedObj != null)
            {
                Destroy(nextGrid.placedObj);
            }

            transform.position = nextGrid.worldPosition;


            
        }

        

        

    }
}
