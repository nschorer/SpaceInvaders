using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class GridBehavior
    {
        public GridBehavior(EnemyGrid pGrid)
        {
            this.pGrid = pGrid;
        }

        public abstract void Handle();

        public abstract void Advance();

        public virtual void Unpause()
        {
            //this.pGrid = null;
            Debug.Print("Tried to unpause from a behavior other than GridBehaviorPause");
            Debug.Assert(false);
        }

        protected EnemyGrid pGrid;
    }
}
