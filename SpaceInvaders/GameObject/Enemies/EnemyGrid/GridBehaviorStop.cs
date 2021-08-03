using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    // This class is a dead end for the grid state
    // An external source has to reset the grid state to one of the active ones

    class GridBehaviorStop : GridBehavior
    {
        public GridBehaviorStop(EnemyGrid pGrid)
            : base(pGrid)
        {
        }
        public override void Advance()
        {

        }

        public override void Handle()
        {

        }
    }
}
