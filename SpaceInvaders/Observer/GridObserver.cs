using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class GridObserver : ColObserver
    {
        public GridObserver()
        {
             
        }
        public override void Notify()
        {
            //Debug.WriteLine("Grid_Observer: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            // OK do some magic
            EnemyGrid pGrid = (EnemyGrid)this.pSubject.pObjA;

            pGrid.HitWall();

            //WallCategory pWall = (WallCategory)this.pSubject.pObjB;
            //if (pWall.GetCategoryType() == WallCategory.Type.Right)
            //{
            //    pGrid.SetDelta(-2.0f);
            //}
            //else if (pWall.GetCategoryType() == WallCategory.Type.Left)
            //{
            //    pGrid.SetDelta(2.0f);
            //}
            //else
            //{
            //    Debug.Assert(false);
            //}

        }
    }
}
