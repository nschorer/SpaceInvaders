using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class KeyLeftObserver : InputObserver
    {
        public override void Notify()
        {
            SceneContext.GetState().InputLeft();
        }
    }
}
