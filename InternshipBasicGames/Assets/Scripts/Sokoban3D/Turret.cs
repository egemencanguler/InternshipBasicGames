using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Sokoban3D
{
    public class Turret : MonoBehaviour,ITurret
    {
        public MyGridSystemXZ gridSystem;
        public Vector3 offSet;
        public MyGridXZ nextGrid, currentGrid;
        public Player player;
        public int roundToShot;
        public int remainingRoundToShot;
        public TextMeshProUGUI shotCounterText;
        public GameObject turretBulletPrefab;
        public ICommand currentCommand;
        public Vector3 attackDirection;
        public List<GameObject> aliveBullet = new List<GameObject>();
        public int bulletIterator=-1;

        // Start is called before the first frame update
        void Start()
        {
            remainingRoundToShot = roundToShot;
            shotCounterText.text = "" + remainingRoundToShot;

            currentGrid = gridSystem.FindGridAccordingToWorldPos(transform.position);
            gridSystem.PlaceSolidObj_Limited(currentGrid.gridPosition, gameObject, ObjectList.TURRET);
            transform.position = currentGrid.worldPosition + offSet;
            currentCommand = new ReadyToShotCommand(this, remainingRoundToShot, remainingRoundToShot - 1);
        }


        public void ReadyToShoot(int counter)
        {
            remainingRoundToShot = counter;
            if(remainingRoundToShot == 1)
            {
                currentCommand = new TurretShotCommand(this);
            }
            else
            {
                currentCommand = new ReadyToShotCommand(this, remainingRoundToShot, remainingRoundToShot - 1);
            }
            shotCounterText.text = "" + remainingRoundToShot;
            
        }

        public void ShootTheBullet()
        {
            bulletIterator++;
            Vector3 initialPos = new Vector3(transform.position.x, 2, transform.position.z);

            turretBulletPrefab.SetActive(false);
            aliveBullet.Add (Instantiate(turretBulletPrefab, initialPos, Quaternion.identity));
            //to change script variable area
            var bulletScript = aliveBullet[bulletIterator].GetComponent<TurretBullet>();
            bulletScript.movementDirection = attackDirection;
            //bulletScript.FirstGridMovement();
            aliveBullet[bulletIterator].SetActive(true);
            turretBulletPrefab.SetActive(true);
            player.commandManager.turretBullets.Add(bulletScript);

            remainingRoundToShot = roundToShot;
            ReadyToShoot(remainingRoundToShot);
        }

        public void RemoveTheBullet()
        {

            remainingRoundToShot = 1;
            ReadyToShoot(remainingRoundToShot);

            var bulletScript = aliveBullet[bulletIterator].GetComponent<TurretBullet>();
            player.commandManager.turretBullets.Remove(bulletScript);
            gridSystem.RemoveSolidObject_Limited(bulletScript.currentGrid.gridPosition);

            Destroy(aliveBullet[bulletIterator]);
            aliveBullet.RemoveAt(bulletIterator);
            bulletIterator--;
            
        }
    }
}

