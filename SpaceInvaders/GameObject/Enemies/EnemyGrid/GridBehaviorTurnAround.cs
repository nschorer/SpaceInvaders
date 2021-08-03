using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class GridBehaviorTurnAround : GridBehavior
    {
        public GridBehaviorTurnAround(EnemyGrid pGrid)
            :base(pGrid)
        {
        }

        public override void Advance()
        {
            Handle();
        }

        public override void Handle()
        {
            pGrid.SetGridBehavior(EnemyGrid.State.MoveSideways);
            pGrid.Advance();
        }
    }
}
