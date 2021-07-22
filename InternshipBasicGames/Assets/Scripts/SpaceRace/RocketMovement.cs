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
        // TODO kullanilmayan degisken ve fonkstonlari silmeyi aliskanlik haline getir
        public float speed;
        public Rect allowedArea; // TODO OnDrawGizmos da cizebilirsin bu sekilde hic anlami yok
        Vector2 movement;
        public Vector2 startPoint; // TODO neden public bolum tasarlayan birisi start positionin burdan ayarlandigini dusunebilir ama yaniltici olur, baska bir kodda erismiyor
        public Vector2 newPos;


        public int score = 0;
        public TextMeshProUGUI scoreText;
        public enum PlayerType { Player1, Player2 } // TODO gerekli biseye benzemiyor zaten scenede atama yaparken belirliyoruz player1 ve player 2 yi
                                                    // ayni seyi anlatan 2 farkli sey olmasi genelde buga neden olur ikisi uyusmadiginda
        /*
         * class InputSystem
         *     public Rocket rocket1;
         *     public Rocket rocket2;
         *
         *
         *     void Update
         *         HandleRocket1Input
         *         HandleRocket2Input
         *
         * 
         */
        
        
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
                // TODO no string const tanimla ya da kullanma 
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

            // TODO field olarak tanimladigin seyi localde tekrar tanimlamissin, benim ide uyariyor visual studioda yokmu
            // TODO sadece tusa basilinca hareket ediyoruz direction tricklerine ne gerek var direk tusa basildinin icersine eklenebilir
            
            /*
             *  bu sekilde yapinca yukari gidiyosam yukariyi asagi gidiyosam asagiyi kontrol ediyorum -
             *  topY ve bottomY awakete onceden hesaplanabilir
             * 
             *    if(InputMoveUp()
             *     var pos = transform.positions;
             *     pos += speed * Time.deltaTime * Vector3.up;
             *     if pos.y > topY
             *         Score()
             *
             *     if InputMoveDown()
             *     var pos = transform.positions
             *     pos += speed * Time.deltaTime * Vector3.down;
             *     pos.y = Mathf.Max(0,pos.y);     
             * 
             *
             * 
             */
            
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

