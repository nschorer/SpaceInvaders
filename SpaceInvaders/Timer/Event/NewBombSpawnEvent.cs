using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class NewBombSpawnEvent : Command
    {
        public NewBombSpawnEvent()
        {

        }

        public override void Execute(float deltaTime)
        {
            //Debug.Print("NewBombSpawnEvent");
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);
            pGrid.SetBombReady();
        }
    }
}
