using System;
using System.Diagnostics;

namespace SpaceInvaders
{

    class RemoveShipObserver : ColObserver
    {
        public RemoveShipObserver()
        {
            this.pShip = null;
        }
        public RemoveShipObserver(RemoveShipObserver s)
        {
            Debug.Assert(s != null);
            this.pShip = s.pShip;
        }

        public override void Notify()
        {
            // Delete missile
            //Debug.WriteLine("RemoveShipObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            this.pShip = (Ship)this.pSubject.pObjB;
            Debug.Assert(this.pShip != null);

            if (pShip.bMarkForDeath == false)
            {
                pShip.bMarkForDeath = true;
                //pSndEngine.SoundVolume = 0.2f;
                //IrrKlang.ISound pSnd = pSndEngine.Play2D("invaderkilled.wav");
                //   Delay
                RemoveShipObserver pObserver = new RemoveShipObserver(this);
                DelayedObjectMan.Attach(pObserver);
            }
        }
        public override void Execute()
        {
            this.pShip.Remove();
        }

        // -------------------------------------------
        // data:
        // -------------------------------------------

        private GameObject pShip;
    }
    
}
