using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipStateMissileNone : ShipState
    {
        public override void Handle(Ship pShip)
        {
            
        }

        public override void MoveRight(Ship pShip)
        {
            Debug.WriteLine("Missile state tried to call movement action");
            Debug.Assert(false);
        }

        public override void MoveLeft(Ship pShip)
        {
            Debug.WriteLine("Missile state tried to call movement action");
            Debug.Assert(false);
        }

        public override void ShootMissile(Ship pShip)
        {

        }

    }
}
