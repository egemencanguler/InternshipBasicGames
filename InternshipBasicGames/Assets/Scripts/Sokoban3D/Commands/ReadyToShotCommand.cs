using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sokoban3D
{
    public class ReadyToShotCommand : ICommand
    {

        ITurret turret;
        int currentCounter;
        int nextCounter;

        public ReadyToShotCommand(ITurret turret,int current,int next)
        {
            this.turret = turret;
            currentCounter = current;
            nextCounter = next;
        }

        public void execute()
        {
            turret.ReadyToShoot(nextCounter);
        }

        public void undo()
        {
            turret.ReadyToShoot(currentCounter);
        }
    }

}

