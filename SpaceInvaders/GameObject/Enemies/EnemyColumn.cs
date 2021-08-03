using System.Diagnostics;

namespace SpaceInvaders
{
    public class EnemyColumn : Composite
    {
        public EnemyColumn(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName)
        {
            this.x = posX;
            this.y = posY;
            this.poColObj.pColSprite.SetLineColor(1, 0, 0);
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an GreenBird
            // Call the appropriate collision reaction            
            other.VisitColumn(this);
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
            //ForwardIterator pIt = new ForwardIterator(this);

            //Component pNode = pIt.First();
            //Debug.Assert(pNode != null);

            //GameObject pGameObj = (GameObject)Iterator.GetChild(this);

            //// create a local pointer
            //ColRect pColTotal = this.poColObj.poColRect;

            //// Set it to the first child;
            //pColTotal.Set(pGameObj.poColObj.poColRect);

            ////Debug.WriteLine("\n");
            //while (!pIt.IsDone())
            //{
            //    pGameObj = (GameObject)pNode;
            //    //Debug.WriteLine("obj:{0} {1} ", pGameObj.GetHashCode(), pGameObj.GetName());

            //    // Inside union (x,y,width,height are updated)
            //    pColTotal.Union(pGameObj.poColObj.poColRect);
            //    pNode = pIt.Next();
            //}

            //// Transfer to the game object its center
            //this.x = this.poColObj.poColRect.x;
            //this.y = this.poColObj.poColRect.y;

            //Debug.WriteLine("x:{0} y:{1} w:{2} h:{3}", pColTotal.x, pColTotal.y, pColTotal.width, pColTotal.height);
            base.BaseUpdateBoundingBox(this);
            base.Update();
        }
    }
}
