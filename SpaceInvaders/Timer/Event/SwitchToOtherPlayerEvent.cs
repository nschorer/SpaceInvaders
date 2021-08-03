using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class SwitchToOtherPlayerEvent : Command
    {
        public override void Execute(float deltaTime)
        {
            ScenePlay.SwitchToOtherPlayer();
        }
    }
}
