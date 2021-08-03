using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipStateMissileReady : ShipState
    {
        public override void Handle(Ship pShip)
        {
            pShip.SetMissileState(ShipMan.State.MissileFlying);
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
            Missile pMissile = ShipMan.ActivateMissile();

            pMissile.SetPos(pShip.x, pShip.y+20);
            pMissile.SetActive(true);

            // switch states
            this.Handle(pShip);
        }

    }
}
