using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class PlayerDefeatedSoundObserver : ColObserver
    {
        public PlayerDefeatedSoundObserver()
        {
            this.pEng = SpaceInvaders.GetSoundEngine();
        }

        public override void Notify()
        {
            pEng.Play2D("PlayerExplosion.wav");
        }

        private IrrKlang.ISoundEngine pEng;
    }
}
