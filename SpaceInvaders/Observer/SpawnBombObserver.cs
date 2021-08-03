using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class SpawnBombObserver : ColObserver
    {
        public SpawnBombObserver() { }

        public override void Notify()
        {
            //Debug.Print("SpawnBombObserver");
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);
            pGrid.SetBombReady();
        }
    }
}
