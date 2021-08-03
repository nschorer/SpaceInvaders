using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class KeySpaceObserver : InputObserver
    {
        public override void Notify()
        {
            SceneContext.GetState().InputSpace();
        }
    }
}