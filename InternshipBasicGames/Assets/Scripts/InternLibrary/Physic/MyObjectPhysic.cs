using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;

[RequireComponent(typeof(ObjectBounds))]
public class MyObjectPhysic : MonoBehaviour
{
    public enum Bodytype { Static, Dynamic }
    public Bodytype bodytype;
    public Vector2 currentPos;
    public Vector2 newPos;
    public Vector2 movement;
    public bool readyToMove;
    public ObjectBounds objBounds;

    public delegate void CollisionEvent();
    public event CollisionEvent On_Collision;

    void Start()
    {
        objBounds = GetComponent<ObjectBounds>();
    }

    // Update is called once per frame
    void Update()
    { 

    }

    public void DoCollisionEvent()
    {
        if(On_Collision != null)
        {
            On_Collision();
        }
    }

    public void SetNewPosition(Vector2 nextPos)
    {
        if(readyToMove == false)
        {
            newPos = nextPos;
            readyToMove = true;
        }
        
    }

    public void SetNewPositionAfterCollision(Vector2 nextPos)
    {
        newPos = nextPos;
    }


    public void SetMovement(Vector2 movement)
    {
        this.movement = movement;
    }

    public void GoNewPosition()
    {
        transform.position = newPos;
    }
}
