using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class RemoveMissileObserverRHS : ShipRemoveMissileObserver
    {
        public override void Notify()
        {
            GameObject temp = this.pSubject.pObjA;
            this.pSubject.pObjA = this.pSubject.pObjB;
            this.pSubject.pObjB = temp;
            base.Notify();
        }
    }
}
