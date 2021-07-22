using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpaceInvaders
{
    public class Player : MonoBehaviour
    {
        // TODO gorev bolumu yine guzel degil, hareket, score ayarlama, olme hersey burda adimizda player
        
        public float movementSpeed = 1;
        public Rect allowedArea;
        public int lives;
        public int score;
        public TextMeshProUGUI livesText;
        public TextMeshProUGUI scoreText;

        [Header("Bullets")]
        public GameObject bullet;
        public Transform attackPoint;
        public float bulletSpeed;
        Vector2 bulletDirection = new Vector2(0, 1); // TODO cok tanimlanmasi gereken biseymi emin degilim yukari ates ediyoruz

        Gamecore gameCore;
        void Start()
        {
            gameCore = FindObjectOfType<Gamecore>();
        }

        // Update is called once per frame
        void Update()
        {
            float direction = Input.GetAxisRaw("Horizontal"); // TODO no string 
            Vector2 newPos = transform.position;
            newPos.x += movementSpeed * direction * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // TODO bulletin icinde bi setup fonksyonu olabilirdi belki
                var newbullet = Instantiate(bullet, attackPoint.position, Quaternion.identity).GetComponent<Bullets>();
                newbullet.speed = bulletSpeed;
                newbullet.movementDirection = bulletDirection;
            }

            transform.position = newPos;
        }

        public void GetScore()
        {
            score += 10;

            scoreText.text = "Score: " + score;
        }

        public void TakeDamage()
        {
            lives--;

            livesText.text = "Lives: " + lives;

            if (lives <= 0)
            {
                gameCore.GameOver(-1);
            }
        }
    }

}

