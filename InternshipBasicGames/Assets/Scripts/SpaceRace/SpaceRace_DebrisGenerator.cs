using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRace_DebrisGenerator : MonoBehaviour
{
    public SpaceRace_Debris[] debrises = new SpaceRace_Debris[24];
    [Range(1, 10)] public int maxBirth1Generate;
    [Range(1,5)] public float generateIntervalTime;
    public int iterator;
    public Vector2[] LeftPoints = new Vector2[2];
    public Vector2[] RightPoints = new Vector2[2];

    float timer;
    float currentIntervalTime;
    private void Awake()
    {
    }

    void Start()
    {
        currentIntervalTime = Random.Range(0, generateIntervalTime);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= currentIntervalTime)
        {
            int randBirthNumber = Random.Range(1, maxBirth1Generate);
            GenerateDebris(randBirthNumber);
            timer = 0;
            currentIntervalTime = Random.Range(0, generateIntervalTime);
        }
    }

    // Generates debrises
    public void GenerateDebris(int birthNumberAt1Time)
    {
        for (int i = 0; i < birthNumberAt1Time; i++)
        {
            int leftOrRight = Random.Range(0, 2);
            float initialPointY, initialPointX, movementDirection;
            if (leftOrRight == 0) //left
            {
                initialPointY = Random.Range(LeftPoints[1].y, LeftPoints[0].y);
                initialPointX = LeftPoints[0].x;
                movementDirection = 1;
            }
            else
            {
                initialPointY = Random.Range(RightPoints[1].y, RightPoints[0].y);
                initialPointX = RightPoints[0].x;
                movementDirection = -1;

            }

            if (!debrises[iterator].gameObject.activeSelf)
            {
                debrises[iterator].movementDirection.x = movementDirection;
                debrises[iterator].transform.position = new Vector2(initialPointX, initialPointY);
                debrises[iterator].gameObject.SetActive(true);
                
            }
            iterator++;
            if (iterator >= debrises.Length)
            {
                iterator = 0;
            }

        }
    }
}
