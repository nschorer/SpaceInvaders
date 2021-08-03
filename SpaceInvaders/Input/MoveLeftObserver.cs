using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class MoveLeftObserver : InputObserver
    {
        public override void Notify()
        {
            //Debug.WriteLine("Move Left");
            Ship pShip = ShipMan.GetShip();
            pShip.MoveLeft();
        }
    }
}
