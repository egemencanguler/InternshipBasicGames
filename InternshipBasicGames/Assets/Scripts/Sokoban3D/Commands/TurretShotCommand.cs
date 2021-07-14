using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class TurretShotCommand : ICommand
    {
        public ITurret shotCommand;

        public TurretShotCommand(ITurret shotCommand)
        {
            this.shotCommand = shotCommand;
        }

        public void execute()
        {
            shotCommand.ShootTheBullet();
        }

        public void undo()
        {
            shotCommand.RemoveTheBullet();
        }
    }
}

