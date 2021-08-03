
using System.Diagnostics;

namespace SpaceInvaders
{
    public class SceneSelect : SceneState
    {
        // ---------------------------------------------------
        // Data
        // ---------------------------------------------------
        public SpriteBatchMan poSpriteBatchMan;
        public TimerMan poTimerMan;
        public ProxySpriteMan poProxySpriteMan;
        public BoxSpriteMan poBoxSpriteMan;
        public GameObjectMan poGameObjectMan;
        public FontMan poFontMan;

        // bad bad bad
        private ProxySprite pSaucer;
        private ProxySprite pSquid;
        private ProxySprite pCrab;
        private ProxySprite pOcto;

        public SceneSelect()
        {
            this.Initialize();
        }
        public override void Handle()
        {
            Debug.WriteLine("Handle");
        }
        public override void Initialize()
        {
            // These are unique to each scene
            // Individual instances of the manager will be created in each scene
            this.poSpriteBatchMan = new SpriteBatchMan(5, 1);
            this.poTimerMan = new TimerMan(5, 1);
            this.poProxySpriteMan = new ProxySpriteMan(60, 5);
            this.poBoxSpriteMan = new BoxSpriteMan(70, 5);
            this.poGameObjectMan = new GameObjectMan(60, 5);
            this.poFontMan = new FontMan(5, 1);

            SpriteBatchMan.SetActive(this.poSpriteBatchMan);
            TimerMan.SetActive(this.poTimerMan);
            ProxySpriteMan.SetActive(this.poProxySpriteMan);
            BoxSpriteMan.SetActive(this.poBoxSpriteMan);
            GameObjectMan.SetActive(this.poGameObjectMan);
            FontMan.SetActive(this.poFontMan);

            CreateSpriteBatches();

            SetupUI();

            
        }

        public override void Transition()
        {
            SpriteBatchMan.SetActive(this.poSpriteBatchMan);
            TimerMan.SetActive(this.poTimerMan);
            ProxySpriteMan.SetActive(this.poProxySpriteMan);
            BoxSpriteMan.SetActive(this.poBoxSpriteMan);
            GameObjectMan.SetActive(this.poGameObjectMan);
            FontMan.SetActive(this.poFontMan);

            TimerMan.ClearTimerEvents();

            this.HideInsertCoin();
            this.HideScoreTable();

            this.ShowScoreTable();

            ScoreBoard.RefreshScoreDisplay();
        }

        public override void Update(float systemTime)
        {
            // Single Step, Free running...
            Simulation.Update(systemTime);

            // Input
            InputMan.Update();

            // Run based on simulation stepping
            if (Simulation.GetTimeStep() > 0.0f)
            {
                // Fire off the timer events
                TimerMan.Update(Simulation.GetTotalTime());

                // Do the collision checks
                ColPairMan.Process();

                // walk through all objects and push to flyweight
                GameObjectMan.Update();

                // Delete any objects here...
                DelayedObjectMan.Process();
            }
        }

        public override void Draw()
        {
            // draw all objects
            SpriteBatchMan.Draw();
        }

        public void CreateSpriteBatches()
        {
            //SpriteBatch pSB_Enemies = SpriteBatchMan.Add(SpriteBatch.Name.Enemies, 1);
            //SpriteBatch pSB_Missile = SpriteBatchMan.Add(SpriteBatch.Name.Spaceship, 2);
            SpriteBatch pSB_UI = SpriteBatchMan.Add(SpriteBatch.Name.UI, 15);
            //SpriteBatch pSB_Explosions = SpriteBatchMan.Add(SpriteBatch.Name.Explosions, 11);
            //SpriteBatch pSB_Shields = SpriteBatchMan.Add(SpriteBatch.Name.Shields, 4);

            //SpriteBatch pSB_Box = SpriteBatchMan.Add(SpriteBatch.Name.Boxes, 100);
            //SpriteBatchMan.SetCollisionSB(SpriteBatch.Name.Boxes);
        }

