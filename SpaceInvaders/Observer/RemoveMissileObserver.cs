using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RemoveMissileObserver : ColObserver
    {
        public RemoveMissileObserver(bool useRHS=false)
        {
            this.pMissile = null;
            this.useRHS = useRHS;
        }

        public RemoveMissileObserver(RemoveMissileObserver m)
        {
            this.pMissile = m.pMissile;
        }

        public override void Notify()
        {
            // Delete missile
            Debug.WriteLine("RemoveMissileObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            // At this point we have two game objects
            // Actually we can control the objects in the visitor
            // Alphabetical ordering... A is missile,  B is wall

            // This cast will throw an exception if I'm wrong
            if (useRHS)
            {
                this.pMissile = (Missile)this.pSubject.pObjA;
            }
            else
            {
                this.pMissile = (Missile)this.pSubject.pObjA;
            }
            Debug.WriteLine("MissileRemoveObserver: --> delete missile {0}", pMissile);

            if (pMissile.bMarkForDeath == false)
            {
                pMissile.bMarkForDeath = true;

                // Delay - remove object later
                // TODO - reduce the new functions
                RemoveMissileObserver pObserver = new RemoveMissileObserver(this);
                DelayedObjectMan.Attach(pObserver);
            }
        }

        public override void Execute()
        {
            // Let the gameObject deal with this... 
            this.pMissile.Remove();
        }

        // data
        private GameObject pMissile;
        private bool useRHS;

    }
}
