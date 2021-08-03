using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ScenePlay : SceneState
    {
        // ---------------------------------------------------
        // Data
        // ---------------------------------------------------
        public SpriteBatchMan   poSpriteBatchMan;
        public TimerMan         poTimerMan;
        public ProxySpriteMan   poProxySpriteMan;
        public BoxSpriteMan     poBoxSpriteMan;
        public GameObjectMan    poGameObjectMan;
        public FontMan          poFontMan;
        public ShipMan          poShipMan;

        // hacky sack
        public static bool resetWave = true;

        public ScenePlay()
        {
            this.Initialize();
        }
        public override void Handle()
        {
            ScoreBoard.CheckHighScore();
            ScoreBoard.SetPlayerScoresToZero();
            SceneContext.SetState(SceneContext.Scene.Select);
        }
 

        public override void Initialize()
        {
            // These are unique to each scene
            // Individual instances of the manager will be created in each scene
            this.poSpriteBatchMan =     new SpriteBatchMan( 5, 1);
            this.poTimerMan =           new TimerMan(5, 1);
            this.poProxySpriteMan =     new ProxySpriteMan(60,5);
            this.poBoxSpriteMan =       new BoxSpriteMan(70, 5);
            this.poGameObjectMan =      new GameObjectMan(60,5);
            this.poFontMan =            new FontMan(5,1);

            SpriteBatchMan.SetActive(this.poSpriteBatchMan);
            TimerMan.SetActive(this.poTimerMan);
            ProxySpriteMan.SetActive(this.poProxySpriteMan);
            BoxSpriteMan.SetActive(this.poBoxSpriteMan);
            GameObjectMan.SetActive(this.poGameObjectMan);
            FontMan.SetActive(this.poFontMan);

            // Setup sprite batches
            CreateSpriteBatches();

            // Create permanent objects
            SetupWalls();
            SetupUI();
            CreateRootObjects();

            // Add the rest
            //AttachAnimations();
            SetupColPairs();

            //ClearWave();
            SpawnWave();
            //SetupUFO();      // this only needs to be set up once
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

                // walk through all objects and push to flyweight
                GameObjectMan.Update(); // Do this BEFORE collision pairs

                // Do the collision checks
                ColPairMan.Process();

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
            // This one is a little different
            ShipMan.SetActive(this.poShipMan);

            SceneContext.Scene pPlayer = SceneContext.ReturnActivePlayer();
            if (pPlayer == SceneContext.Scene.Player1)
            {
                ScoreBoard.SetCurrentPlayer(ScoreBoard.Player.Player1);
            }
            else
            {
                ScoreBoard.SetCurrentPlayer(ScoreBoard.Player.Player2);
            }

            // Add stuff that gets deleted
            // These will need to be set up again when a new game starts
            if (resetWave)
            {
                ClearWave();
                SpawnWave();
                //SetupUFO();      // this only needs to be set up once
            }

            SetupUFO();


            //SetupShip();
            //SetupMissile();
            resetWave = true;

            ScoreBoard.RefreshScoreDisplay();
        }

        // -----------------------
        // PRIVATE METHODS
        // -----------------------

        private void CreateSpriteBatches()
        {
            SpriteBatch pSB_Enemies = SpriteBatchMan.Add(SpriteBatch.Name.Enemies, 1);
            SpriteBatch pSB_Missile = SpriteBatchMan.Add(SpriteBatch.Name.Spaceship, 2);
            SpriteBatch pSB_UI = SpriteBatchMan.Add(SpriteBatch.Name.UI, 15);
            SpriteBatch pSB_Explosions = SpriteBatchMan.Add(SpriteBatch.Name.Explosions, 11);
            SpriteBatch pSB_Shields = SpriteBatchMan.Add(SpriteBatch.Name.Shields, 4);

            SpriteBatch pSB_Box = SpriteBatchMan.Add(SpriteBatch.Name.Boxes, 100);
            SpriteBatchMan.SetCollisionSB(SpriteBatch.Name.Boxes);
        }

        private void SetupUI()
        {
            FontMan.Add(Font.Name.Score1Label, SpriteBatch.Name.UI, "SCORE<1>", Glyph.Name.Consolas36pt, 50, 775);
            FontMan.Add(Font.Name.Score2Label, SpriteBatch.Name.UI, "SCORE<2>", Glyph.Name.Consolas36pt, 500, 775);
            FontMan.Add(Font.Name.HighScoreLabel, SpriteBatch.Name.UI, "HI-SCORE", Glyph.Name.Consolas36pt, 250, 775);
            Font player1 = FontMan.Add(Font.Name.Score1, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 75, 725);
            Font player2 = FontMan.Add(Font.Name.Score2, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 525, 725);
            Font highScore = FontMan.Add(Font.Name.HighScore, SpriteBatch.Name.UI, "0000", Glyph.Name.Consolas36pt, 275, 725);
            FontMan.Add(Font.Name.CreditsLabel, SpriteBatch.Name.UI, "CREDIT", Glyph.Name.Consolas36pt, 520, 20);
            FontMan.Add(Font.Name.Credits, SpriteBatch.Name.UI, "00", Glyph.Name.Consolas36pt, 650, 20);
            FontMan.Add(Font.Name.Lives, SpriteBatch.Name.UI, "-", Glyph.Name.Consolas36pt, 5, 20);

            FontMan.Add(Font.Name.GameOver, SpriteBatch.Name.UI, "Game Over", Glyph.Name.Consolas36pt, 250, 400, false);

            ScoreBoard.Create(player1, player2, highScore);
        }

        // Creates all the permanent root objects. These never get deleted so that we can maintain our collision pairs
        private void CreateRootObjects()
        {
            SpriteBatch pSB_Boxes = SpriteBatchMan.GetCollisionSB();
            SpriteBatch pSB_Enemies = SpriteBatchMan.Find(SpriteBatch.Name.Enemies);
            SpriteBatch pSB_Ship = SpriteBatchMan.Find(SpriteBatch.Name.Spaceship);

            // Enemy Grid
            EnemyFactory EF = new EnemyFactory(SpriteBatch.Name.Enemies);
            EnemyGrid pGrid = (EnemyGrid)EF.Create(GameObject.Name.EnemyGrid, EnemyCategory.Type.Grid);
            GameObjectMan.Attach(pGrid);

            // Shield Root
            ShieldFactory SF = new ShieldFactory(SpriteBatch.Name.Shields, SpriteBatch.Name.Boxes);
            ShieldRoot pShieldRoot = SF.CreateShieldRoot();
            GameObjectMan.Attach(pShieldRoot);

            // Ship Root (and ShipManager)
            ShipRoot pShipRoot = new ShipRoot(GameObject.Name.ShipRoot, GameSprite.Name.NullObject, 0.0f, 0.0f);
            GameObjectMan.Attach(pShipRoot);
            pShipRoot.ActivateGameSprite(pSB_Ship);
            pShipRoot.ActivateCollisionSprite(pSB_Boxes);
            this.poShipMan = ShipMan.Create();     // this is kind of wonky
            ShipMan.SetActive(this.poShipMan);

            // Missile Group
            MissileGroup pMissileGroup = new MissileGroup(GameObject.Name.MissileGroup, GameSprite.Name.NullObject, 0.0f, 0.0f);
            GameObjectMan.Attach(pMissileGroup);
            pMissileGroup.ActivateGameSprite(pSB_Ship);
            pMissileGroup.ActivateCollisionSprite(pSB_Boxes);

            // Bomb Root
            BombRoot pBombRoot = new BombRoot(GameObject.Name.BombRoot, GameSprite.Name.NullObject, 0.0f, 0.0f);
            GameObjectMan.Attach(pBombRoot);
            pMissileGroup.ActivateGameSprite(pSB_Enemies);
            pMissileGroup.ActivateCollisionSprite(pSB_Boxes);

            // UFO Root (and UFOManager)
            UFORoot pUFORoot = new UFORoot(GameObject.Name.UFORoot, GameSprite.Name.NullObject, 0.0f, 0.0f);
            pUFORoot.ActivateGameSprite(pSB_Enemies);
            pUFORoot.ActivateCollisionSprite(pSB_Boxes);
            GameObjectMan.Attach(pUFORoot);
            UFOMan.Create(); 
        }

        private void SetupGrid()
        {
            EnemyFactory EF = new EnemyFactory(SpriteBatch.Name.Enemies);
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);

            for (int i = 0; i < 11; i++)
            {
                float baseX = 50.0f;
                float baseY = 400.0f;
                baseY -= (pGrid.GetLevel() * 50.0f);

                // Column
                GameObject pCol = EF.Create((GameObject.Name)(i + GameObject.ENEMY_COL_0_IDX), EnemyCategory.Type.Column);

                // Octopus rows
                pCol.Add(EF.Create(GameObject.Name.Octopus, GameObject.Type.Octopus, baseX + i * 53.0f, baseY));
                pCol.Add(EF.Create(GameObject.Name.Octopus, GameObject.Type.Octopus, baseX + i * 53.0f, baseY + 55));

                // Crab rows
                pCol.Add(EF.Create(GameObject.Name.Crab, GameObject.Type.Crab, baseX + i * 53.0f, baseY + 110));
                pCol.Add(EF.Create(GameObject.Name.Crab, GameObject.Type.Crab, baseX + i * 53.0f, baseY + 165));

                // Squid
                pCol.Add(EF.Create(GameObject.Name.Squid, GameObject.Type.Squid, baseX + i * 53.0f, baseY + 220));

                pGrid.Add(pCol);
            }

            // This is a *little* dangerous
            //pGrid.DropBomb();
            TimerMan.Add(TimeEvent.Name.NewBombSpawn, new NewBombSpawnEvent(), 2f);
            pGrid.ResetGridMembers();
            pGrid.QueueGridMovement();
        }

        private void SetupShields()
        {

            ShieldFactory SF = new ShieldFactory(SpriteBatch.Name.Shields, SpriteBatch.Name.Boxes);
            ShieldRoot pShieldRoot = (ShieldRoot)GameObjectMan.Find(GameObject.Name.ShieldRoot);

            SF.CreateEntireShield(pShieldRoot, GameObject.Name.Shield_0, 75.0f, 150.0f);
            SF.CreateEntireShield(pShieldRoot, GameObject.Name.Shield_1, 233.0f, 150.0f);
            SF.CreateEntireShield(pShieldRoot, GameObject.Name.Shield_2, 392.0f, 150.0f);
            SF.CreateEntireShield(pShieldRoot, GameObject.Name.Shield_3, 550.0f, 150.0f);
        }

        private void SetupWalls()
        {

            SpriteBatch pSB_Box = SpriteBatchMan.GetCollisionSB();
            SpriteBatch pSB_Shields = SpriteBatchMan.Find(SpriteBatch.Name.Shields);

            // Wall Root
            WallGroup pWallGroup = new WallGroup(GameObject.Name.WallGroup, GameSprite.Name.NullObject, 0.0f, 0.0f);
            pWallGroup.ActivateCollisionSprite(pSB_Box);

            WallRight pWallRight = new WallRight(GameObject.Name.WallRight, GameSprite.Name.NullObject, 690, 375, 10, 650);
            pWallRight.ActivateCollisionSprite(pSB_Box);

            WallLeft pWallLeft = new WallLeft(GameObject.Name.WallLeft, GameSprite.Name.NullObject, 10, 375, 10, 650);
            pWallLeft.ActivateCollisionSprite(pSB_Box);

            WallTop pWallTop = new WallTop(GameObject.Name.WallTop, GameSprite.Name.NullObject, 350, 700, 700, 5);
            pWallTop.ActivateCollisionSprite(pSB_Box);

            WallBottom pWallBottom = new WallBottom(GameObject.Name.WallBottom, GameSprite.Name.Floor, 350, 40.0f, 700, 5);
            pWallBottom.ActivateCollisionSprite(pSB_Box);
            pWallBottom.ActivateGameSprite(pSB_Shields);


            // Add to the composite the children
            pWallGroup.Add(pWallRight);
            pWallGroup.Add(pWallLeft);
            pWallGroup.Add(pWallTop);
            pWallGroup.Add(pWallBottom);
            GameObjectMan.Attach(pWallGroup);
        }

        private void SetupShip()
        {
            //ShipRoot pShipRoot = (ShipRoot)GameObjectMan.Find(GameObject.Name.ShipRoot);
            //SpriteBatch pSB_Box = SpriteBatchMan.GetCollisionSB();
            //SpriteBatch pSB_Ship = SpriteBatchMan.Find(SpriteBatch.Name.Spaceship);

            ShipMan.SpawnShip();

            //pShipRoot.ActivateGameSprite(pSB_Ship);
            //pShipRoot.ActivateCollisionSprite(pSB_Box);
        }

        private void SetupMissile()
        {
            //MissileGroup pMissileGroup = (MissileGroup)GameObjectMan.Find(GameObject.Name.MissileGroup);
            //SpriteBatch pSB_Missile = SpriteBatchMan.Find(SpriteBatch.Name.Spaceship);

            //pMissileGroup.ActivateGameSprite(pSB_Missile);
            //pMissileGroup.ActivateCollisionSprite(SpriteBatchMan.GetCollisionSB());
        }

        private void SetupBomb()
        {
            //BombRoot pBombRoot = (BombRoot)GameObjectMan.Find(GameObject.Name.BombRoot);
            //SpriteBatch pSB_Enemies = SpriteBatchMan.Find(SpriteBatch.Name.Enemies);

            //pBombRoot.ActivateGameSprite(pSB_Enemies);
            //pBombRoot.ActivateCollisionSprite(SpriteBatchMan.GetCollisionSB());
        }

        private void SetupUFO()
        {
            /***
             *  SOMETHING IS WRONG HERE --- GAME IS HANGING 
             */

            // Start a countdown until new UFO
            UFOMan.ClearOldUFO();
            UFOMan.SpawnNewUFO();
        }

        private void AttachAnimations()
        {
            //EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);
            //float gridSpeed = pGrid.GetGridSpeed();

            //TimerMan.Add(TimeEvent.Name.GridMovement, new GridMovement(pGrid), gridSpeed);

            //AnimationSprite pAnimSprite = new EnemyAnimation(GameSprite.Name.OctopusA, pGrid);
            //pAnimSprite.Attach(Image.Name.OctopusB);
            //pAnimSprite.Attach(Image.Name.OctopusA);
            //TimerMan.Add(TimeEvent.Name.SpriteAnimation, pAnimSprite, gridSpeed);

            //pAnimSprite = new EnemyAnimation(GameSprite.Name.SquidA, pGrid);
            //pAnimSprite.Attach(Image.Name.SquidB);
            //pAnimSprite.Attach(Image.Name.SquidA);
            //TimerMan.Add(TimeEvent.Name.SpriteAnimation, pAnimSprite, gridSpeed);

            //pAnimSprite = new EnemyAnimation(GameSprite.Name.CrabA, pGrid);
            //pAnimSprite.Attach(Image.Name.CrabB);
            //pAnimSprite.Attach(Image.Name.CrabA);
            //TimerMan.Add(TimeEvent.Name.SpriteAnimation, pAnimSprite, gridSpeed);

            AnimationSprite pAnimSprite = new AnimationSprite(GameSprite.Name.PlayerExplosionA);
            pAnimSprite.Attach(Image.Name.PlayerExplosionB);
            pAnimSprite.Attach(Image.Name.PlayerExplosionA);
            TimerMan.Add(TimeEvent.Name.SpriteAnimation, pAnimSprite, 0.4f);

        }

        // Note: These must be set up AFTER the associated objects have stored in the manager
        private void SetupColPairs()
        {
            WallGroup pWallGroup = (WallGroup)GameObjectMan.Find(GameObject.Name.WallGroup);
            WallTop pWallTop = null;
            WallLeft pWallLeft = null;
            WallRight pWallRight = null;
            WallBottom pWallBottom = null;
            GetFourWalls(pWallGroup, ref pWallTop, ref pWallBottom, ref pWallLeft, ref pWallRight);

            ShieldRoot pShieldRoot = (ShieldRoot)GameObjectMan.Find(GameObject.Name.ShieldRoot);

            MissileGroup pMissileGroup = (MissileGroup)GameObjectMan.Find(GameObject.Name.MissileGroup);
            BombRoot pBombRoot = (BombRoot)GameObjectMan.Find(GameObject.Name.BombRoot);

            UFORoot pUFORoot = (UFORoot)GameObjectMan.Find(GameObject.Name.UFORoot);
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);
            ShipRoot pShipRoot = (ShipRoot)GameObjectMan.Find(GameObject.Name.ShipRoot);

            // associate in a collision pair                                                                                
            ColPair pColPair = ColPairMan.Add(ColPair.Name.Enemy_Wall, pGrid, pWallLeft);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new GridObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Enemy_Wall, pGrid, pWallRight);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new GridObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Enemy_Floor, pGrid, pWallBottom);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new GridObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Enemy_Missile, pGrid, pMissileGroup);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new ShipReadyObserver());
            pColPair.Attach(new RemoveMissileObserver());
            pColPair.Attach(new RemoveEnemyObserver());
            pColPair.Attach(new IncreaseGridSpeedObserver((EnemyGrid)pGrid));
            pColPair.Attach(new EnemyDefeatedSoundObserver());
            pColPair.Attach(new AwardPointsObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.AlienExplosion, 0.25f));
            pColPair.Attach(new CheckGridDeadObserver(pGrid));

            pColPair = ColPairMan.Add(ColPair.Name.Bomb_Missile, pBombRoot, pMissileGroup);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveMissileObserver(true));
            pColPair.Attach(new ShipReadyObserver());
            pColPair.Attach(new RemoveBombObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.PlayerShotExplosion, 0.25f, true));
            pColPair.Attach(new SpawnBombObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Enemy_Shield, pGrid, pShieldRoot);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveBrickObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Missile_Wall, pMissileGroup, pWallTop);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new ShipReadyObserver());
            pColPair.Attach(new RemoveMissileObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.PlayerShotExplosion, 0.25f, true));

            pColPair = ColPairMan.Add(ColPair.Name.Ship_LeftWall, pShipRoot, pWallLeft);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new ShipLeftWallObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Ship_RightWall, pShipRoot, pWallRight);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new ShipRightWallObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Missile_Shield, pMissileGroup, pShieldRoot);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveMissileObserver());
            pColPair.Attach(new ShipReadyObserver());
            pColPair.Attach(new RemoveBrickObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.PlayerShotExplosion, 0.25f, false));

            pColPair = ColPairMan.Add(ColPair.Name.Bomb_Shield, pBombRoot, pShieldRoot);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveBombObserver());
            pColPair.Attach(new RemoveBrickObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.PlayerShotExplosion, 0.25f, true));
            pColPair.Attach(new SpawnBombObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Enemy_Ship, pGrid, pShipRoot);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveShipObserver());
            pColPair.Attach(new ShipDestroyedObserver(this));
            pColPair.Attach(new PlayerDefeatedSoundObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.PlayerExplosionA, 1.6f));

            pColPair = ColPairMan.Add(ColPair.Name.Bomb_Ship, pBombRoot, pShipRoot);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveShipObserver());
            pColPair.Attach(new ShipDestroyedObserver(this));
            pColPair.Attach(new PlayerDefeatedSoundObserver());
            pColPair.Attach(new RemoveBombObserver());
            pColPair.Attach(new SpawnBombObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.PlayerExplosionA, 1.6f));

            pColPair = ColPairMan.Add(ColPair.Name.Bomb_Wall, pBombRoot, pWallBottom);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveBombObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.PlayerShotExplosion, 0.25f, true));
            pColPair.Attach(new SpawnBombObserver());

            pColPair = ColPairMan.Add(ColPair.Name.Missile_UFO, pMissileGroup, pUFORoot);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveMissileObserver());
            pColPair.Attach(new ShipReadyObserver());
            pColPair.Attach(new ExplosionImageObserver(GameSprite.Name.SaucerExplosion, 1.0f));
            pColPair.Attach(new RemoveUFOObserver());
            pColPair.Attach(new EnemyDefeatedSoundObserver(true));
            pColPair.Attach(new AwardPointsObserver());

            pColPair = ColPairMan.Add(ColPair.Name.UFO_Wall, pUFORoot, pWallLeft);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveUFOObserver(true));

            pColPair = ColPairMan.Add(ColPair.Name.UFO_Wall, pUFORoot, pWallRight);
            Debug.Assert(pColPair != null);
            pColPair.Attach(new RemoveUFOObserver(true));
        }

        private void ShieldStressTest()
        {
            Random pRandom = new Random();

            for (int i = 0; i < 400; i++)
            {
                float time = (float)pRandom.Next(100, 10000) / 1000.0f + 1.0f;
                Debug.WriteLine("set--->time: {0} ", time);
                TimerMan.Add(TimeEvent.Name.MissileSpawn, new MissileSpawnEvent(pRandom), time);
            }

            for (int i = 0; i < 25; i++)
            {
                float time = (float)pRandom.Next(100, 10000) / 1000.0f + 1.0f;
                //Debug.WriteLine("set--->time: {0} ", time);
                TimerMan.Add(TimeEvent.Name.BombSpawn, new BombSpawnEvent(pRandom), time);
            }
        }

        private void GetFourWalls(WallGroup pGroup, ref WallTop pWT, ref WallBottom pWB, ref WallLeft pWL, ref WallRight pWR)
        {
            Debug.Assert(pGroup != null);

            pWT = null;
            pWB = null;
            pWL = null;
            pWR = null;

            ForwardIterator pIt = new ForwardIterator(pGroup);

            Component pW = pIt.First();
            while (!pIt.IsDone())
            {
                GameObject pGameObj = (GameObject)pW;
                GameObject.Name name = pGameObj.GetName();

                if (name == GameObject.Name.WallTop)
                {
                    pWT = (WallTop)pGameObj;
                }
                else if (name == GameObject.Name.WallBottom)
                {
                    pWB = (WallBottom)pGameObj;
                }
                else if (name == GameObject.Name.WallLeft)
                {
                    pWL = (WallLeft)pGameObj;
                }
                else if (name == GameObject.Name.WallRight)
                {
                    pWR = (WallRight)pGameObj;
                }

                pW = pIt.Next();
            }

            Debug.Assert(pWT != null);
            Debug.Assert(pWB != null);
            Debug.Assert(pWL != null);
            Debug.Assert(pWR != null);
        }

        // Create a brand new wave.
        // Anything that MAY have been deleted should be recreated
        public void SpawnWave()
        {
            // Make sure our Timer is clear out and then set everything up
            //TimerMan.ClearTimerEvents();
            SetupGrid();
            SetupShields();
            SetupShip();
            SetupMissile();
            SetupBomb();
            //SetupUFO();

            AttachAnimations();
        }

        public void ClearWave(bool clearTimeEvents = true)
        {
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);
            ShieldRoot pShieldRoot = (ShieldRoot)GameObjectMan.Find(GameObject.Name.ShieldRoot);
            ShipRoot pShipRoot = (ShipRoot)GameObjectMan.Find(GameObject.Name.ShipRoot);
            MissileGroup pMissileGroup = (MissileGroup)GameObjectMan.Find(GameObject.Name.MissileGroup);
            BombRoot pBombRoot = (BombRoot)GameObjectMan.Find(GameObject.Name.BombRoot);
            UFORoot pUFORoot = (UFORoot)GameObjectMan.Find(GameObject.Name.UFORoot);

            pGrid.RemoveAllChildren();
            pShieldRoot.RemoveAllChildren();
            pShipRoot.RemoveAllChildren();
            pMissileGroup.RemoveAllChildren();
            pBombRoot.RemoveAllChildren();
            pUFORoot.RemoveAllChildren();

            if (clearTimeEvents)
            {
                TimerMan.ClearTimerEvents();
            }
        }


        public void PlayerDies()
        {
            MissileGroup pMissileGroup = (MissileGroup)GameObjectMan.Find(GameObject.Name.MissileGroup);
            BombRoot pBombRoot = (BombRoot)GameObjectMan.Find(GameObject.Name.BombRoot);
            UFORoot pUFORoot = (UFORoot)GameObjectMan.Find(GameObject.Name.UFORoot);
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);

            // Missiles will be enabled when the ship respawns
            // UFO will automatically set itself up to respawn when it's removed
            // We need to cue the bombs to start again
            //pMissileGroup.RemoveAllChildren();
            //pBombRoot.RemoveAllChildren();
            pUFORoot.RemoveAllChildren();
            UFOMan.ClearOldUFO();          // UFO is giving us a lot of shit, so we're just going to nuke it

            pGrid.PauseGrid();
        }


        public void GameOver()
        {
            EnemyGrid pGrid = (EnemyGrid)GameObjectMan.Find(GameObject.Name.EnemyGrid);
            pGrid.RemoveAllChildren();

            SpriteBatch pSB = SpriteBatchMan.Find(SpriteBatch.Name.UI);
            Font pFont = FontMan.Find(Font.Name.GameOver);
            FontMan.AddToSB(pFont, pSB);


            pGrid.StopGrid();

            TimerMan.Add(TimeEvent.Name.GoToSelect, new BackToSelectCommand(pFont), 5.0f);
        }

        public static void SwitchToOtherPlayer()
        {
            // No validation here!

            SceneContext.Scene pPlayer = SceneContext.ReturnActivePlayer();
            if (pPlayer == SceneContext.Scene.Player1)
            {
                resetWave = false;
                SceneContext.SetState(SceneContext.Scene.Player2);
            }
            else
            {
                resetWave = false;
                SceneContext.SetState(SceneContext.Scene.Player1);
            }
        }

        public override void InputLeft()
        {
            //Debug.WriteLine("Move Left");
            Ship pShip = ShipMan.GetShip();
            pShip.MoveLeft();
        }

        public override void InputRight()
        {
            //Debug.WriteLine("Move Left");
            Ship pShip = ShipMan.GetShip();
            pShip.MoveRight();
        }

        public override void InputSpace()
        {
            //Debug.WriteLine("Shoot Observer");
            Ship pShip = ShipMan.GetShip();
            pShip.ShootMissile();
        }

        public override void InputK()
        {
            SpriteBatchMan.Find(SpriteBatch.Name.Boxes).ToggleRender();
        }
    }
}
