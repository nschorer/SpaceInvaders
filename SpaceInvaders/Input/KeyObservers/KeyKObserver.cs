using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    class KeyKObserver : InputObserver
    {
        public override void Notify()
        {
            SceneContext.GetState().InputK();
        }
    }
}
