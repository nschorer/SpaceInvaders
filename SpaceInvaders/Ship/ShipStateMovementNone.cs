using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipStateMovementNone : ShipState
    {
        public override void Handle(Ship pShip)
        {
            
        }

        public override void MoveRight(Ship pShip)
        {
            // No right!
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
