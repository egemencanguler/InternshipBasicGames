using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;
using System;

namespace SpaceRace
{
    public class Debris : MonoBehaviour
    {
        public float speed;
        Vector2 movement; // TODO local tanimlanabilcek bir degisken neden burda
        public Vector2 movementDirection;
        Vector2 newPos; // TODO local tanimlanabilcek bir degisken neden burda
        public float destroyingPointsLeft = -6.3f, destroyingPointsRight = 6.3f;

        public GameObject player1;
        public GameObject player2;
        
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            newPos = transform.position;
            movementDirection.Normalize();
            movement = movementDirection.normalized * speed * Time.deltaTime;
            newPos += movement;


            // TODO ayni kod iki kere yazilmis player 1 ve player 2 icin CheckCollision(Transform playerTransform, Vector2 playerSize)
            // TODO local scali size olarak kullanmak guzel bir aliskanlik degil
            /*
                class Rocket
                    public Vector2 size;
             */

            if (newPos.x - transform.localScale.x / 2 < player1.transform.position.x + (player1.transform.localScale.x / 2) &&
                newPos.x + transform.localScale.x / 2 > player1.transform.position.x - (player1.transform.localScale.x / 2) &&
                newPos.y - transform.localScale.y / 2 < player1.transform.position.y + (player1.transform.localScale.y / 2) &&
                newPos.y + transform.localScale.y / 2 > player1.transform.position.y - (player1.transform.localScale.y / 2))
            {
                player1.GetComponent<RocketMovement>().ResetTheRocket();
            }
            else if (newPos.x - transform.localScale.x / 2 < player2.transform.position.x + (player2.transform.localScale.x / 2) &&
                newPos.x + transform.localScale.x / 2 > player2.transform.position.x - (player2.transform.localScale.x / 2) &&
                newPos.y - transform.localScale.y / 2 < player2.transform.position.y + (player2.transform.localScale.y / 2) &&
                newPos.y + transform.localScale.y / 2 > player2.transform.position.y - (player2.transform.localScale.y / 2))
            {
                player2.GetComponent<RocketMovement>().ResetTheRocket();
            }
            else if (newPos.x < destroyingPointsLeft || newPos.x > destroyingPointsRight)
            {
                gameObject.SetActive(false);
            }

            transform.position = newPos;
        }


        private void OnDrawGizmos()
        {
            Debug.DrawRay((Vector2)transform.position, movementDirection.normalized * speed, Color.red);
        }


    }

}

