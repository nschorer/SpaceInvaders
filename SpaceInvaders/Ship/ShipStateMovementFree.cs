using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipStateMovementFree : ShipState
    {
        public override void Handle(Ship pShip)
        {
            Debug.WriteLine("ShipStateMovementFree.Handle() Not implemented");
            Debug.Assert(false);
        }

        public override void MoveRight(Ship pShip)
        {
            pShip.x += pShip.shipSpeed;
        }

        public override void MoveLeft(Ship pShip)
        {
            pShip.x -= pShip.shipSpeed;
        }

        public override void ShootMissile(Ship pShip)
        {
            Debug.WriteLine("Movement state tried to call missile action");
            Debug.Assert(false);
        }
    }
}
