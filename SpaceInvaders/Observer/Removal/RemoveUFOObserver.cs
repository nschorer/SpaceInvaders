using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RemoveUFOObserver : ColObserver
    {
        public RemoveUFOObserver(bool useLHS = false)
        {
            this.pUFO = null;
            this.useLHS = useLHS;
        }

        public RemoveUFOObserver(RemoveUFOObserver u)
        {
            this.pUFO = u.pUFO;
        }

        public override void Notify()
        {
            // Delete missile
            //Debug.WriteLine("RemoveEnemyObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            // At this point we have two game objects
            // Actually we can control the objects in the visitor
            // Alphabetical ordering... A is missile,  B is wall

            // This cast will throw an exception if I'm wrong
            if (useLHS)
            {
                this.pUFO = (EnemyCategory)this.pSubject.pObjA;
            }
            else
            {
                this.pUFO = (EnemyCategory)this.pSubject.pObjB;
            }

            //Debug.WriteLine("MissileRemoveObserver: --> delete enemy {0}", pUFO);

            if (pUFO.bMarkForDeath == false)
            {
                pUFO.bMarkForDeath = true;

                // Delay - remove object later
                // TODO - reduce the new functions
                RemoveUFOObserver pObserver = new RemoveUFOObserver(this);
                DelayedObjectMan.Attach(pObserver);
            }
        }

        public override void Execute()
        {
            this.pUFO.Remove();
        }
        private bool privCheckParent(GameObject pObj)
        {
            GameObject pGameObj = (GameObject)Iterator.GetChild(pObj);
            if (pGameObj == null)
            {
                return true;
            }

            return false;
        }

        // data
        private GameObject pUFO;
        private bool useLHS;

    }
}
