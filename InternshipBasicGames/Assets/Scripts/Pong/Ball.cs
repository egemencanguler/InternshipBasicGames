using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;

namespace Pong
{
    public class Ball : MonoBehaviour
    {


        public float ballSpeed;
        Vector2 movement;
        public Vector2 movementDirection;
        Vector2 newPos;

        public ObjectBounds objectbounds;
        public GameCore gameCore;

        private void Start()
        {
            objectbounds = GetComponent<ObjectBounds>();
            gameCore = FindObjectOfType<GameCore>();
        }

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
            

            var hit = VektorProperties.ColliderIntersec((Vector2)transform.position, movement, gameCore.objectsBounds, objectbounds);
            if (hit != null && Vector2.Dot(hit.normal, movement.normalized) < 0)
            {
                if (hit.objectTag.Equals(ObjectTagList.ObjectTags.Wall) || hit.objectTag.Equals(ObjectTagList.ObjectTags.Ball))
                {
                    float d = Vector2.Dot(movementDirection.normalized, hit.normal);
                    movementDirection = movementDirection - (hit.normal * (d * 2.0f));
                    newPos = hit.hitPoint + (new Vector2(hit.normal.x * 0.001f, hit.normal.y * 0.001f));
                }
                else if (hit.objectTag.Equals(ObjectTagList.ObjectTags.LeftScore) || hit.objectTag.Equals(ObjectTagList.ObjectTags.RightScore))
                {

                    gameCore.GoScore(hit.objectTag);
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
}

