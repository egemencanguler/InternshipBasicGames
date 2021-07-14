using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public interface ITurret
    {
        void ReadyToShoot(int counter);

        void ShootTheBullet();

        void RemoveTheBullet();
    }
}

