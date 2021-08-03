using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class Key2Observer : InputObserver
    {

        public override void Notify()
        {
            SceneContext.GetState().Input2();
        }
    }
}
