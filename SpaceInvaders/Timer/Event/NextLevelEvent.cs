using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class NextLevelEvent : Command
    {
        public NextLevelEvent(EnemyGrid pGrid)
        {
            this.pGrid = pGrid;
        }
        public override void Execute(float deltaTime)
        {
            pGrid.NextLevel();
        }

        EnemyGrid pGrid;
    }
}
