using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipStateMovementNoLeft : ShipState
    {
        public override void Handle(Ship pShip)
        {
            pShip.SetMovementState(ShipMan.State.MovementFree);
        }

        public override void MoveRight(Ship pShip)
        {
            pShip.x += pShip.shipSpeed;
            Handle(pShip);
        }

        public override void MoveLeft(Ship pShip)
        {
            // No left!
        }

        public override void ShootMissile(Ship pShip)
        {
            Debug.WriteLine("Movement state tried to call missile action");
            Debug.Assert(false);
        }
    }
}
