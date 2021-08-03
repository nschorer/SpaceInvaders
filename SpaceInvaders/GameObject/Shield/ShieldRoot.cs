using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShieldRoot : Composite
    {
        public ShieldRoot(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName)
        {
            this.x = posX;
            this.y = posY;
            this.poColObj.pColSprite.SetLineColor(1.0f, 0.0f, 1.0f);
        }
        ~ShieldRoot()
        {
        }
        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitShieldRoot(this);
        }
        public override void VisitMissileGroup(MissileGroup m)
        {
            //Debug.Print("Missile Group <---> ShieldRoot");
            // MissileRoot vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(m);
            ColPair.Collide(pGameObj, this);
        }
        public override void VisitMissile(Missile m)
        {
            // Missile vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(m, pGameObj);
        }

        public override void VisitBombRoot(BombRoot b)
        {
            // BombRoot vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(b);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitBomb(Bomb b)
        {
            // Bomb vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(b, pGameObj);
        }

        public override void VisitGroup(EnemyGrid e)
        {
            // EnemyGrid vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(e);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitColumn(EnemyColumn e)
        {
            // EnemyColumn vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(e);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitCrab(Crab c)
        {
            // Crab vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(c, pGameObj);
        }

        public override void VisitOctopus(Octopus o)
        {
            // Octopus vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(o, pGameObj);
        }

        public override void VisitSquid(Squid s)
        {
            // Squid vs ShieldRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(s, pGameObj);
        }

        //public override void Update()
        //{
        //    ForwardIterator pIt = new ForwardIterator(this);

        //    Component pNode = pIt.First();
        //    Debug.Assert(pNode != null);

        //    GameObject pGameObj = (GameObject)Iterator.GetChild(this);
        //    //GameObject pGameObj = (GameObject)pIt.First();
        //    bool hasBeenSet = false;

        //    // create a local pointer
        //    ColRect pColTotal = this.poColObj.poColRect;
        //    //ColRect pColTotal = new ColRect();

        //    // Set it to the first child;
        //    //pColTotal.Set(pGameObj.poColObj.poColRect);

        //    //Debug.WriteLine("\n");
        //    while (!pIt.IsDone())
        //    {
        //        pGameObj = (GameObject)pNode;
        //        //Debug.WriteLine("obj:{0} {1} ", pGameObj.GetHashCode(), pGameObj.GetName());

        //        // Inside union (x,y,width,height are updated)
        //        if (pGameObj.holder == Container.LEAF)
        //        {
        //            if (!hasBeenSet)
        //            {
        //                pColTotal.Set(pGameObj.poColObj.poColRect);
        //                hasBeenSet = true;
        //            }
        //            else
        //            {
        //                pColTotal.Union(pGameObj.poColObj.poColRect);
        //            }
        //        }
        //        pNode = pIt.Next();
        //    }

        //    if (!hasBeenSet)
        //    {
        //        pColTotal.Set(new Azul.Rect(0, 0, 0, 0));
        //    }

        //    // Transfer to the game object its center
        //    this.x = this.poColObj.poColRect.x;
        //    this.y = this.poColObj.poColRect.y;

        //    ////Debug.WriteLine("x:{0} y:{1} w:{2} h:{3}", pColTotal.x, pColTotal.y, pColTotal.width, pColTotal.height);
        //    //base.BaseUpdateBoundingBox(this);
        //    base.Update();
        //}

        public override void Update()
        {
            if (this.IsCompositeEmpty())
            {
                this.poColObj.poColRect.Set(new Azul.Rect(0, 0, 0, 0));
            }

            base.BaseUpdateBoundingBox(this);
            base.Update();
        }

        // ------------------------------------------
        // Data:
        // ------------------------------------------


    }
}

// End of File
