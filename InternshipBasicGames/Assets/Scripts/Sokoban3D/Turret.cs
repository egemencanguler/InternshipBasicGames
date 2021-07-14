using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Sokoban3D
{
    public class Turret : MonoBehaviour,ITurret
    {
        public MyGridSystemXZ gridSystem;
        public float yOffset;
        public MyGridXZ nextGrid, currentGrid;
        public Player player;
        public int maxTurretShot;
        public int roundToShot;
        public TextMeshProUGUI shotCounterText;
        public GameObject turretBulletPrefab;
        public ICommand currentCommand;
        public Vector3 attackDirection;
        public List<GameObject> aliveBullet = new List<GameObject>();
        public int bulletIterator=-1;

        // Start is called before the first frame update
        void Start()
        {
           // roundToShot = maxTurretShot;
            shotCounterText.text = "" + roundToShot;
            var gridPos = gridSystem.WorldPositionToGrid(transform.position);
            currentGrid = gridSystem.GetCurrentGrid(gridPos);
            gridSystem.PlaceSolidObj_Limited(gridPos, yOffset, gameObject, ObjectList.TURRET);
            currentCommand = new ReadyToShotCommand(this, roundToShot, roundToShot - 1);
        }


        public void ReadyToShoot(int counter)
        {
            roundToShot = counter;
            if(roundToShot == 1)
            {
                
                Debug.Log("Shot");
                currentCommand = new TurretShotCommand(this);
            }
            else
            {
                currentCommand = new ReadyToShotCommand(this, roundToShot, roundToShot - 1);
            }
            shotCounterText.text = "" + roundToShot;
            
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
           // bulletScript.FirstGridMovement();
            //
            aliveBullet[bulletIterator].SetActive(true);
            turretBulletPrefab.SetActive(true);
            player.commandManager.turretBullets.Add(bulletScript);

            roundToShot = 3;
            ReadyToShoot(roundToShot);
        }

        public void RemoveTheBullet()
        {

            roundToShot = 1;
            ReadyToShoot(roundToShot);

            var bulletScript = aliveBullet[bulletIterator].GetComponent<TurretBullet>();
            player.commandManager.turretBullets.Remove(bulletScript);
            gridSystem.RemoveSolidObject_Limited(bulletScript.currentGrid.gridPosition);

            Destroy(aliveBullet[bulletIterator]);
            aliveBullet.RemoveAt(bulletIterator);
            bulletIterator--;
            
        }
    }
}

