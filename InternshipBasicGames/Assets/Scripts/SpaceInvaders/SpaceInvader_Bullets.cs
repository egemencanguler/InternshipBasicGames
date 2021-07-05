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
            if (hit.borderName == "Alien")
            {
                Destroy(gameObject);
                SpaceInvaders_Gamecore.instance.player.GetComponent<SpaceInvader_Player>().GetScore();
                hit.GetComponent<SpaceInvaders_Alien>().Dead();
                
            }
            else if(hit.borderName == "Player")
            {
                Destroy(gameObject);
                hit.GetComponent<SpaceInvader_Player>().TakeDamage();
            }
        }
        
        if(transform.position.y < -5f  || transform.position.y >5)
        {
            Destroy(gameObject);
        }


        transform.position = newPos;
    }
}
