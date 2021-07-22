using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class Alien : MonoBehaviour
    {
        public bool canAttack; // TODO cok problemli ve karisik bi logic var burda
        
        /*
            class AlienController
                Alien[,] aliens
                
                for x = 0 x < sizeX x ++
                    for y = 0 y < sizeY y ++
                        if aliens[x,y] != null // Not killed en ondekini bul ve ondan ates et
                            ShootBullet(x,y);
                            break; 
         
         
         */
        
        
        public int numberAtAlienList;
        float timer;
        public int timeToAttackMax;
        Gamecore gameCore;

        [Header("Bullets")]
        public GameObject bullet;
        public float bulletSpeedMax;
        Vector2 bulletDirection = new Vector2(0, -1);



        private void Awake()
        {
            gameCore = FindObjectOfType<Gamecore>();
        }

        // Start is called before the first frame update
        void Start()
        {
            timer = Random.Range(1, timeToAttackMax + 1);
        }

        // Update is called once per frame
        void Update()
        {
            if (canAttack)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    var newbullet = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y - transform.localScale.y), Quaternion.identity).GetComponent<Bullets>();
                    newbullet.speed = Random.Range(3, bulletSpeedMax);
                    newbullet.movementDirection = bulletDirection;

                    timer = Random.Range(1, timeToAttackMax + 1);
                }

            }
        }

        public void Dead()
        {
            var alienListController = transform.GetComponentInParent<AliensController>();
            if (canAttack)
            {
                if (transform.GetSiblingIndex() - 1 >= 0)
                {
                    transform.parent.transform.GetChild(transform.GetSiblingIndex() - 1).GetComponent<Alien>().canAttack = true;
                }
            }

            gameCore.BulletCheckList.Remove(gameObject.GetComponent<ObjectBounds>());
            alienListController.AliensList.Remove(gameObject);
            Destroy(gameObject);


            if (alienListController.AliensList.Count == 0)
            {
                gameCore.GameOver(1);

            }
        }
    }

}

