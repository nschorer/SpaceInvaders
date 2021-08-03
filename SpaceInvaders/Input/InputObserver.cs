using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    abstract class InputObserver : DLink
    {
        // define this in concrete
        abstract public void Notify();

        public InputSubject pSubject;
    }
}
