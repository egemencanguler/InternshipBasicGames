using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    public class WaitingCommand : ICommand
    {
        IMoveableObjects moveableObjects;

        public WaitingCommand(IMoveableObjects obj)
        {
            moveableObjects = obj;
        }

        public void execute()
        {
            moveableObjects.Waiting();
        }

        public void undo()
        {
            moveableObjects.Waiting();
        }
    }
}

