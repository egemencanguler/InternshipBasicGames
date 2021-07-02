using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;
using System;

public class SpaceRace_Debris : MonoBehaviour
{
    [SerializeField] private float speed;
    Vector2 movement;
    public Vector2 movementDirection;
    Vector2 newPos;
    public Vector2 destroyingPointsLeft, destroyingPointsRight;

    public GameObject player1;
    public GameObject player2;

    void Start()
    {
        player1 = SpaceRace_Gamecore.instance.player1;
        player2 = SpaceRace_Gamecore.instance.player2;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = transform.position;
        movementDirection.Normalize();
        movement = movementDirection.normalized * speed * Time.deltaTime;
        newPos += movement;



        if (newPos.x - transform.localScale.x / 2 < player1.transform.position.x + (player1.transform.localScale.x/2) &&
            newPos.x + transform.localScale.x / 2 >  player1.transform.position.x - (player1.transform.localScale.x/2) &&
            newPos.y - transform.localScale.y/2 < player1.transform.position.y + (player1.transform.localScale.y/2) &&
            newPos.y + transform.localScale.y/2 > player1.transform.position.y - (player1.transform.localScale.y/2))
        {
            player1.GetComponent<SpaceRace_RocketMovement>().ResetTheRocket();
        }
        else if (newPos.x - transform.localScale.x / 2 < player2.transform.position.x + (player2.transform.localScale.x/2) &&
            newPos.x + transform.localScale.x/2 > player2.transform.position.x - (player2.transform.localScale.x/2) &&
            newPos.y - transform.localScale.y / 2 < player2.transform.position.y + (player2.transform.localScale.y/2) &&
            newPos.y + transform.localScale.y / 2 > player2.transform.position.y - (player2.transform.localScale.y/2))
        {
            player2.GetComponent<SpaceRace_RocketMovement>().ResetTheRocket();
        }
        else if (newPos.x < destroyingPointsLeft.x || newPos.x > destroyingPointsRight.x)
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