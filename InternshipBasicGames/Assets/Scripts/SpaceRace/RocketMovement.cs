using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InternLibrary.Vektors;
using InternLibrary.Border;
using TMPro;

namespace SpaceRace
{
    public class RocketMovement : MonoBehaviour
    {
        public float speed;
        public Rect allowedArea;
        Vector2 movement;
        public Vector2 startPoint;
        public Vector2 newPos;


        public int score = 0;
        public TextMeshProUGUI scoreText;
        public enum PlayerType { Player1, Player2 }
        public PlayerType playerType;

        private void Awake()
        {
            startPoint = transform.position;
        }
        void Start()
        {

        }

        private void Update()
        {
            float direction=0;
            if (playerType.Equals(PlayerType.Player1))
            {
                direction = Input.GetAxisRaw("Vertical");
            }
            else
            {
                if (Input.GetKey(KeyCode.I))
                {
                    direction = 1;
                }
                else if (Input.GetKey(KeyCode.K))
                {
                    direction = -1;
                }
            }

            Vector2 newPos = transform.position;
            newPos.y += speed * direction * Time.deltaTime;

            if (newPos.y - transform.localScale.y / 2 < allowedArea.yMin)
            {
                newPos = transform.position;
            }
            else if (newPos.y + transform.localScale.y / 2 > allowedArea.yMax)
            {
                GoScore();
                newPos = startPoint;
            }

            /*if (!allowedArea.Contains(newPos))
            {

            }*/
            transform.position = newPos;
        }

        public void ResetTheRocket()
        {
            transform.position = startPoint;
        }

        public void GoScore()
        {
            score++;
            scoreText.text = "" + score;
        }



    }

}

