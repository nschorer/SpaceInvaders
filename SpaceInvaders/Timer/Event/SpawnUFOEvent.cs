using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class SpawnUFOEvent : Command
    {
        public SpawnUFOEvent()
        {

        }

        override public void Execute(float deltaTime)
        {
            UFOMan.ActivateUFO();
        }

    }
}

// End of File