        public void SetupUI()
        {
            // Scores and Credits -- always showing
            FontMan.Add(Font.Name.Score1Label, SpriteBatch.Name.UI, "SCORE<1>", Glyph.Name.Consolas36pt, 50, 775);
            FontMan.Add(Font.Name.Score2Label, SpriteBatch.Name.UI, "SCORE<2>", Glyph.Name.Consolas36pt, 500, 775);
            FontMan.Add(Font.Name.HighScoreLabel, SpriteBatch.Name.UI, "HI-SCORE", Glyph.Name.Consolas36pt, 250, 775);
            Font player1 = FontMan.Add(Font.Name.Score1, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 75, 725);
            Font player2 = FontMan.Add(Font.Name.Score2, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 525, 725);
            FontMan.Add(Font.Name.HighScore, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 275, 725);
            FontMan.Add(Font.Name.CreditsLabel, SpriteBatch.Name.UI, "CREDIT", Glyph.Name.Consolas36pt, 520, 20);
            FontMan.Add(Font.Name.Credits, SpriteBatch.Name.UI, "00", Glyph.Name.Consolas36pt, 650, 20);

            // Score table -- sometimes showing
            FontMan.Add(Font.Name.Play, SpriteBatch.Name.UI, "PLAY", Glyph.Name.Consolas36pt, 300, 600, false);
            FontMan.Add(Font.Name.SpaceInvaders, SpriteBatch.Name.UI, "SPACE INVADERS", Glyph.Name.Consolas36pt, 200, 525, false);
            FontMan.Add(Font.Name.ScoreAdvanceTable, SpriteBatch.Name.UI, "*SCORE ADVANCE TABLE*", Glyph.Name.Consolas36pt, 150, 425, false);
            FontMan.Add(Font.Name.Mystery, SpriteBatch.Name.UI, "=? MYSTERY", Glyph.Name.Consolas36pt, 275, 375, false);
            FontMan.Add(Font.Name.Thirty, SpriteBatch.Name.UI, "=30 POINTS", Glyph.Name.Consolas36pt, 275, 325, false);
            FontMan.Add(Font.Name.Twenty, SpriteBatch.Name.UI, "=20 POINTS", Glyph.Name.Consolas36pt, 275, 275, false);
            FontMan.Add(Font.Name.Ten, SpriteBatch.Name.UI, "=10 POINTS", Glyph.Name.Consolas36pt, 275, 225, false);

            pSaucer = ProxySpriteMan.Add(GameSprite.Name.Saucer);
            pSaucer.Set(225, 375);
            pSquid = ProxySpriteMan.Add(GameSprite.Name.SquidA);
            pSquid.Set(225, 325);
            pCrab = ProxySpriteMan.Add(GameSprite.Name.CrabB);
            pCrab.Set(225, 275);
            pOcto = ProxySpriteMan.Add(GameSprite.Name.OctopusA);
            pOcto.Set(225, 225);

            // Insert coin -- sometimes showing
            FontMan.Add(Font.Name.InsertCoin, SpriteBatch.Name.UI, "INSERT COIN", Glyph.Name.Consolas36pt, 225, 490, false);
            FontMan.Add(Font.Name.OneOrTwoPlayers, SpriteBatch.Name.UI, "<1 OR 2 PLAYERS>", Glyph.Name.Consolas36pt, 175, 400, false);
            FontMan.Add(Font.Name.OnePOneC, SpriteBatch.Name.UI, "*1 PLAYER  1 COIN", Glyph.Name.Consolas36pt, 175, 325, false);
            FontMan.Add(Font.Name.TwoPTwoC, SpriteBatch.Name.UI, "*2 PLAYERS 2 COINS", Glyph.Name.Consolas36pt, 175, 250, false);
        }

