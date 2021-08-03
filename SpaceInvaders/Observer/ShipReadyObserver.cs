using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShipReadyObserver : ColObserver
    {
        public override void Notify()
        {
            Ship pShip = ShipMan.GetShip();
            pShip.GetMissileState().Handle(pShip); // this is bad
        }

        // data


    }
}
