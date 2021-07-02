using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;
public class Pong_Ball : MonoBehaviour
{


    [SerializeField] private float ballSpeed;
    Vector2 movement;
    public Vector2 movementDirection;
    Vector2 newPos;

    public ObjectBounds objectbounds;

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        newPos = transform.position;
        movementDirection.Normalize();
        movement = movementDirection.normalized * ballSpeed * Time.deltaTime;
        newPos += movement;

        var hit = VektorProperties.ColliderIntersec((Vector2)transform.position, movement, Pong_GameCore.instance.objectsBounds, objectbounds);
        if (hit != null && Vector2.Dot(hit.normal, movement.normalized) < 0)
        {
            if(hit.borderName == "Wall" || hit.borderName == "Ball")
            {
                float d = Vector2.Dot(movementDirection.normalized, hit.normal);
                movementDirection = movementDirection - (hit.normal * (d * 2.0f));
                newPos = hit.hitPoint + (new Vector2(hit.normal.x * 0.001f, hit.normal.y * 0.001f));
            }
            else if(hit.borderName == "LeftScore" || hit.borderName == "RightScore")
            {

                Pong_GameCore.instance.GoScore(hit.borderName);
                ResetTheBall();
            }
        }
        
        transform.position = newPos;

        
    }


    public void ResetTheBall()
    {
        newPos = Vector2.zero;
        transform.position = Vector2.zero;
    }



    private void OnDrawGizmos()
    {
        //Debug.DrawRay((Vector2)transform.position, movementDirection.normalized * ballSpeed * Time.deltaTime, Color.red);
        Debug.DrawRay((Vector2)transform.position, movementDirection.normalized * ballSpeed, Color.red);

        Borders[] borders = new Borders[4];
        borders = objectbounds.UpdateBorderAndReturn();

        Debug.DrawRay(borders[0].p1, movementDirection.normalized * ballSpeed, Color.green);
        Debug.DrawRay(borders[1].p1, movementDirection.normalized * ballSpeed, Color.green);
        Debug.DrawRay(borders[2].p1, movementDirection.normalized * ballSpeed, Color.green);
        Debug.DrawRay(borders[3].p1, movementDirection.normalized * ballSpeed, Color.green);
    }

}