using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class Gamecore : MonoBehaviour
    {
        // TODO hic bisey yapmayan bi class
        // TODO static instance kullanma demistim ama bu heryerde GameObject.FindByType kullan demek degil
        
        public List<ObjectBounds> BulletCheckList = new List<ObjectBounds>(); // TODO neden buyuk harf
        public GameObject player;
        public GameObject WonPanel;
        public GameObject LosePanel;
        private void Awake()
        {
            BulletCheckList.Add(player.GetComponent<ObjectBounds>());
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            

        }

        public void GameOver(int status)
        {
            if (status == 1)
            {
                // TODO Time.timeScale = 0  guzel bir yontem degil, ya soyle bi kod varsa -
                // var a = 5 / Time.deltaTime; deltatime 0 oldugu icin a NaN olacak 
                Time.timeScale = 0; 
                WonPanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 0;
                LosePanel.SetActive(true);
            }
        }

    }
}


