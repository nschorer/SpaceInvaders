using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Missile : MissileCategory
    {
        public Missile(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName)
        {
            this.x = posX;
            this.y = posY;
            this.bHit = false;
            this.enable = false;
            //this.pEng = new IrrKlang.ISoundEngine();
            
        }

        public override void Update()
        {
            base.Update();

            if (!bHit)
            {
                this.y += 13.0f;
                //pEng.Play2D("Missile.wav");
            }
        }

        ~Missile()
        {

        }

        public void Hit()
        {
            this.bHit = true;
        }

        public override void VisitGroup(EnemyGrid e)
        {
            // BirdGroup vs MissileGroup
            //Debug.WriteLine("         collide:  {0} <-> {1}", e.name, this.name);

            // MissileGroup vs Columns
            GameObject pGameObj = (GameObject)Iterator.GetChild(e);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitColumn(EnemyColumn e)
        {
            // BirdGroup vs MissileGroup
            //Debug.WriteLine("         collide:  {0} <-> {1}", e.name, this.name);

            // MissileGroup vs Columns
            GameObject pGameObj = (GameObject)Iterator.GetChild(e);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitOctopus(Octopus o)
        {
            // BirdGroup vs MissileGroup
            Debug.WriteLine("         collide:  {0} <-> {1}", this.name, o.name);

            // Missile vs Octopus
            //Debug.WriteLine(" ---> Done");
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(this, o );
            pColPair.NotifyListeners();
        }

        public override void VisitCrab(Crab c)
        {
            // BirdGroup vs MissileGroup
            Debug.WriteLine("         collide:  {0} <-> {1}", c.name, this.name);

            // Missile vs Crab
            //Debug.WriteLine(" ---> Done");
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(this, c);
            pColPair.NotifyListeners();
        }

        public override void VisitSquid(Squid s)
        {
            // BirdGroup vs MissileGroup
            Debug.WriteLine("         collide:  {0} <-> {1}", this.name, s.name);

            // Missile vs Squid
            //Debug.WriteLine(" ---> Done");
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(this, s);
            pColPair.NotifyListeners();
        }

        public override void VisitBomb(Bomb b)
        {
            // Bomb vs Missile
            Debug.WriteLine("         collide:  {0} <-> {1}", b.name, this.name);

            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(b, this);
            pColPair.NotifyListeners();
        }

        public override void VisitUFORoot(UFORoot u)
        {
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(u, pGameObj);
        }

        public override void VisitUFO(UFO u)
        {
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(this, u);
            pColPair.NotifyListeners();
        }

        //public override void Remove()
        //{
        //    // Keenan(delete.E)
        //    // Since the Root object is being drawn
        //    // 1st set its size to zero
        //    this.poColObj.poColRect.Set(0, 0, 0, 0);
        //    base.Update();

        //    // Update the parent (missile root)
        //    GameObject pParent = (GameObject)this.pParent;
        //    pParent.Update();

        //    // Now remove it
        //    base.Remove();
        //}

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Missile
            // Call the appropriate collision reaction            
            other.VisitMissile(this);
        }

        public void SetPos(float xPos, float yPos)
        {
            this.x = xPos;
            this.y = yPos;
        }

        public void SetActive(bool state)
        {
            this.enable = state;
        }

        // Data

        public bool bHit;
        private bool enable;
        //private IrrKlang.ISoundEngine pEng;
    }
}