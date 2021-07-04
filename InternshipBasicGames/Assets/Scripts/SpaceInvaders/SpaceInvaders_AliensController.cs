using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;

public class SpaceInvaders_AliensController : MonoBehaviour
{
    public Vector2 alienNumber;
    public Vector2 aliensIntervalOffset;
    public Vector2 sizeOfGrupBounds = new Vector2();

    public ObjectBounds objBounds;
    public GameObject alienPrefab;
    public Vector2 iteratorPoints;


    public List<GameObject> AliensList = new List<GameObject>();
    private int alienListNumber = 0;

    public ObjectBounds[] checkCollisionWith = new ObjectBounds[1];

    [Header("Movement")]
    public  float speed;
    Vector2 movement;
    public Vector2 movementDirection;
    Vector2 newPos;

    private void Awake()
    {
        
    }

    void Start()
    {
        GenerateAlienGroup();
    }

    // Update is called once per frame
    void Update()
    {
        newPos = transform.position;
        movementDirection.Normalize();
        movement = movementDirection.normalized * speed * Time.deltaTime;
        newPos += movement;

        var hit = VektorProperties.ColliderIntersec((Vector2)transform.position, movement, checkCollisionWith, objBounds);
        Debug.Log(hit);
        if (hit != null && Vector2.Dot(hit.normal, movement.normalized) < 0)
        {
            if(hit.borderName == "Wall")
            {
                movementDirection.x = -movementDirection.x;
                newPos = hit.hitPoint + (new Vector2(hit.normal.x * 0.001f, (hit.normal.y * 0.001f) - 0.1f));
            }
        }

        transform.position = newPos;
    }

    public void GenerateAlienGroup()
    {
        objBounds.scaleBounds = sizeOfGrupBounds;

        objBounds.ConfigureBorder();

        objBounds.UpdateBorders();
        iteratorPoints = objBounds.corners[1];
        var alienBounds = alienPrefab.GetComponent<ObjectBounds>();
        iteratorPoints.x += alienBounds.rect.width / 2 ;
        iteratorPoints.y -= alienBounds.rect.height / 2;




        for (int i = 0; i < alienNumber.x; i++)
        {
            for (int k = 0; k < alienNumber.y; k++)
            {
                AliensList.Add(Instantiate(alienPrefab, iteratorPoints, Quaternion.identity,transform));
                SpaceInvaders_Gamecore.instance.BulletCheckList.Add(AliensList[alienListNumber].GetComponent<ObjectBounds>());
                AliensList[alienListNumber].GetComponent<SpaceInvaders_Alien>().numberAtAlienList = alienListNumber;
                if (k == alienNumber.y - 1)
                {
                    AliensList[alienListNumber].GetComponent<SpaceInvaders_Alien>().canAttack = true;
                }
                alienListNumber++;
                iteratorPoints.y -= aliensIntervalOffset.y;
                

            }

            iteratorPoints.y = objBounds.corners[1].y;
            iteratorPoints.x += alienBounds.rect.width / 2 + aliensIntervalOffset.x;
            iteratorPoints.y -= alienBounds.rect.height / 2;
        }


        
    }


    private void OnDrawGizmos()
    {
        objBounds.scaleBounds = sizeOfGrupBounds;

        objBounds.ConfigureBorder();

        objBounds.UpdateBorders();
        iteratorPoints = objBounds.corners[1];
        var alienBounds = alienPrefab.GetComponent<ObjectBounds>();
        iteratorPoints.x += alienBounds.rect.width / 2;
        iteratorPoints.y -= alienBounds.rect.height / 2;


        for (int i = 0; i < alienNumber.x; i++)
        {
            for (int k = 0; k < alienNumber.y; k++)
            {
                Gizmos.DrawWireCube(iteratorPoints, alienBounds.rect.size);
                iteratorPoints.y -= aliensIntervalOffset.y;
            }

            iteratorPoints.y = objBounds.corners[1].y;
            iteratorPoints.x += alienBounds.rect.width / 2+  aliensIntervalOffset.x;
            iteratorPoints.y -= alienBounds.rect.height / 2;
        }
        Gizmos.DrawWireCube(transform.position, sizeOfGrupBounds);

        /////
        //Debug.DrawRay((Vector2)transform.position, movementDirection.normalized * ballSpeed * Time.deltaTime, Color.red);
        Debug.DrawRay((Vector2)transform.position, movementDirection.normalized * speed, Color.red);

        Borders[] borders = new Borders[4];
        borders = objBounds.UpdateBorderAndReturn();

        Debug.DrawRay(borders[0].p1, movementDirection.normalized * speed, Color.green);
        Debug.DrawRay(borders[1].p1, movementDirection.normalized * speed, Color.green);
        Debug.DrawRay(borders[2].p1, movementDirection.normalized * speed, Color.green);
        Debug.DrawRay(borders[3].p1, movementDirection.normalized * speed, Color.green);
    }
}
