using System.Diagnostics;

namespace SpaceInvaders
{
    public class SceneOver : SceneState
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

        public SceneOver()
        {
            this.Initialize();
        }
        public override void Handle()
        {

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

            SpriteBatch pSB_UI = SpriteBatchMan.Add(SpriteBatch.Name.UI, 1);

            Font pFont = FontMan.Add(Font.Name.TestMessage, SpriteBatch.Name.UI, "Game OVER", Glyph.Name.Consolas36pt, 100, 500);
            pFont.SetColor(0.10f, 0.10f, 0.50f);
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
        public override void Transition()
        {
            SpriteBatchMan.SetActive(this.poSpriteBatchMan);
            TimerMan.SetActive(this.poTimerMan);
            ProxySpriteMan.SetActive(this.poProxySpriteMan);
            BoxSpriteMan.SetActive(this.poBoxSpriteMan);
            GameObjectMan.SetActive(this.poGameObjectMan);
            FontMan.SetActive(this.poFontMan);
        }

        
    }
}
