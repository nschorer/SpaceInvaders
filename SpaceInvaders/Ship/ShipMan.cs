using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShipMan
    {
        public enum State
        {
            MissileReady,
            MissileFlying,
            MissileNone,
            MovementFree,
            MovementNoLeft,
            MovementNoRight,
            MovementNone,
            End
        }

        private ShipMan()
        {
            // Store the states
            this.pStateMissileReady = new ShipStateMissileReady();
            this.pStateMissileFlying = new ShipStateMissileFlying();
            this.pStateMissileNone = new ShipStateMissileNone();
            this.pStateMovementFree = new ShipStateMovementFree();
            this.pStateMovementNone = new ShipStateMovementNone();
            this.pStateMovementNoLeft = new ShipStateMovementNoLeft();
            this.pStateMovementNoRight = new ShipStateMovementNoRight();
            this.pStateEnd = new ShipStateEnd();

            // set active
            this.pShip = null;
            this.pMissile = null;
            this.pLivesMan = new LivesMan();
        }

        public static ShipMan Create()
        {
            Debug.Assert(pPlayer1 == null || pPlayer2 == null);      // we should call this either once or twice, no more

            bool isPlayerOne = false;

            // Do the initialization
            if (instance == null)
            {
                isPlayerOne = true;
                pEng = SpaceInvaders.GetSoundEngine();
            }

            instance = new ShipMan();
            Debug.Assert(instance != null);

            if (isPlayerOne)
            {
                pPlayer1 = instance;
            }
            else
            {
                pPlayer2 = instance;
            }

            return instance;
        }

        public static void SetActive(ShipMan pShipMan)
        {
            Debug.Assert(pShipMan != null);
            ShipMan.pActiveShipMan = pShipMan;
        }

        private static ShipMan PrivInstance()
        {
            Debug.Assert(instance != null);

            return instance;
        }

        public static Ship GetShip()
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;

            Debug.Assert(pShipMan != null);
            Debug.Assert(pShipMan.pShip != null);

            return pShipMan.pShip;
        }

        public static ShipState GetState(State state)
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;
            Debug.Assert(pShipMan != null);

            ShipState pShipState = null;

            switch (state)
            {
                case ShipMan.State.MissileReady:
                    pShipState = pShipMan.pStateMissileReady;
                    break;

                case ShipMan.State.MissileFlying:
                    pShipState = pShipMan.pStateMissileFlying;
                    break;

                case ShipMan.State.MissileNone:
                    pShipState = pShipMan.pStateMissileNone;
                    break;

                case ShipMan.State.MovementFree:
                    pShipState = pShipMan.pStateMovementFree;
                    break;

                case ShipMan.State.MovementNone:
                    pShipState = pShipMan.pStateMovementNone;
                    break;

                case ShipMan.State.MovementNoLeft:
                    pShipState = pShipMan.pStateMovementNoLeft;
                    break;

                case ShipMan.State.MovementNoRight:
                    pShipState = pShipMan.pStateMovementNoRight;
                    break;

                case ShipMan.State.End:
                    pShipState = pShipMan.pStateEnd;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            return pShipState;
        }

        public static Missile GetMissile()
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;

            Debug.Assert(pShipMan != null);
            Debug.Assert(pShipMan.pMissile != null);

            return pShipMan.pMissile;
        }

        public static Missile ActivateMissile()
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;
            Debug.Assert(pShipMan != null);

            // copy over safe copy
            Missile pMissile = (Missile)GameObjectMan.ReuseGameObject(GameObject.Name.Missile);
            if (pMissile == null)
            {
                pMissile = new Missile(GameObject.Name.Missile, GameSprite.Name.Missile, 400, 100);
            }
            pShipMan.pMissile = pMissile;

            // Attached to SpriteBatches
            SpriteBatch pSB_Aliens = SpriteBatchMan.Find(SpriteBatch.Name.Enemies);
            SpriteBatch pSB_Boxes = SpriteBatchMan.Find(SpriteBatch.Name.Boxes);

            pMissile.ActivateCollisionSprite(pSB_Boxes);
            pMissile.ActivateGameSprite(pSB_Aliens);

            // Attach the missile to the missile root
            GameObject pMissileGroup = GameObjectMan.Find(GameObject.Name.MissileGroup);
            Debug.Assert(pMissileGroup != null);

            // Add to GameObject Tree - {update and collisions}
            pMissileGroup.Add(pShipMan.pMissile);

            pEng.Play2D("shoot.wav");

            return pShipMan.pMissile;
        }

        public static void SpawnShip()
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;
            Debug.Assert(pShipMan != null);

            // I really shouldn't be doing this here, but I'm tired
            UFOMan.ClearOldUFO();
            UFOMan.SpawnNewUFO();

            pShipMan.pShip = ActivateShip();
            pShipMan.pShip.SetMissileState(ShipMan.State.MissileReady);
            pShipMan.pShip.SetMovementState(ShipMan.State.MovementFree);
        }

        private static Ship ActivateShip()
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;
            Debug.Assert(pShipMan != null);

            // copy over safe copy
            Ship pShip = (Ship)GameObjectMan.ReuseGameObject(GameObject.Name.Ship);
            if (pShip == null)
            {
                pShip = new Ship(GameObject.Name.Ship, GameSprite.Name.Ship, 200, 100, pShipMan);
            }
            pShipMan.pShip = pShip;

            // Attach the sprite to the correct sprite batch
            SpriteBatch pSB_Ship = SpriteBatchMan.Find(SpriteBatch.Name.Spaceship);

            pShip.ActivateGameSprite(pSB_Ship);
            pShip.ActivateCollisionSprite(SpriteBatchMan.GetCollisionSB());

            // Attach the missile to the missile root
            GameObject pShipRoot = GameObjectMan.Find(GameObject.Name.ShipRoot);
            Debug.Assert(pShipRoot != null);

            // Add to GameObject Tree - {update and collisions}
            pShipRoot.Add(pShipMan.pShip);

            return pShipMan.pShip;
        }

        public void ShipDestroyed()
        {
            // Current Player Lives
            pLivesMan.LoseLife();
            bool thisPlayerGameOver = true;
            if (pLivesMan.GetLives() > 0)
            {
                TimerMan.Add(TimeEvent.Name.SpawnNewShip, new SpawnNewShip(), 3.0f);
                thisPlayerGameOver = false;
            }

            // Other PlayerLives
            bool otherPlayerGameOver = true;
            if (DoesOtherPlayerHaveLives())
            {
                otherPlayerGameOver = false;
                TimerMan.Add(TimeEvent.Name.SwitchPlayer, new SwitchToOtherPlayerEvent(), 2f);
            }

            // Both
            if (thisPlayerGameOver && otherPlayerGameOver)
            {
                //Game Over
                ScenePlay pScene = (ScenePlay)SceneContext.GetState();
                pScene.GameOver();
            }
        }

        // Automatic game over for the person playing
        public static void AliensLanded()
        {
            // Other PlayerLives
            bool otherPlayerGameOver = true;
            if (DoesOtherPlayerHaveLives())
            {
                otherPlayerGameOver = false;
                TimerMan.Add(TimeEvent.Name.SwitchPlayer, new SwitchToOtherPlayerEvent(), 2f);
            }

            // Both
            if (otherPlayerGameOver)
            {
                //Game Over
                ScenePlay pScene = (ScenePlay)SceneContext.GetState();
                pScene.GameOver();
            }

        }

        public static int GetLives()
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;
            return pShipMan.pLivesMan.GetLives();
        }

        private static int GetLives(ShipMan pShipMan)
        {
            Debug.Assert(pShipMan != null);
            return pShipMan.pLivesMan.GetLives();
        }

        public static void ResetLives()
        {
            //ShipMan pShipMan = ShipMan.PrivInstance();
            ShipMan pShipMan = ShipMan.pActiveShipMan;
            pShipMan.pLivesMan.ResetLives();
        }

        public static bool DoesOtherPlayerHaveLives()
        {
            bool isPlayer1 = (pActiveShipMan == pPlayer1);

            if (isPlayer1)
            {
                if (!SceneContext.IsTwoPlayer())
                {
                    return false;
                }
                else
                {
                    return GetLives(pPlayer2) > 0;
                }
            }
            else
            {
                return GetLives(pPlayer1) > 0;
            }
        }

        // Data: ----------------------------------------------
        private static ShipMan instance = null;
        private static ShipMan pActiveShipMan = null;
        private static ShipMan pPlayer1 = null;
        private static ShipMan pPlayer2 = null;

        // Active
        private Ship pShip;
        private Missile pMissile;
        private LivesMan pLivesMan;

        private static IrrKlang.ISoundEngine pEng;

        // Reference
        private ShipStateMissileReady pStateMissileReady;
        private ShipStateMissileFlying pStateMissileFlying;
        private ShipStateMissileNone pStateMissileNone;
        private ShipStateMovementFree pStateMovementFree;
        private ShipStateMovementNone pStateMovementNone;
        private ShipStateMovementNoLeft pStateMovementNoLeft;
        private ShipStateMovementNoRight pStateMovementNoRight;
        private ShipStateEnd pStateEnd;

    }
}
