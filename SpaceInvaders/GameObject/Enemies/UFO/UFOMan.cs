using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class UFOMan
    {

        private enum StartPoint
        {
            Left,
            Right
        }

        private UFOMan()
        {
            pUFO = null;
            pRandom = new Random();
            this.pEng = SpaceInvaders.GetSoundEngine();

            // We SHOULD do something like this, but our walls our kind of janky so we're just gonna hack it in
            //WallLeft = GameObjectMan.Find(GameObject.Name.WallLeft).x + 50.0f;
            //WallRight = GameObjectMan.Find(GameObject.Name.WallRight).x - 50.0f;
            //WallTop = GameObjectMan.Find(GameObject.Name.WallTop).y - 25.0f;

            WallLeft = 50f;
            WallRight = 650f;
            WallTop = 675f;

            pHi = pEng.AddSoundSourceFromFile("ufo_highpitch.wav");
            pLow = pEng.AddSoundSourceFromFile("ufo_lowpitch.wav");
        }

        public static void Create()
        {
            // make sure its the first time
            //Debug.Assert(instance == null);

            // Do the initialization
            if (instance == null)
            {
                instance = new UFOMan();
            }

            Debug.Assert(instance != null);

            // Stuff to initialize after the instance was created
            //instance.pUFO = ActivateUFO();
        }

        private static UFOMan PrivInstance()
        {
            Debug.Assert(instance != null);

            return instance;
        }

        public static UFO ActivateUFO()
        {
            UFOMan pUFOMan = UFOMan.PrivInstance();
            Debug.Assert(pUFOMan != null);

            //Debug.Print("Activate UFO");

            float x = pUFOMan.WallLeft;
            float y = pUFOMan.WallTop;
            bool goRight = true;

            if (pUFOMan.pRandom.Next(0, 2) == (int)StartPoint.Right) // Random.Next(0,2) returns either 0 or 1
            {
                x = pUFOMan.WallRight;
                goRight = false;
            }

            // copy over safe copy
            //UFO pNewUFO = null;
            UFO pNewUFO = (UFO)GameObjectMan.ReuseGameObject(GameObject.Name.UFO);
            if (pNewUFO == null)
            {
                pNewUFO = new UFO(GameObject.Name.UFO, GameSprite.Name.UFO, x, y, goRight);
            }
            else
            {
                pNewUFO.Set(x, y, goRight);
            }
            pUFOMan.pUFO = pNewUFO;

            IrrKlang.ISoundEngine pEng = pUFOMan.pEng;
            pUFOMan.pHiRepeat = pEng.Play2D(pUFOMan.pHi, true, false, false);

            // Attached to SpriteBatches
            SpriteBatch pSB_Aliens = SpriteBatchMan.Find(SpriteBatch.Name.Enemies);
            SpriteBatch pSB_Boxes = SpriteBatchMan.GetCollisionSB();

            pNewUFO.ActivateCollisionSprite(pSB_Boxes);
            pNewUFO.ActivateGameSprite(pSB_Aliens);

            // Attach the UFO to the UFO root
            GameObject pUFORoot = GameObjectMan.Find(GameObject.Name.UFORoot);
            Debug.Assert(pUFORoot != null);

            // Add to GameObject Tree - {update and collisions}
            pUFORoot.Add(pNewUFO);

            return pNewUFO;
        }

        public static void PlayHighSound()
        {
            UFOMan pMan = UFOMan.PrivInstance();
            if ((pMan.pHiRepeat != null && pMan.pHiRepeat.Finished) || pMan.pHiRepeat == null)
            {
                pMan.pHiRepeat = pMan.pEng.Play2D(pMan.pHi, false, false, false);
            }
        }

        public static void StopHighSound()
        {
            UFOMan pMan = UFOMan.PrivInstance();
            if (pMan.pHiRepeat != null)
            {
                pMan.pHiRepeat.Stop();
            }
        }

        public static void PlayLowSound()
        {
            UFOMan pMan = UFOMan.PrivInstance();
            pMan.pEng.Play2D(pMan.pLow, false, false, false);
        }

        public static IrrKlang.ISoundEngine GetSoundEngine()
        {
            return UFOMan.PrivInstance().pEng;
        }

        public static void ClearOldUFO()
        {
            TimeEvent pEvent = TimerMan.Find(TimeEvent.Name.SpawnUFO);
            while (pEvent != null)
            {
                TimerMan.Remove(pEvent);
                pEvent = TimerMan.Find(TimeEvent.Name.SpawnUFO);
            }
        }

        public static void SpawnNewUFO()
        {
            UFOMan pUFOMan = UFOMan.PrivInstance();
            Debug.Assert(pUFOMan != null);
            float delay = pUFOMan.pRandom.Next(20, 21);
            TimerMan.Add(TimeEvent.Name.SpawnUFO, new SpawnUFOEvent(), delay);
        }

        // Data: ----------------------------------------------
        private static UFOMan instance = null;

        // Active
        private UFO pUFO;      // this can return an outdated reference... we need to rethink this if we actually want to use it this way
        private IrrKlang.ISoundEngine pEng;
        private IrrKlang.ISoundSource pHi;
        private IrrKlang.ISoundSource pLow;
        private IrrKlang.ISound pHiRepeat;
        private Random pRandom;
        private float WallLeft;
        private float WallRight;
        private float WallTop;
    }
}
