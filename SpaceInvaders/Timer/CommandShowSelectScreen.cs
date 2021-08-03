using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class CommandShowSelectScreen : Command
    {
        public enum SelectScreen
        {
            ScoreTable,
            InsertCoin
        }

        public CommandShowSelectScreen(SceneSelect pSceneSelect, SelectScreen pScreen)
        {
            this.pSceneSelect = pSceneSelect;
            this.pScreen = pScreen;
        }
        

        
        public override void Execute(float deltaTime)
        {
            switch (pScreen)
            {
                case SelectScreen.ScoreTable:
                    pSceneSelect.HideInsertCoin();
                    pSceneSelect.ShowScoreTable();
                    break;
                case SelectScreen.InsertCoin:
                    pSceneSelect.HideScoreTable();
                    pSceneSelect.ShowInsertCoin();
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        SceneSelect pSceneSelect;
        SelectScreen pScreen;
    }
}
