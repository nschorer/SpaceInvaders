using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipStateEnd : ShipState
    {
        public override void Handle(Ship pShip)
        {

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
            //Missile pMissile = ShipMan.ActivateMissile();

            //pMissile.SetPos(pShip.x, pShip.y);
            //pMissile.SetActive(true);
        }
    }
}
