using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{


    [SerializeField] private float ballSpeed;
    [SerializeField] Rect allowedArea;
    Vector2 velocity;
    Vector2 movementDirection;
    Vector2 newPos;
    private Bounds bounds;
    private Player player1, player2;
    private bool collisionEnable;
    private void Awake()
    {
        collisionEnable = true;
        bounds = GetComponent<Renderer>().bounds;
        movementDirection = new Vector2(1, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        player1 = GameCore.instance.Get_Player1().GetComponent<Player>();
        player2 = GameCore.instance.Get_Player2().GetComponent<Player>();
    }
    private void OnDisable()
    {
        transform.position = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        bounds = GetComponent<Renderer>().bounds;
        Movement();
    }

    public void Movement()
    {
        newPos = transform.position;
        velocity = movementDirection * ballSpeed * Time.deltaTime;
        newPos += velocity;
        CollisionCheck();
        if (newPos.y < allowedArea.yMin)
        {
            newPos.y = allowedArea.yMin;
            movementDirection.y = -movementDirection.y;
            velocity.y = -velocity.y;
        }
        else if (newPos.y > allowedArea.yMax)
        {
            newPos.y = allowedArea.yMax;
            movementDirection.y = -movementDirection.y;
            velocity.y = -velocity.y;
        }
        else if(newPos.x < allowedArea.xMin)
        {
            collisionEnable = false;
            GameCore.instance.GoScore(-1);
        }
        else if(newPos.x > allowedArea.xMax)
        {
            collisionEnable = false;
            GameCore.instance.GoScore(1);
        }
        
        
        transform.position = newPos;
    }

    public void CollisionCheck()
    {
        if (collisionEnable)
        {
            CollisionWithPlayer1();
            CollisionWithPlayer2();
        }
    }


    public void CollisionWithPlayer1()
    {
        if (newPos.x - (bounds.size.x / 2) < player1.Get_bounds().max.x && CheckPaddleCollision(player1) && 
            Vector2.Dot(player1.Get_bounds().ClosestPoint(transform.position).normalized, velocity.normalized) > 0)
        {
            if ((player1.Get_bounds().ClosestPoint(transform.position).y == player1.Get_bounds().max.y ||
                player1.Get_bounds().ClosestPoint(transform.position).y == player1.Get_bounds().min.y) && newPos.x + (bounds.size.x / 2) > player1.Get_bounds().min.x)
            {
                newPos.y = player1.Get_bounds().ClosestPoint(transform.position).y;
                movementDirection.y = -movementDirection.y;
                velocity.y = -velocity.y;
            }
            else if (player1.Get_bounds().ClosestPoint(transform.position).x == player1.Get_bounds().max.x)
            {
                newPos.x = player1.Get_bounds().ClosestPoint(transform.position).x + (bounds.size.x / 2);
                movementDirection.x = -movementDirection.x;
                velocity.x = -velocity.x;
            }
        }

    }

    public void CollisionWithPlayer2()
    {
        if (newPos.x + (bounds.size.x / 2) > player2.Get_bounds().min.x && CheckPaddleCollision(player2) && 
            Vector2.Dot(player2.Get_bounds().ClosestPoint(transform.position).normalized, velocity.normalized) > 0)
        {
            if ((player2.Get_bounds().ClosestPoint(transform.position).y == player2.Get_bounds().max.y ||
                player2.Get_bounds().ClosestPoint(transform.position).y ==  player2.Get_bounds().min.y) && newPos.x - (bounds.size.x / 2) < player2.Get_bounds().max.x)
            {
                newPos.y = player2.Get_bounds().ClosestPoint(transform.position).y;
                movementDirection.y = -movementDirection.y;
                velocity.y = -velocity.y;
            }
            else if(player2.Get_bounds().ClosestPoint(transform.position).x == player2.Get_bounds().min.x)
            {
                newPos.x = player2.Get_bounds().ClosestPoint(transform.position).x - (bounds.size.x / 2);
                movementDirection.x = -movementDirection.x;
                velocity.x = -velocity.x;
            }
            
        }
    }

    public void ResetTheBall()
    {
        newPos = Vector2.zero;
        transform.position = Vector2.zero;
        collisionEnable = true;
    }

    public bool CheckPaddleCollision(Player player)
    {
        return newPos.y > player.Get_bounds().min.y && newPos.y < player.Get_bounds().max.y;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, bounds.size);
    }

}