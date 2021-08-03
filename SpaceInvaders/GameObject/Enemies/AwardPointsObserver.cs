using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class AwardPointsObserver : ColObserver
    {
     public AwardPointsObserver(bool useLHS = false)
        {
            this.useLHS = useLHS;
        }
        
        public override void Notify()
        {
            // This cast will throw an exception if I'm wrong

            EnemyCategory pEnemy;

            if (useLHS)
            {
                pEnemy = (EnemyCategory)this.pSubject.pObjA;
            }
            else {
                pEnemy = (EnemyCategory)this.pSubject.pObjB;
            }

            int points = pEnemy.GetPointValue();
            ScoreBoard.AddToScore(points);
            //PointMan.AddPoints();
            //Debug.Print("+" + points);
        }

        bool useLHS;
    }
}
