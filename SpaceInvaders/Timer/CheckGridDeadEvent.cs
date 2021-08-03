using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class CheckGridDeadEvent : Command
    {
        public CheckGridDeadEvent(EnemyGrid pGrid)
        {
            this.pGrid = pGrid;
        }

        public override void Execute(float deltaTime)
        {
            this.pGrid.CheckGridDead();
        }

        EnemyGrid pGrid;
    }
}
