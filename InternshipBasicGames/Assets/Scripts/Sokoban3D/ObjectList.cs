using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class ObjectList
    {
        public enum NextGridIs { Player,Wall,Cube,CubeHolder,SAW,TurretBullet,EmptyGrid, None }
        public const string PLAYER = "player";
        public const string CUBE = "cube";
        public const string WALL = "wall";
        public const string CUBEHOLDER = "cubeholder";
        public const string TURRET = "turret";
        public const string TURRETBULLET = "turretbullet";
        public const string SAW = "saw";

    }
}


