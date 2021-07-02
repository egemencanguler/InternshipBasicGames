using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong_Player : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 1;
    [SerializeField] Rect allowedArea;


    void Update()
    {
        
        Movement();
    }

    void Movement()
    {
        float direction = Input.GetAxisRaw("Vertical");
        Vector2 newPos = transform.position;
        newPos.y += movementSpeed * direction * Time.deltaTime;
        if (!allowedArea.Contains(newPos))
        {
            newPos = transform.position;
        }
        transform.position = newPos;
    }

}
