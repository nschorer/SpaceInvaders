using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipRightWallObserver : ColObserver
    {
        public override void Notify()
        {
            Ship pShip = ShipMan.GetShip();
            pShip.SetMovementState(ShipMan.State.MovementNoRight);
        }

        // data
    }
}
