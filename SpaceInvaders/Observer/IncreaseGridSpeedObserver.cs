using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class IncreaseGridSpeedObserver : ColObserver
    {
        public IncreaseGridSpeedObserver(EnemyGrid pGrid)
        {
            this.pGrid = pGrid;
        }
        public override void Notify()
        {
            pGrid.IncreaseGridSpeed();
        }

        EnemyGrid pGrid;
    }
}
