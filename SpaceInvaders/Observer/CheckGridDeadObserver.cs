using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class CheckGridDeadObserver : ColObserver
    {
        public CheckGridDeadObserver(EnemyGrid pGrid)
        {
            this.pGrid = pGrid;
        }
        public override void Notify()
        {
            TimerMan.Add(TimeEvent.Name.CheckGridDead, new CheckGridDeadEvent(pGrid), 0.01f);
        }

        EnemyGrid pGrid;
    }
}
