using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    abstract public class ColObserver : DLink
    {
        public abstract void Notify();

        // WHY not add a state pattern into our Observer!
        public virtual void Execute()
        {
            // default implementation
        }

        public ColSubject pSubject;
    }
}
