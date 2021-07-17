using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokoban3D
{
    [System.Serializable]
    public class CommandManager: MonoBehaviour
    {
        List<ICommand> thisTurnCommand = new List<ICommand>();
        List<ICommand> pastCommands = new List<ICommand>();
        public List<int> numberOfObjects = new List<int>();
        public int pastNumberOfObjects;
        public Player player;
        public Cubes[] cubes;
        public Turret[] turrets;
        public List<TurretBullet> turretBullets = new List<TurretBullet>();
        public Saw[] saws;

        private void Awake()
        {
            player = FindObjectOfType<Player>();
            cubes = FindObjectsOfType<Cubes>();
            turrets = FindObjectsOfType<Turret>();
            saws = FindObjectsOfType<Saw>();
        }

        public void ExecuteAllCommand()
        {
            pastNumberOfObjects = 0;
            FindAllCommand();

            if (player.currentCommand != null)
            {
                for (int i = 0; i < thisTurnCommand.Count; i++)
                {
                    pastCommands.Add(thisTurnCommand[i]);
                    thisTurnCommand[i].execute();
                }
            }

            thisTurnCommand.Clear();
        }

        public void UndoCommands()
        {
            if (pastCommands != null && pastCommands.Count > 0)
            {
                for (int k = numberOfObjects[numberOfObjects.Count - 1]; k > 0; k--)
                {

                    pastCommands[pastCommands.Count - 1].undo();
                    pastCommands.RemoveAt(pastCommands.Count - 1);

                }
                numberOfObjects.RemoveAt(numberOfObjects.Count - 1);
            }

        }

        public void FindAllCommand()
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                thisTurnCommand.Add(cubes[i].currentCommand);
                pastNumberOfObjects++;
            }

            thisTurnCommand.Add(player.currentCommand);
            pastNumberOfObjects++;


            for (int i = 0; i < turrets.Length; i++)
            {
                thisTurnCommand.Add(turrets[i].currentCommand);
                pastNumberOfObjects++;
            }

            for (int i = 0; i < turretBullets.Count; i++)
            {
                thisTurnCommand.Add(turretBullets[i].currentCommand);
                pastNumberOfObjects++;
            }

            for (int i = 0; i < saws.Length; i++)
            {
                thisTurnCommand.Add(saws[i].currentCommand);
                pastNumberOfObjects++;
            }

            numberOfObjects.Add(pastNumberOfObjects);
        }
    }

}

