using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class InputMan
    {
        private InputMan()
        {
            this.pSubjectArrowLeft = new InputSubject();
            this.pSubjectArrowRight = new InputSubject();
            this.pSubjectSpace = new InputSubject();
            this.pSubjectK = new InputSubject();
            this.pSubject1 = new InputSubject();
            this.pSubject2 = new InputSubject();

            this.privSpaceKeyPrev = false;
            this.privKKeyPrev = false;
        }

        private static InputMan privGetInstance()
        {
            if (pInstance == null)
            {
                pInstance = new InputMan();
            }
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        public static InputSubject GetArrowRightSubject()
        {
            InputMan pMan = InputMan.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectArrowRight;
        }

        public static InputSubject GetArrowLeftSubject()
        {
            InputMan pMan = InputMan.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectArrowLeft;
        }

        public static InputSubject GetSpaceSubject()
        {
            InputMan pMan = InputMan.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectSpace;
        }

        public static InputSubject Get1Subject()
        {
            InputMan pMan = InputMan.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubject1;
        }

        public static InputSubject Get2Subject()
        {
            InputMan pMan = InputMan.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubject2;
        }

        public static InputSubject GetKSubject()
        {
            InputMan pMan = InputMan.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectK;
        }

        public static void Update()
        {
            InputMan pMan = InputMan.privGetInstance();
            Debug.Assert(pMan != null);

            // SpaceKey: (with key history) -----------------------------------------------------------
            bool spaceKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_SPACE);

            if (spaceKeyCurr == true && pMan.privSpaceKeyPrev == false)
            {
                pMan.pSubjectSpace.Notify();
            }

            // KKey: (with key history) -----------------------------------------------------------
            bool kKeyCur = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_K);

            if (kKeyCur == true && pMan.privKKeyPrev == false)
            {
                pMan.pSubjectK.Notify();
            }

            // 1Key: (with key history) -----------------------------------------------------------
            bool oneKeyCur = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_1);

            if (oneKeyCur == true && pMan.priv1KeyPrev == false)
            {
                pMan.pSubject1.Notify();
            }

            // 2Key: (with key history) -----------------------------------------------------------
            bool twoKeyCur = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_2);

            if (twoKeyCur == true && pMan.priv2KeyPrev == false)
            {
                pMan.pSubject2.Notify();
            }

            // LeftKey: (no history) -----------------------------------------------------------
            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_LEFT) == true)
            {
                pMan.pSubjectArrowLeft.Notify();
            }

            // RightKey: (no history) -----------------------------------------------------------
            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_RIGHT) == true)
            {
                pMan.pSubjectArrowRight.Notify();
            }

            pMan.privSpaceKeyPrev = spaceKeyCurr;
            pMan.privKKeyPrev = kKeyCur;
            pMan.priv1KeyPrev = oneKeyCur;
            pMan.priv2KeyPrev = twoKeyCur;

        }

        // Data: ----------------------------------------------
        private static InputMan pInstance = null;
        private bool privSpaceKeyPrev;
        private bool privKKeyPrev;
        private bool priv1KeyPrev;
        private bool priv2KeyPrev;

        private InputSubject pSubjectArrowRight;
        private InputSubject pSubjectArrowLeft;
        private InputSubject pSubjectSpace;
        private InputSubject pSubjectK;
        private InputSubject pSubject1;
        private InputSubject pSubject2;
    }
}
