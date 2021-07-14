using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class ReplaceCommand : ICommand
{
        IMoveableObjects moveableObjects;
        MyGridXZ placedGrid;
        MyGridXZ currentGrid;
        public ReplaceCommand(IMoveableObjects obj,MyGridXZ placedGrid,MyGridXZ currentGrid)
        {
            moveableObjects = obj;
            this.placedGrid = placedGrid;
            this.currentGrid = currentGrid;
        }

        public void execute()
        {
            moveableObjects.NextMoveOnGridSystem(placedGrid);
        }

        public void undo()
        {
            moveableObjects.UndoTheMove(currentGrid);
        }
    }

}

