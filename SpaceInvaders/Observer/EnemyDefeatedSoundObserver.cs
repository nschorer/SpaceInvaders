using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class EnemyDefeatedSoundObserver : ColObserver
    {
        public EnemyDefeatedSoundObserver(bool isUFO = false)
        {
            this.pEng = SpaceInvaders.GetSoundEngine();
            this.isUFO = isUFO;
        }

        public override void Notify()
        {
            if (isUFO)
            {
                UFOMan.PlayLowSound();
            }
            else
            {
                pEng.Play2D("invaderkilled.wav");
            }
        }

        private IrrKlang.ISoundEngine pEng;
        private bool isUFO;
    }
}
