using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceRace
{
    public class DebrisGenerator : MonoBehaviour
    {
        public Debris[] debrises = new Debris[24];
        [Range(1, 10)] public int maxBirth1Generate;
        [Range(1, 5)] public float generateIntervalTime;
        public int iterator; // TODO neden public
        public Vector2[] LeftPoints = new Vector2[2]; // TODO neden buyuk harfle basliyor, gizmoyla cizilmemis editlemesi zor,
                                                      // transform atamasida yapilabilirdi editlemeyi kolaylastirmak icin
                                                      
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
            // TODO timer -= Time.deltaTime - bu sekilde yazarsan sadece timer degiskeniyle olayi kurtariyosun start fonksyonunada gerek kalmiyor
            // if timer < 0 timer = Random.Range(0.5f, 2f);
            
            timer += Time.deltaTime;
            if (timer >= currentIntervalTime)
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
                // TODO degiskene random int atip secim yapmak yerine 
                // if (Random.value < 0.5f) yapabilirsin
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
}

