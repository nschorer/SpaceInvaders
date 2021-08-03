using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class SceneContext
    {
        public enum Scene
        {
            Select,
            Player1,
            Player2,
            Over,

            Unitialized
        }
        private SceneContext()
        {
            // reserve the states
            this.poSceneSelect = new SceneSelect();
            this.poScenePlayer1 = new ScenePlay();
            this.poScenePlayer2 = new ScenePlay();
            //this.poScenePlayer1 = new ScenePlay();
            this.poSceneOver = new SceneOver();

            // initialize to the select state
            this.pSceneState = this.poSceneSelect;
            this.pSceneState.Transition();
        }

        public static void Create()
        {
            Debug.Assert(pInstance == null);
            pInstance = new SceneContext();
        }

        public static SceneState GetState()
        {
            Debug.Assert(pInstance != null);
            return pInstance.pSceneState;
        }
        public static void SetState( Scene eScene)
        {
            Debug.Assert(pInstance != null);

            switch(eScene)
            {
                case Scene.Select:
                    pInstance.pSceneState = pInstance.poSceneSelect;
                    pInstance.pSceneState.Transition();
                    break;

                case Scene.Player1:
                    pInstance.pSceneState = pInstance.poScenePlayer1;
                    pInstance.pSceneState.Transition();
                    break;

                case Scene.Player2:
                    pInstance.pSceneState = pInstance.poScenePlayer2;
                    pInstance.pSceneState.Transition();
                    break;

                case Scene.Over:
                    pInstance.pSceneState = pInstance.poSceneOver;
                    pInstance.pSceneState.Transition();
                    break;

            }
        }

        public static SceneContext.Scene ReturnActivePlayer()
        {
            Scene pScene = Scene.Unitialized;

            if (pInstance.pSceneState == pInstance.poScenePlayer1)
            {
                pScene = Scene.Player1;
            }
            else if (pInstance.pSceneState == pInstance.poScenePlayer2)
            {
                pScene = Scene.Player2;
            }
            else
            {
                Debug.Assert(false);
            }

            return pScene;
        }

        public static void SetNumPlayers(bool isTwoPlayer)
        {
            pInstance.isTwoPlayer = isTwoPlayer;
        }

        public static bool IsTwoPlayer()
        {
            return pInstance.isTwoPlayer;
        }

        // ----------------------------------------------------
        // Data: 
        // -------------------------------------------o---------
        public static SceneContext pInstance = null;

        SceneState pSceneState;
        SceneSelect poSceneSelect;
        SceneOver poSceneOver;
        ScenePlay poScenePlayer1;
        ScenePlay poScenePlayer2;
        bool isTwoPlayer = false;
    }
}
