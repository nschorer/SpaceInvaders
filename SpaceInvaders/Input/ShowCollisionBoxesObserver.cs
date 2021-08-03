using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    class ShowCollisionBoxesObserver : InputObserver
    {
        public override void Notify()
        {
            SpriteBatchMan.Find(SpriteBatch.Name.Boxes).ToggleRender();
        }
    }
}
