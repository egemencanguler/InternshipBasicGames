using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public interface ICommand
    {
        void execute();

        void undo();
    }
}

