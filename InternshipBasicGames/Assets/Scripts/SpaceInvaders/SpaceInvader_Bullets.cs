using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
public class SpaceInvader_Bullets : MonoBehaviour
{
    public float speed;
    Vector2 movement;
    public Vector2 movementDirection;
    Vector2 newPos;
    public ObjectBounds objBounds;

    void Start()
    {
        
    }

    void Update()
    {
        newPos = transform.position;
        movementDirection.Normalize();
        movement = movementDirection.normalized * speed * Time.deltaTime;
        newPos += movement;

        var hit = VektorProperties.ColliderIntersec((Vector2)transform.position, movement, SpaceInvaders_Gamecore.instance.alienBoundList, objBounds);
        if (hit != null && Vector2.Dot(hit.normal, movement.normalized) < 0)
        {
            if (hit.borderName == "Alien")
            {
                Destroy(gameObject);
            }
        }


        transform.position = newPos;
    }
}
