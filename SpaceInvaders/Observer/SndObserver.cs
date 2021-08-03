using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class SndObserver : ColObserver
    {
        public SndObserver(IrrKlang.ISoundEngine pEng)
        { 
            Debug.Assert(pEng != null);
            this.pSndEngine = pEng;
        }
        public override void Notify()
        {
            Debug.WriteLine(" Snd_Observer: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            pSndEngine.SoundVolume = 0.2f;
            IrrKlang.ISound pSnd = pSndEngine.Play2D("fastinvader1.wav");
        }

        // Data
        IrrKlang.ISoundEngine pSndEngine;
    }
}
