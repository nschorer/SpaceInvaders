using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class UFORoot : Composite
    {
        public UFORoot(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName)
        {
            this.x = posX;
            this.y = posY;
        }

        public override void VisitMissileGroup(MissileGroup m)
        {
            GameObject pGameObj = (GameObject)Iterator.GetChild(m);
            ColPair.Collide(pGameObj, this);
        }

        public override void VisitMissile(Missile m)
        {
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);
            ColPair.Collide(m, pGameObj);
        }

        public override void Accept(ColVisitor other)
        {
            other.VisitUFORoot(this);
        }

        public override void Update()
        {
            GameObject pGameObj = (GameObject)Iterator.GetChild(this);

            // create a local pointer
            ColRect pColTotal = this.poColObj.poColRect;
            //ColRect pColTotal = new ColRect();

            if (pGameObj != null)
            {
                pColTotal.Set(pGameObj.poColObj.poColRect);
            }
            else
            {
                pColTotal.Set(new Azul.Rect(0, 0, 0, 0));
            }

            // Transfer to the game object its center
            this.x = this.poColObj.poColRect.x;
            this.y = this.poColObj.poColRect.y;

            ////Debug.WriteLine("x:{0} y:{1} w:{2} h:{3}", pColTotal.x, pColTotal.y, pColTotal.width, pColTotal.height);
            //base.BaseUpdateBoundingBox(this);
            base.Update();
        }
    }
}
