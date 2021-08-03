using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class GameOverObserver : ColObserver
    {
        public GameOverObserver(EnemyGrid pGrid)
        {
            this.pGrid = pGrid;
        }

        public override void Notify()
        {
            //Game Over
            pGrid.StopGrid();
            ShipMan.AliensLanded();
        }

        EnemyGrid pGrid;
    }
}