        public void ShowScores()
        {
            FontMan.Add(Font.Name.Score1Label, SpriteBatch.Name.UI, "SCORE<1>", Glyph.Name.Consolas36pt, 50, 775);
            FontMan.Add(Font.Name.Score2Label, SpriteBatch.Name.UI, "SCORE<2>", Glyph.Name.Consolas36pt, 500, 775);
            FontMan.Add(Font.Name.HighScoreLabel, SpriteBatch.Name.UI, "HI-SCORE", Glyph.Name.Consolas36pt, 250, 775);
            Font player1 = FontMan.Add(Font.Name.Score1, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 50, 725);
            Font player2 = FontMan.Add(Font.Name.Score2, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 500, 725);
            FontMan.Add(Font.Name.HighScore, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 250, 725);
            FontMan.Add(Font.Name.CreditsLabel, SpriteBatch.Name.UI, "CREDIT", Glyph.Name.Consolas36pt, 520, 20);
            FontMan.Add(Font.Name.Credits, SpriteBatch.Name.UI, "00", Glyph.Name.Consolas36pt, 650, 20);
        }

        public void ShowScoreTable()
        {
            SpriteBatch pSB = SpriteBatchMan.Find(SpriteBatch.Name.UI);

            Font pFont = FontMan.Find(Font.Name.Play);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.SpaceInvaders);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.ScoreAdvanceTable);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.Mystery);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.Thirty);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.Twenty);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.Ten);
            FontMan.AddToSB(pFont, pSB);

            // Should have a better way of doing this
            ProxySpriteMan.Show(pSaucer, pSB);
            ProxySpriteMan.Show(pSquid, pSB);
            ProxySpriteMan.Show(pCrab, pSB);
            ProxySpriteMan.Show(pOcto, pSB);

            TimerMan.Add(TimeEvent.Name.ShowInsertCoin, new CommandShowSelectScreen(this, CommandShowSelectScreen.SelectScreen.InsertCoin), 5.0f);
        }

        public void HideScoreTable()
        {
            SpriteBatch pSB = SpriteBatchMan.Find(SpriteBatch.Name.UI);

            Font pFont = FontMan.Find(Font.Name.Thirty);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.Play);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.SpaceInvaders);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.ScoreAdvanceTable);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.Mystery);
            FontMan.RemoveFromSB(pFont, pSB);

            

            pFont = FontMan.Find(Font.Name.Twenty);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.Ten);
            FontMan.RemoveFromSB(pFont, pSB);

            // Should have a better way of doing this
            ProxySpriteMan.Hide(pSaucer);
            ProxySpriteMan.Hide(pSquid);
            ProxySpriteMan.Hide(pCrab);
            ProxySpriteMan.Hide(pOcto);
        }

        public void ShowInsertCoin()
        {
            SpriteBatch pSB = SpriteBatchMan.Find(SpriteBatch.Name.UI);

            Font pFont = FontMan.Find(Font.Name.InsertCoin);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.OneOrTwoPlayers);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.OnePOneC);
            FontMan.AddToSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.TwoPTwoC);
            FontMan.AddToSB(pFont, pSB);

            TimerMan.Add(TimeEvent.Name.ShowInsertCoin, new CommandShowSelectScreen(this, CommandShowSelectScreen.SelectScreen.ScoreTable), 5.0f);
        }

        public void HideInsertCoin()
        {
            SpriteBatch pSB = SpriteBatchMan.Find(SpriteBatch.Name.UI);

            Font pFont = FontMan.Find(Font.Name.InsertCoin);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.OneOrTwoPlayers);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.OnePOneC);
            FontMan.RemoveFromSB(pFont, pSB);

            pFont = FontMan.Find(Font.Name.TwoPTwoC);
            FontMan.RemoveFromSB(pFont, pSB);
        }

        public override void Input1()
        {
            ScenePlay.resetWave = true;
            SceneContext.SetNumPlayers(false);
            SceneContext.SetState(SceneContext.Scene.Player1);
        }

        public override void Input2()
        {
            ScenePlay.resetWave = true;
            SceneContext.SetNumPlayers(true);
            SceneContext.SetState(SceneContext.Scene.Player1);
        }
    }
}
