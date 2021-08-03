using System.Diagnostics;

namespace SpaceInvaders
{
    abstract public class EnemyCategory : Leaf
    {
        public EnemyCategory(GameObject.Name name, GameSprite.Name spriteName)
            : base(name, spriteName)
        {

        }

        public override void Remove()
        {
            // Keenan(delete.E)
            // Since the Root object is being drawn
            // 1st set its size to zero
            //this.poColObj.poColRect.Set(0, 0, 0, 0);
            //base.Update();

            // Now remove it
            base.Remove();

            // Update the parent (enemy column)
            GameObject pParent = (GameObject)this.pParent;
            pParent.Update();
        }

        abstract public int GetPointValue();
    }
}
