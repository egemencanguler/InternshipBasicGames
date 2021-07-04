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

        var hit = VektorProperties.FoundCollidedGameObject((Vector2)transform.position, movement, SpaceInvaders_Gamecore.instance.BulletCheckList, objBounds);
        if (hit != null)
        {
            Debug.Log(hit.borderName);
            if (hit.borderName == "Alien")
            {
                Destroy(gameObject);
                hit.GetComponent<SpaceInvaders_Alien>().Dead();
                
            }
            else if(hit.borderName == "Player")
            {
                Destroy(hit.gameObject);
            }
        }
        
        if(transform.position.y < -5f  || transform.position.y >5)
        {
            Destroy(gameObject);
        }


        transform.position = newPos;
    }
}
