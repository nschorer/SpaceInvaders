using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipLeftWallObserver : ColObserver
    {
        public override void Notify()
        {
            Ship pShip = ShipMan.GetShip();
            pShip.SetMovementState(ShipMan.State.MovementNoLeft);
        }

        // data
    }
}
