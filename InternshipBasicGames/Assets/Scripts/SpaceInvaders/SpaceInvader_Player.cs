using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvader_Player : MonoBehaviour
{
    public float movementSpeed = 1;
    public Rect allowedArea;

    [Header("Bullets")]
    public GameObject bullet;
    public Transform attackPoint;
    public float bulletSpeed;
    Vector2 bulletDirection = new Vector2(0,1);
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        Vector2 newPos = transform.position;
        newPos.x += movementSpeed * direction * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
           var newbullet = Instantiate(bullet, attackPoint.position, Quaternion.identity).GetComponent<SpaceInvader_Bullets>();
            newbullet.speed = bulletSpeed;
            newbullet.movementDirection = bulletDirection;
        }

        transform.position = newPos;
    }
}
