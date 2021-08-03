using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class GridMovement : Command
    {


        public GridMovement(EnemyGrid pGrid)
        {
            this.pGrid = pGrid;
        }

        public override void Execute(float deltaTime)
        {
            pGrid.Advance();
            //TimerMan.Add(TimeEvent.Name.GridMovement, this, pGrid.GetGridSpeed());
            pGrid.QueueGridMovement();
        }

        private EnemyGrid pGrid;
    }
}
