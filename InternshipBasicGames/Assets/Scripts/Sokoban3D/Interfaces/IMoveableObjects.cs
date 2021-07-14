using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public interface IMoveableObjects
    {
        void NextMoveOnGridSystem(MyGridXZ placedGrid);

        void UndoTheMove(MyGridXZ placedGrid);
        void Waiting();
    }
}

