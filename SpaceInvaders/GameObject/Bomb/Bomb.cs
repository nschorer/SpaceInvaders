
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Bomb : BombCategory
    {
        //public Bomb(GameObject.Name name, GameSprite.Name spriteName, FallStrategy strategy, float posX, float posY)
        //    : base(name, spriteName, BombCategory.Type.Bomb)
        //{
        //    this.x = posX;
        //    this.y = posY;
        //    this.delta = 6.0f;

        //    Debug.Assert(strategy != null);
        //    this.pStrategy = strategy;

        //    this.pStrategy.Reset(this.y);

        //    this.poColObj.pColSprite.SetLineColor(1, 1, 0);
        //}

        private Bomb(GameObject.Name name, GameSprite.Name spriteName, FallStrategy strategy, float posX, float posY)
            : base(name, spriteName, BombCategory.Type.Bomb)
        {
            this.x = posX;
            this.y = posY;
            this.delta = 6.0f;

            Debug.Assert(strategy != null);
            this.pStrategy = strategy;

            this.pStrategy.Reset(this.y);

            this.poColObj.pColSprite.SetLineColor(1, 1, 0);
        }

        public static Bomb CreateBomb(GameObject.Name name, float bombX, float bombY)
        {
            if (Bomb.pFallStraight == null)
            {
                Bomb.SetupFallStrategies();
            }

            //Bomb pBomb = null;
            Bomb pBomb = (Bomb)GameObjectMan.ReuseGameObject(name);

            if (pBomb == null)
            {
                switch (name)
                {
                    case GameObject.Name.BombStraight:
                        pBomb = new Bomb(GameObject.Name.BombStraight, GameSprite.Name.BombStraight, pFallStraight, bombX, bombY);
                        break;
                    case GameObject.Name.BombDagger:
                        pBomb = new Bomb(GameObject.Name.BombDagger, GameSprite.Name.BombDagger, pFallDagger, bombX, bombY);
                        break;
                    case GameObject.Name.BombZigZag:
                        pBomb = new Bomb(GameObject.Name.BombZigZag, GameSprite.Name.BombZigZag, pFallZigZag, bombX, bombY);
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            else
            {
                pBomb.Set(bombX, bombY);
                pBomb.pStrategy.Reset(pBomb.y);
            }

            pBomb.Update();
            return pBomb;
        }

        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Reset()
        {
            this.y = 700.0f;
            this.pStrategy.Reset(this.y);
        }
        //public override void Remove()
        //{
        //    // Since the Root object is being drawn
        //    // 1st set its size to zero
        //    this.poColObj.poColRect.Set(0, 0, 0, 0);
        //    base.Update();

        //    // Now remove it
        //    base.Remove();

        //    // Update the parent (missile root)
        //    GameObject pParent = (GameObject)this.pParent;
        //    pParent.Update();
        //}

            
        //}
        public override void Update()
        {
            base.Update();
            this.y -= delta;

            // Strategy
            this.pStrategy.Fall(this);
        }
        public float GetBoundingBoxHeight()
        {
            return this.poColObj.poColRect.height;
        }
        ~Bomb()
        {
        }
        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitBomb(this);
        }
        public override void VisitShieldBrick(ShieldBrick s)
        {
            // Bomb vs ShieldBrick
            //Debug.WriteLine(" ---> Done");
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(this, s);
            pColPair.NotifyListeners();
        }

        public void SetPos(float xPos, float yPos)
        {
            this.x = xPos;
            this.y = yPos;
        }
        public void MultiplyScale(float sx, float sy)
        {
            Debug.Assert(this.pProxySprite != null);

            this.pProxySprite.sx *= sx;
            this.pProxySprite.sy *= sy;
        }

        public static void SetupFallStrategies()
        {
            pFallStraight = new FallStraight();
            pFallDagger = new FallDagger();
            pFallZigZag = new FallZigZag();
        }

        // Data
        new public float delta;
        private FallStrategy pStrategy;
        private static FallStraight pFallStraight;
        private static FallDagger pFallDagger;
        private static FallZigZag pFallZigZag;
    }
}

// End of File
