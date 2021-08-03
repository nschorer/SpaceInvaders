using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class KeyRightObserver : InputObserver
    {
        public override void Notify()
        {
            SceneContext.GetState().InputRight();
        }
    }
}
