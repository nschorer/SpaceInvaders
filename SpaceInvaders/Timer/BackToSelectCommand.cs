using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class BackToSelectCommand : Command
    {
        public BackToSelectCommand(Font pFont)
        {
            this.pFont = pFont;
        }
        public override void Execute(float deltaTime)
        {
            FontMan.RemoveFromSB(pFont, SpriteBatchMan.Find(SpriteBatch.Name.UI));
            ShipMan.ResetLives();
            ScenePlay pScene = (ScenePlay)SceneContext.GetState();
            pScene.ClearWave(false);
            pScene.Handle();
        }

        Font pFont;
    }
}
