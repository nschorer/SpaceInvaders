using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class SpawnNewShip : Command
    {
        public SpawnNewShip()
        {
        }

        public override void Execute(float deltaTime)
        {
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);

            if (ShipMan.GetLives() > 0)
            {
                ShipMan.SpawnShip();
                //pGrid.SetPaused(false);
                pGrid.Unpause();
                pGrid.SetBombReady();
                //pGrid.QueueGridMovement();
            }
        }
    }
}
