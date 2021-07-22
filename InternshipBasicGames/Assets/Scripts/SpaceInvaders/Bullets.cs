using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;

namespace SpaceInvaders
{
    public class Bullets : MonoBehaviour
    {
        public float speed;
        Vector2 movement; // TODO neden local degil
        public Vector2 movementDirection;
        Vector2 newPos;// TODO neden local degil
        public ObjectBounds objBounds;
        Gamecore gameCore;

        void Awake()
        {
            gameCore = FindObjectOfType<Gamecore>();
        }

        void Update()
        {
            newPos = transform.position;
            movementDirection.Normalize();
            movement = movementDirection.normalized * speed * Time.deltaTime;
            newPos += movement;

            var hit = VektorProperties.FoundCollidedGameObject((Vector2)transform.position, movement, gameCore.BulletCheckList, objBounds);
            if (hit != null)
            {
                if (hit.objectTag.Equals(ObjectTagList.ObjectTags.Alien))
                {
                    Destroy(gameObject);
                    gameCore.player.GetComponent<Player>().GetScore();
                    hit.GetComponent<Alien>().Dead(); // TODO alien.Kill()

                }
                else if (hit.objectTag.Equals(ObjectTagList.ObjectTags.Player))
                {
                    Destroy(gameObject);
                    hit.GetComponent<Player>().TakeDamage();
                }
            }

            // TODO sihirli sayilar
            if (transform.position.y < -5f || transform.position.y > 5)
            {
                Destroy(gameObject);
            }


            transform.position = newPos;
        }
    }
}


