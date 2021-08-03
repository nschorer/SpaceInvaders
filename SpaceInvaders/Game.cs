using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class SpaceInvaders : Azul.Game
    {

        static IrrKlang.ISoundEngine sndEngine = null;

        public static IrrKlang.ISoundEngine GetSoundEngine()
        {
            return sndEngine;
        }

        //-----------------------------------------------------------------------------
        // Game::Initialize()
        //		Allows the engine to perform any initialization it needs to before 
        //      starting to run.  This is where it can query for any required services 
        //      and load any non-graphic related content. 
        //-----------------------------------------------------------------------------
        public override void Initialize()
        {
            // Game Window Device setup
            this.SetWindowName("Space Invaders");
            this.SetWidthHeight(700, 800);
            this.SetClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        //-----------------------------------------------------------------------------
        // Game::LoadContent()
        //		Allows you to load all content needed for your engine,
        //	    such as objects, graphics, etc.
        //-----------------------------------------------------------------------------
        public override void LoadContent()
        {
            // Should make this into a proper class later
            sndEngine = new IrrKlang.ISoundEngine();
            sndEngine.SoundVolume = 0.3f;

            // Testing
            Simulation.Create();


            // Create managers

            // These are created once
            
            TextureMan.Create(3, 1);
            GlyphMan.Create(3, 1);
            ImageMan.Create(5, 3);
            GameSpriteMan.Create(20, 5);
            ColPairMan.Create(15, 1);

            SetupInputs();

            // These are unique to each scene
            // Individual instances of the manager will be created in each scene
            TimerMan.Create();
            SpriteBatchMan.Create();
            ProxySpriteMan.Create();
            BoxSpriteMan.Create();
            GameObjectMan.Create();
            FontMan.Create();
   
            // Load assets -- useful for all scenes
            LoadTextures();
            LoadGlyphs();
            LoadImages();
            LoadSprites();

            SceneContext.Create();
        }

        //-----------------------------------------------------------------------------
        // Game::Update()
        //      Called once per frame, update data, tranformations, etc
        //      Use this function to control process order
        //      Input, AI, Physics, Animation, and Graphics
        //-----------------------------------------------------------------------------
        public override void Update()
        {
            // Hack to proof of concept... 
            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_6) == true)
            {
                SceneContext.SetState(SceneContext.Scene.Select);
            }

            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_7) == true)
            {
                ScenePlay.resetWave = true;
                SceneContext.SetState(SceneContext.Scene.Player1);
            }

            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_8) == true)
            {
                ScenePlay.resetWave = true;
                SceneContext.SetState(SceneContext.Scene.Player2);
            }

            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_9) == true)
            {
                SceneContext.SetState(SceneContext.Scene.Over);
            }

            sndEngine.Update();

            SceneContext.GetState().Update(this.GetTime());
            //TimerMan.Update(this.GetTime());
            //InputMan.Update();
            //GameObjectMan.Update();
            ColPairMan.Process();
            
            DelayedObjectMan.Process();

            
        }

        //-----------------------------------------------------------------------------
        // Game::Draw()
        //		This function is called once per frame
        //	    Use this for draw graphics to the screen.
        //      Only do rendering here
        //-----------------------------------------------------------------------------
        public override void Draw()
        {
            //SpriteBatchMan.Draw();
            SceneContext.GetState().Draw();
        }

        //-----------------------------------------------------------------------------
        // Game::UnLoadContent()
        //       unload content (resources loaded above)
        //       unload all content that was loaded before the Engine Loop started
        //-----------------------------------------------------------------------------
        public override void UnLoadContent()
        {

        }

        //-----------------------------------------------------------------------------
        // PRIVATE METHODS
        //-----------------------------------------------------------------------------

        private void LoadTextures()
        {
            TextureMan.Add(Texture.Name.SpaceInvaders, "SpaceInvaders.tga");
            TextureMan.Add(Texture.Name.Aliens, "Birds_N_Shield.tga");
            TextureMan.Add(Texture.Name.Consolas20pt, "Consolas20pt.tga");
            TextureMan.Add(Texture.Name.Consolas36pt, "Consolas36pt.tga");
        }

        private void LoadGlyphs()
        {
            GlyphMan.AddXml(Glyph.Name.Consolas20pt, "Consolas20pt.xml", Texture.Name.Consolas20pt);   
            GlyphMan.AddXml(Glyph.Name.Consolas36pt, "Consolas36pt.xml", Texture.Name.Consolas36pt);
        }

        private void LoadImages()
        {
            ImageMan.Add(Image.Name.OctopusA, Texture.Name.SpaceInvaders, 3, 3, 12, 8);
            ImageMan.Add(Image.Name.OctopusB, Texture.Name.SpaceInvaders, 18, 3, 12, 8);
            ImageMan.Add(Image.Name.CrabA, Texture.Name.SpaceInvaders, 33, 3, 11, 8);
            ImageMan.Add(Image.Name.CrabB, Texture.Name.SpaceInvaders, 47, 3, 11, 8);
            ImageMan.Add(Image.Name.SquidA, Texture.Name.SpaceInvaders, 61, 3, 8, 8);
            ImageMan.Add(Image.Name.SquidB, Texture.Name.SpaceInvaders, 72, 3, 8, 8);
            ImageMan.Add(Image.Name.AlienExplosion, Texture.Name.SpaceInvaders, 83, 3, 13, 8);
            ImageMan.Add(Image.Name.Saucer, Texture.Name.SpaceInvaders, 99, 3, 16, 8);
            ImageMan.Add(Image.Name.SaucerExplosion, Texture.Name.SpaceInvaders, 118, 3, 21, 8);

            ImageMan.Add(Image.Name.Player, Texture.Name.SpaceInvaders, 3, 14, 13, 8);
            ImageMan.Add(Image.Name.Ship, Texture.Name.SpaceInvaders, 3, 14, 13, 8);        // Duplicate of player... should probably clean this up
            ImageMan.Add(Image.Name.PlayerExplosionA, Texture.Name.SpaceInvaders, 19, 14, 16, 8);
            ImageMan.Add(Image.Name.PlayerExplosionB, Texture.Name.SpaceInvaders, 38, 14, 16, 8);
            ImageMan.Add(Image.Name.AlienPullYA, Texture.Name.SpaceInvaders, 57, 14, 15, 8);
            ImageMan.Add(Image.Name.AlienPullYB, Texture.Name.SpaceInvaders, 75, 14, 15, 8);
            ImageMan.Add(Image.Name.AlienPullUpisdeDownYA, Texture.Name.SpaceInvaders, 93, 14, 14, 8);
            ImageMan.Add(Image.Name.AlienPullUpsideDownYB, Texture.Name.SpaceInvaders, 110, 14, 14, 8);

            ImageMan.Add(Image.Name.PlayerShot, Texture.Name.SpaceInvaders, 3, 29, 1, 4);
            ImageMan.Add(Image.Name.PlayerShotExplosion, Texture.Name.SpaceInvaders, 7, 25, 8, 8);
            ImageMan.Add(Image.Name.SquigglyShotA, Texture.Name.SpaceInvaders, 18, 26, 3, 7);
            ImageMan.Add(Image.Name.SquigglyShotB, Texture.Name.SpaceInvaders, 24, 26, 3, 7);
            ImageMan.Add(Image.Name.SquigglyShotC, Texture.Name.SpaceInvaders, 30, 26, 3, 7);
            ImageMan.Add(Image.Name.SquigglyShotD, Texture.Name.SpaceInvaders, 36, 26, 3, 7);
            ImageMan.Add(Image.Name.PlungerShotA, Texture.Name.SpaceInvaders, 42, 27, 3, 6);
            ImageMan.Add(Image.Name.PlungerShotB, Texture.Name.SpaceInvaders, 48, 27, 3, 6);
            ImageMan.Add(Image.Name.PlungerShotC, Texture.Name.SpaceInvaders, 54, 27, 3, 6);
            ImageMan.Add(Image.Name.PlungerShotD, Texture.Name.SpaceInvaders, 60, 27, 3, 6);
            ImageMan.Add(Image.Name.RollingShotA, Texture.Name.SpaceInvaders, 65, 26, 3, 7);
            ImageMan.Add(Image.Name.RollingShotB, Texture.Name.SpaceInvaders, 70, 26, 3, 7);
            ImageMan.Add(Image.Name.RollingShotC, Texture.Name.SpaceInvaders, 75, 26, 3, 7);
            ImageMan.Add(Image.Name.RollingShotD, Texture.Name.SpaceInvaders, 80, 26, 3, 7);
            ImageMan.Add(Image.Name.AlienShotExplosion, Texture.Name.SpaceInvaders, 86, 25, 6, 8);

            ImageMan.Add(Image.Name.A, Texture.Name.SpaceInvaders, 3, 36, 5, 7);
            ImageMan.Add(Image.Name.B, Texture.Name.SpaceInvaders, 11, 36, 5, 7);
            ImageMan.Add(Image.Name.C, Texture.Name.SpaceInvaders, 19, 36, 5, 7);
            ImageMan.Add(Image.Name.D, Texture.Name.SpaceInvaders, 27, 36, 5, 7);
            ImageMan.Add(Image.Name.E, Texture.Name.SpaceInvaders, 35, 36, 5, 7);
            ImageMan.Add(Image.Name.F, Texture.Name.SpaceInvaders, 43, 36, 5, 7);
            ImageMan.Add(Image.Name.G, Texture.Name.SpaceInvaders, 51, 36, 5, 7);
            ImageMan.Add(Image.Name.H, Texture.Name.SpaceInvaders, 59, 36, 5, 7);
            ImageMan.Add(Image.Name.I, Texture.Name.SpaceInvaders, 67, 36, 5, 7);
            ImageMan.Add(Image.Name.J, Texture.Name.SpaceInvaders, 75, 36, 5, 7);
            ImageMan.Add(Image.Name.K, Texture.Name.SpaceInvaders, 83, 36, 5, 7);
            ImageMan.Add(Image.Name.L, Texture.Name.SpaceInvaders, 91, 36, 5, 7);
            ImageMan.Add(Image.Name.M, Texture.Name.SpaceInvaders, 99, 36, 5, 7);

            ImageMan.Add(Image.Name.N, Texture.Name.SpaceInvaders, 3, 46, 5, 7);
            ImageMan.Add(Image.Name.O, Texture.Name.SpaceInvaders, 11, 46, 5, 7);
            ImageMan.Add(Image.Name.P, Texture.Name.SpaceInvaders, 19, 46, 5, 7);
            ImageMan.Add(Image.Name.Q, Texture.Name.SpaceInvaders, 27, 46, 5, 7);
            ImageMan.Add(Image.Name.R, Texture.Name.SpaceInvaders, 35, 46, 5, 7);
            ImageMan.Add(Image.Name.S, Texture.Name.SpaceInvaders, 43, 46, 5, 7);
            ImageMan.Add(Image.Name.T, Texture.Name.SpaceInvaders, 51, 46, 5, 7);
            ImageMan.Add(Image.Name.U, Texture.Name.SpaceInvaders, 59, 46, 5, 7);
            ImageMan.Add(Image.Name.V, Texture.Name.SpaceInvaders, 67, 46, 5, 7);
            ImageMan.Add(Image.Name.W, Texture.Name.SpaceInvaders, 75, 46, 5, 7);
            ImageMan.Add(Image.Name.X, Texture.Name.SpaceInvaders, 83, 46, 5, 7);
            ImageMan.Add(Image.Name.Y, Texture.Name.SpaceInvaders, 91, 46, 5, 7);
            ImageMan.Add(Image.Name.Z, Texture.Name.SpaceInvaders, 99, 46, 5, 7);

            ImageMan.Add(Image.Name.Zero, Texture.Name.SpaceInvaders, 3, 56, 5, 7);
            ImageMan.Add(Image.Name.One, Texture.Name.SpaceInvaders, 11, 56, 5, 7);
            ImageMan.Add(Image.Name.Two, Texture.Name.SpaceInvaders, 19, 56, 5, 7);
            ImageMan.Add(Image.Name.Three, Texture.Name.SpaceInvaders, 27, 56, 5, 7);
            ImageMan.Add(Image.Name.Four, Texture.Name.SpaceInvaders, 35, 56, 5, 7);
            ImageMan.Add(Image.Name.Five, Texture.Name.SpaceInvaders, 43, 56, 5, 7);
            ImageMan.Add(Image.Name.Six, Texture.Name.SpaceInvaders, 51, 56, 5, 7);
            ImageMan.Add(Image.Name.Seven, Texture.Name.SpaceInvaders, 59, 56, 5, 7);
            ImageMan.Add(Image.Name.Eight, Texture.Name.SpaceInvaders, 67, 56, 5, 7);
            ImageMan.Add(Image.Name.Nine, Texture.Name.SpaceInvaders, 75, 56, 5, 7);
            ImageMan.Add(Image.Name.LessThan, Texture.Name.SpaceInvaders, 83, 56, 5, 7);
            ImageMan.Add(Image.Name.GreaterThan, Texture.Name.SpaceInvaders, 91, 56, 5, 7);
            ImageMan.Add(Image.Name.Space, Texture.Name.SpaceInvaders, 99, 56, 5, 7);
            ImageMan.Add(Image.Name.Equals, Texture.Name.SpaceInvaders, 107, 56, 5, 7);
            ImageMan.Add(Image.Name.Asterisk, Texture.Name.SpaceInvaders, 115, 56, 5, 7);
            ImageMan.Add(Image.Name.Question, Texture.Name.SpaceInvaders, 123, 56, 5, 7);
            ImageMan.Add(Image.Name.Hyphen, Texture.Name.SpaceInvaders, 131, 56, 5, 7);

            ImageMan.Add(Image.Name.Brick, Texture.Name.Aliens, 20, 210, 10, 5);
            ImageMan.Add(Image.Name.BrickLeft_Top0, Texture.Name.Aliens, 15, 180, 10, 5);
            ImageMan.Add(Image.Name.BrickLeft_Top1, Texture.Name.Aliens, 15, 185, 10, 5);
            ImageMan.Add(Image.Name.BrickLeft_Bottom, Texture.Name.Aliens, 35, 215, 10, 5);
            ImageMan.Add(Image.Name.BrickRight_Top0, Texture.Name.Aliens, 75, 180, 10, 5);
            ImageMan.Add(Image.Name.BrickRight_Top1, Texture.Name.Aliens, 75, 185, 10, 5);
            ImageMan.Add(Image.Name.BrickRight_Bottom, Texture.Name.Aliens, 55, 215, 10, 5);
        }

        private void LoadSprites()
        {
            GameSpriteMan.Add(GameSprite.Name.OctopusA, Image.Name.OctopusA, 300.0f, 400.0f, 38.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.OctopusB, Image.Name.OctopusB, 300.0f, 400.0f, 38.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.SquidA, Image.Name.SquidA, 300.0f, 400.0f, 27.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.SquidB, Image.Name.SquidB, 300.0f, 400.0f, 27.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.CrabA, Image.Name.CrabA, 300.0f, 400.0f, 33.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.CrabB, Image.Name.CrabB, 300.0f, 400.0f, 33.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.UFO, Image.Name.Saucer, 300.0f, 400.0f, 50.0f, 27.0f, new Azul.Color(1.0f, 0.0f, 0.0f));
            GameSpriteMan.Add(GameSprite.Name.Saucer, Image.Name.Saucer, 300.0f, 400.0f, 50.0f, 27.0f); // white ufo
            GameSpriteMan.Add(GameSprite.Name.SaucerExplosion, Image.Name.SaucerExplosion, 300.0f, 400.0f, 50.0f, 27.0f, new Azul.Color(1.0f, 0.0f, 0.0f));

            //GameSpriteMan.Add(GameSprite.Name.Player, Image.Name.Player, 300.0f, 400.0f, 27.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.Ship, Image.Name.Ship, 300.0f, 400.0f, 38.0f, 27.0f, new Azul.Color(0,1,0));
            GameSpriteMan.Add(GameSprite.Name.PlayerExplosionA, Image.Name.PlayerExplosionA, 300.0f, 400.0f, 40.0f, 27.0f);
            GameSpriteMan.Add(GameSprite.Name.PlayerExplosionB, Image.Name.PlayerExplosionB, 300.0f, 400.0f, 40.0f, 27.0f);
            //GameSpriteMan.Add(GameSprite.Name.PlayerShot, Image.Name.PlayerShot, 300.0f, 400.0f, 3.0f, 12.0f);
            GameSpriteMan.Add(GameSprite.Name.Missile, Image.Name.PlayerShot, 0.0f, 0.0f, 3.0f, 12.0f);
            GameSpriteMan.Add(GameSprite.Name.PlayerShotExplosion, Image.Name.PlayerShotExplosion, 300.0f, 400.0f, 18.0f, 18.0f);
            GameSpriteMan.Add(GameSprite.Name.AlienExplosion, Image.Name.AlienExplosion, 300.0f, 400.0f, 24.0f, 24.0f);

            GameSpriteMan.Add(GameSprite.Name.BombZigZag, Image.Name.SquigglyShotB, 24, 26, 9, 18);
            GameSpriteMan.Add(GameSprite.Name.BombStraight, Image.Name.RollingShotB, 70, 26, 9, 18);
            GameSpriteMan.Add(GameSprite.Name.BombDagger, Image.Name.PlungerShotA, 48, 27, 9, 18);

            float w = ShieldFactory.BRICK_WIDTH;
            float h = ShieldFactory.BRICK_HEIGHT;

            GameSpriteMan.Add(GameSprite.Name.Brick, Image.Name.Brick, 50, 25, w, h);
            GameSpriteMan.Add(GameSprite.Name.Brick_LeftTop0, Image.Name.BrickLeft_Top0, 50, 25, w, h);
            GameSpriteMan.Add(GameSprite.Name.Brick_LeftTop1, Image.Name.BrickLeft_Top1, 50, 25, w, h);
            GameSpriteMan.Add(GameSprite.Name.Brick_LeftBottom, Image.Name.BrickLeft_Bottom, 50, 25, w, h);
            GameSpriteMan.Add(GameSprite.Name.Brick_RightTop0, Image.Name.BrickRight_Top0, 50, 25, w, h);
            GameSpriteMan.Add(GameSprite.Name.Brick_RightTop1, Image.Name.BrickRight_Top1, 50, 25, w, h);
            GameSpriteMan.Add(GameSprite.Name.Brick_RightBottom, Image.Name.BrickRight_Bottom, 50, 25, w, h);

            GameSpriteMan.Add(GameSprite.Name.Floor, Image.Name.PlayerShot, 0.0f, 0.0f, 800.0f, 5.0f, new Azul.Color(0, 1, 0));

        }

        private void SetupInputs()
        {
            InputSubject pInputSubject;
            pInputSubject = InputMan.GetArrowRightSubject();
            pInputSubject.Attach(new KeyRightObserver());

            pInputSubject = InputMan.GetArrowLeftSubject();
            pInputSubject.Attach(new KeyLeftObserver());

            pInputSubject = InputMan.GetSpaceSubject();
            pInputSubject.Attach(new KeySpaceObserver());

            pInputSubject = InputMan.GetKSubject();
            pInputSubject.Attach(new KeyKObserver());

            pInputSubject = InputMan.Get1Subject();
            pInputSubject.Attach(new Key1Observer());

            pInputSubject = InputMan.Get2Subject();
            pInputSubject.Attach(new Key2Observer());
        }
    }
}

