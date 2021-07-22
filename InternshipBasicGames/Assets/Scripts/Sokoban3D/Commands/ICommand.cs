using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public interface ICommand
    {
        // TODO Execute Undo
        void execute();

        void undo();
    }
}

