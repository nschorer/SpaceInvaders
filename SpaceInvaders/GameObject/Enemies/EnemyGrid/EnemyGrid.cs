using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class EnemyGrid : Composite
    {
        public enum State
        {
            MoveDown,
            MoveSideways,
            MoveTurnAround,
            MovePaused,
            MoveStopped
        }

        public EnemyGrid(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName)
        {
            this.x = posX;
            this.y = posY;
            this.poColObj.pColSprite.SetLineColor(0.0f, 1.0f, 0.0f);

            this.pRandom = new Random();
            EnemyGrid.startDeltaX = 10.0f;
            this.level = 0;
            pEng = SpaceInvaders.GetSoundEngine();
            this.pGridMovement = new GridMovement(this);

            ResetGridMembers();

            SetupAnimations();
            SetupBehaviorStates();
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an GreenBird
            // Call the appropriate collision reaction            
            other.VisitGroup(this);
        }

        public override void VisitMissileGroup(MissileGroup m)
        {
            // BirdGroup vs MissileGroup
            //Debug.WriteLine("         collide:  {0} <-> {1}", m.name, this.name);

            // MissileGroup vs Columns
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(m, pGameObj);
        }

        public override void Update()
        {
            if (this.IsCompositeEmpty())
            {
                this.poColObj.poColRect.Set(new Azul.Rect(0, 0, 0, 0));
            }
            else
            {
                DropBomb();
            }
            base.BaseUpdateBoundingBox(this);
            base.Update();
        }

        //public void MoveGrid()
        //{

        //    ForwardIterator pFor = new ForwardIterator(this);

        //    Component pNode = pFor.First();
        //    while (!pFor.IsDone())
        //    {
        //        GameObject pGameObj = (GameObject)pNode;
        //        pGameObj.x += this.gridDeltaX;

        //        pNode = pFor.Next();
        //    }
        //}

        //public void Advance()
        //{
        //    if (!isPaused)
        //    {
        //        if (!this.IsCompositeEmpty())
        //        {
        //            PlaySound();

        //            ForwardIterator pFor = new ForwardIterator(this);

        //            // Did the grid just collide with the wall?
        //            bool moveGridDown = hitWall && !droppedDown;

        //            Component pNode = pFor.First();
        //            while (!pFor.IsDone())
        //            {
        //                GameObject pGameObj = (GameObject)pNode;

        //                // Drop down...
        //                if (moveGridDown)
        //                {
        //                    pGameObj.y -= this.gridDeltaY;
        //                }
        //                // ... Or move to the side
        //                else
        //                {
        //                    pGameObj.x += this.gridDeltaX;
        //                }
        //                pNode = pFor.Next();
        //            }

        //            // If we just dropped down...
        //            if (moveGridDown)
        //            {
        //                // ... We're sitting on the wall... we need to get out!
        //                droppedDown = true;
        //                hitWall = true;
        //                this.gridDeltaX = -this.gridDeltaX;
        //            }

        //            // Otherwise, we just moved to the side
        //            else
        //            {
        //                // Note: if we ran into a wall, our observer will set hitWall to true later this frame
        //                hitWall = false;
        //                droppedDown = false;
        //            }

        //            TickAnimations();
        //            TimerMan.Add(TimeEvent.Name.GridMovement, new GridMovement(this), this.GetGridSpeed());
        //        }
        //    }
        //}

        public void Advance()
        {
            this.pGridBehavior.Advance();
        }

        public void DropBomb()
        {
            if (this.IsActive())
            {
                if (bombReady)
                {

                    EnemyColumn pCol = (EnemyColumn)ForwardIterator.GetChild(this);

                    //Debug.Assert(pCol != null);
                    if (pCol == null)
                    {
                        // Grid is empty so we can't drop bombs
                        return;
                    }



                    int nullCount = 0;
                    // Pick a random column from what we have left
                    int count = pRandom.Next(0, 20); // this seems random enough
                    while (count > 0)
                    {
                        // If all of the columns are empty, don't drop bombs!
                        if (nullCount >= 11)
                        {
                            return;
                        }

                        pCol = (EnemyColumn)ForwardIterator.GetSibling(pCol);

                        // Reset back to the start
                        if (pCol == null)
                        {
                            pCol = (EnemyColumn)ForwardIterator.GetChild(this);
                        }

                        // Don't drop a bomb from an empty column
                        if (ForwardIterator.GetChild(pCol) == null)
                        {
                            nullCount++;
                            continue;
                        }
                        else
                        {
                            nullCount = 0;
                            count--;
                        }
                    }

                    float bombX = pCol.poColObj.poColRect.x;
                    float bombY = pCol.poColObj.poColRect.y - pCol.poColObj.poColRect.height / 2;

                    int bombType = pRandom.Next(0, 3) + (int)GameObject.Name.BombStraight;

                    Bomb pBomb = Bomb.CreateBomb((GameObject.Name)bombType, bombX, bombY);

                    pBomb.ActivateCollisionSprite(SpriteBatchMan.Find(SpriteBatch.Name.Boxes));
                    pBomb.ActivateGameSprite(SpriteBatchMan.Find(SpriteBatch.Name.Enemies));
                    GameObjectMan.Find(GameObject.Name.BombRoot).Add(pBomb);

                    bombReady = false;
                }
            }
        }

        public void ResetGridMembers()
        {
            this.gridDeltaX = EnemyGrid.startDeltaX;
            this.gridDeltaY = 25.0f;
            if (gridDeltaY < 0) gridDeltaY *= -1;
            this.gridSpeed = 1.0f - (0.05f * level);
            this.bombReady = false;
            //this.isPaused = false;
            this.SetGridBehavior(State.MoveSideways);

            // hacking this in for now
            // this code will run after the first wave
            if (this.pMoveSideways != null)
            {
                this.pMoveSideways.UpdateDeltaX(this.gridDeltaX);
            }
        }

        public float GetDelta()
        {
            return this.gridDeltaX;
        }

        public void SetDelta(float inDelta)
        {
            this.gridDeltaX = inDelta;
        }

        public void HitWall()
        {
            if (this.GetGridBehavior() == this.pMoveSideways)
            {
                this.pGridBehavior.Handle();
            }
        }

        public void IncreaseGridSpeed()
        {
            // First 40ish
            if (gridSpeed > 0.7f)
            {
                gridSpeed *= 0.99f;
            }
            // Next 10ish
            else if (gridSpeed > 0.4f)
            {
                 gridSpeed *= 0.9f;
            }

            else
            {
                gridSpeed *= 0.8f;
            }
        }

        public float GetGridSpeed()
        {
            return gridSpeed;
        }

        private  void PlaySound()
        {
            //IrrKlang.ISoundSource temp = null;

            switch (sndIdx)
            {
                case 0:
                    //temp = snd0;
                    pEng.Play2D("fastinvader1.wav");
                    break;
                case 1:
                    //temp = snd1;
                    pEng.Play2D("fastinvader2.wav");
                    break;
                case 2:
                    //temp = snd2;
                    pEng.Play2D("fastinvader3.wav");
                    break;
                case 3:
                    //temp = snd3;
                    pEng.Play2D("fastinvader4.wav");
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            //pEng.Play2D(temp, false, false, false);
            //pEng.Play2D("fastinvader1.wav");
            sndIdx++;
            sndIdx %= 4;

        }

        public void SetBombReady()
        {
            bombReady = true;
        }

        //public void SetPaused(bool isPaused)
        //{
        //    this.isPaused = isPaused;
        //}

        private bool IsActive()
        {
            bool isActive = false;
            if (this.pGridBehavior == this.pMoveDown)
            {
                isActive = true;
            } else if (this.pGridBehavior == this.pMoveSideways)
            {
                isActive = true;
            } else if (this.pGridBehavior == this.pMoveTurnAround)
            {
                isActive = true;
            }

            return isActive;
        }

        public void PauseGrid()
        {
            this.pGridMovement = null;
            this.pMovePause.RememberLastBehavior(this.pGridBehavior);
            this.SetGridBehavior(State.MovePaused);
        }

        public void StopGrid()
        {
            this.pGridMovement = null;
            this.SetGridBehavior(State.MoveStopped);
        }

        public void Unpause()
        {
            //this.pGridMovement = new GridMovement(this); // Queue the repeated movements...
            //QueueGridMovement();
            this.pGridBehavior.Unpause();
            this.Advance();                         // But also move right when unpaused

            QueueGridMovement();
        }

        public int GetLevel()
        {
            return this.level;
        }

        public void CheckGridDead()
        {
            if (this.IsCompositeEmpty())
            {
                this.StopGrid();
                TimerMan.Add(TimeEvent.Name.NextLevel, new NextLevelEvent(this), 2f);
            }
        }

        public void NextLevel()
        {
            ScenePlay pScene = (ScenePlay)SceneContext.GetState();
            level++;
            pScene.ClearWave(false);
            pScene.SpawnWave();
        }

        private void SetupAnimations()
        {
            pAnimOcto = new EnemyAnimation(GameSprite.Name.OctopusA, this);
            pAnimOcto.Attach(Image.Name.OctopusB);
            pAnimOcto.Attach(Image.Name.OctopusA);

            pAnimSquid = new EnemyAnimation(GameSprite.Name.SquidA, this);
            pAnimSquid.Attach(Image.Name.SquidB);
            pAnimSquid.Attach(Image.Name.SquidA);

            pAnimCrab = new EnemyAnimation(GameSprite.Name.CrabA, this);
            pAnimCrab.Attach(Image.Name.CrabB);
            pAnimCrab.Attach(Image.Name.CrabA);
        }

        private void TickAnimations()
        {
            // We're not really using the TimerManager for these anymore, since the Timer is advancing all of the grid at the same time
            pAnimOcto.Execute(0.0f);
            pAnimCrab.Execute(0.0f);
            pAnimSquid.Execute(0.0f);
        }

        public void PlayMovementEffects()
        {
            PlaySound();
            TickAnimations();
        }

        private void SetupBehaviorStates()
        {
            pMoveDown = new GridBehaviorMoveDown(this, gridDeltaY);
            pMoveSideways = new GridBehaviorMoveSideways(this, gridDeltaX);
            pMoveTurnAround = new GridBehaviorTurnAround(this);
            pMovePause = new GridBehaviorPause(this);
            pMoveStop = new GridBehaviorStop(this);
        }

        public void ToggleGridDirection()
        {
            this.gridDeltaX *= -1;
            this.pMoveSideways.UpdateDeltaX(this.gridDeltaX);
        }


        public void SetGridBehavior(State pBehavior)
        {

            this.pGridBehavior = null;

            switch (pBehavior)
            {
                case State.MoveDown:
                    this.pGridBehavior = this.pMoveDown;
                    break;

                case State.MoveSideways:
                    this.pGridBehavior = this.pMoveSideways;
                    break;

                case State.MoveTurnAround:
                    this.pGridBehavior = this.pMoveTurnAround;
                    break;

                case State.MovePaused:
                    this.pGridBehavior = this.pMovePause;
                    break;

                case State.MoveStopped:
                    this.pGridBehavior = this.pMoveStop;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public void SetGridBehavior(GridBehavior pNewBehavior)
        {
            this.pGridBehavior = pNewBehavior;
        }

        public GridBehavior GetGridBehavior()
        {
            return this.pGridBehavior;
        }

        // All grid movements should be queued with this function
        public void QueueGridMovement()
        {
            if (this.IsActive())
            {
                if (pGridMovement == null)
                {
                    pGridMovement = new GridMovement(this);
                }
                TimerMan.Add(TimeEvent.Name.GridMovement, pGridMovement, this.GetGridSpeed()); // we need to start this up again
            }
        }

        // Data: ---------------

        private IrrKlang.ISoundEngine pEng;

        private Random pRandom;
        private EnemyAnimation pAnimOcto;
        private EnemyAnimation pAnimCrab;
        private EnemyAnimation pAnimSquid;

        private GridMovement pGridMovement;
        private GridBehavior pGridBehavior;
        private GridBehaviorMoveDown pMoveDown;
        private GridBehaviorMoveSideways pMoveSideways;
        private GridBehaviorTurnAround pMoveTurnAround;
        private GridBehaviorPause pMovePause;
        private GridBehaviorStop pMoveStop;

        private static float startDeltaX;
        private float gridDeltaX;
        private float gridDeltaY;
        private int sndIdx;
        private float gridSpeed;
        private int level;
        private bool bombReady = false;
        //private bool isPaused = false;
    }
}
