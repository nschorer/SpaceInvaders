using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class WallLeft: WallCategory
    {
        public WallLeft(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY, float width, float height)
            : base(name, spriteName, WallCategory.Type.Left)
        {
            this.poColObj.poColRect.Set(posX, posY, width, height);

            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(1, 1, 0);
        }

        ~WallLeft()
        {

        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitWallLeft(this);
        }
        public override void Update()
        {
            // Go to first child
            base.Update();
        }



        public override void VisitGroup(EnemyGrid e)
        {
            // BirdGroup vs WallRight
            //Debug.WriteLine("\ncollide: {0} with {1}", this, e);
            //Debug.WriteLine("               --->DONE<----");

            ColPair pColPair = ColPairMan.GetActiveColPair();
            Debug.Assert(pColPair != null);

            pColPair.SetCollision(e, this);
            pColPair.NotifyListeners();
        }

        public override void VisitShipRoot(ShipRoot s)
        {

            ColPair pColPair = ColPairMan.GetActiveColPair();
            Debug.Assert(pColPair != null);

            pColPair.SetCollision(s, this);
            pColPair.NotifyListeners();
        }

        public override void VisitUFORoot(UFORoot u)
        {
            // WallLeft vs UFORoot
            GameObject pGameObj = (GameObject)Iterator.GetChild(u);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitUFO(UFO u)
        {
            ColPair pColPair = ColPairMan.GetActiveColPair();
            Debug.Assert(pColPair != null);

            pColPair.SetCollision(u, this);
            pColPair.NotifyListeners();
        }

        // Data: ---------------


    }
}
