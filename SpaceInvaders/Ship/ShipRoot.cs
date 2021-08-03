using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShipRoot: Composite
    {
        public ShipRoot(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName)
        {
            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(1, 1, 1);
        }

        ~ShipRoot()
        {
        }

        public override void VisitBombRoot(BombRoot b)
        {
            GameObject pGameObj = (GameObject)Iterator.GetChild(b);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitBomb(Bomb b)
        {
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(b, pGameObj);
        }

        public override void VisitGroup(EnemyGrid e)
        {
            // EnemyGrid vs ShipRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(e);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitColumn(EnemyColumn e)
        {
            // EnemyColumn vs ShipRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(e);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitCrab(Crab c)
        {
            // Crab vs ShipRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(c, pGameObj);
        }

        public override void VisitOctopus(Octopus o)
        {
            // Octopus vs ShipRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(o, pGameObj);
        }

        public override void VisitSquid(Squid s)
        {
            // Squid vs ShipRoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(s, pGameObj);
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitShipRoot(this);
        }
        public override void Update()
        {
            if (this.IsCompositeEmpty())
            {
                this.poColObj.poColRect.Set(new Azul.Rect(0, 0, 0, 0));
            }
            base.BaseUpdateBoundingBox(this);
            base.Update();
        }

      

        // Data: ---------------


    }
}
