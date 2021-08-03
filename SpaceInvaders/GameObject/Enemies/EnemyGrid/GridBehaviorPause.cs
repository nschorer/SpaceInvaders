using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class GridBehaviorPause : GridBehavior
    {
        public GridBehaviorPause(EnemyGrid pGrid)
            : base(pGrid)
        {
            this.pLastBehavior = null;
        }
        public override void Advance()
        {
            // Do nothing -- we're paused!
        }

        public override void Handle()
        {

            pGrid.SetGridBehavior(this.pLastBehavior);
            //this.pLastBehavior.Advance();
            this.pLastBehavior = null;

            //TimerMan.Add(TimeEvent.Name.GridMovement, new GridMovement(pGrid), pGrid.GetGridSpeed()); // we need to start this up again
            //pGrid.QueueGridMovement();
        }

        public override void Unpause()
        {
            Handle();
        }

        public void RememberLastBehavior(GridBehavior pBehavior)
        {
            // What happens if we pass in GridBehaviorNone?
            pLastBehavior = pBehavior;
        }

        GridBehavior pLastBehavior;
    }
}
