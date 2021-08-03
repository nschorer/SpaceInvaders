using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class Key1Observer : InputObserver
    {
        public override void Notify()
        {
            SceneContext.GetState().Input1();
        }
    }
}
