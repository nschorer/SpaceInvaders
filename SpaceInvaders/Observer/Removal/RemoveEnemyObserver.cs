using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RemoveEnemyObserver : ColObserver
    {
        public RemoveEnemyObserver(bool useLHS = false)
        {
            this.pEnemy = null;
            this.useLHS = useLHS;
        }

        public RemoveEnemyObserver(RemoveEnemyObserver e)
        {
            this.pEnemy = e.pEnemy;
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
                this.pEnemy = (EnemyCategory)this.pSubject.pObjA;
            }
            else
            {
                this.pEnemy = (EnemyCategory)this.pSubject.pObjB;
            }

            if (pEnemy.bMarkForDeath == false)
            {
                pEnemy.bMarkForDeath = true;

                // Delay - remove object later
                // TODO - reduce the new functions
                RemoveEnemyObserver pObserver = new RemoveEnemyObserver(this);
                DelayedObjectMan.Attach(pObserver);
            }
        }

        public override void Execute()
        {
            //  if this brick removed the last child in the column, then remove column
            // Debug.WriteLine(" brick {0}  parent {1}", this.pBrick, this.pBrick.pParent);
            GameObject pA = (GameObject)this.pEnemy;
            GameObject pB = (GameObject)Iterator.GetParent(pA);

            this.pEnemy.Remove();        // remove enemy

            // TODO: Need a better way... 
            if (privCheckParent(pB) == true)
            {
                ((EnemyColumn)pB).Remove();  // if column is empty, remove column
            }
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
        private GameObject pEnemy;
        private bool useLHS;

    }
}
