using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShipDestroyedObserver : ColObserver
    {
        public ShipDestroyedObserver(ScenePlay pState)
        {
            this.pState = pState;
        }

        public override void Notify()
        {
            Ship pShip = ShipMan.GetShip();
            pShip.SetMovementState(ShipMan.State.MovementNone);
            pShip.SetMissileState(ShipMan.State.MissileNone);
            this.pState.PlayerDies(); // this handles stopping stuff now
            pShip.ShipDestroyed();    // this handles starting stuff again after a pause
        }

        // data
        ScenePlay pState = null;
    }
}
