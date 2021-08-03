using System.Diagnostics;

namespace SpaceInvaders
{
    public class SpriteNode : DLink
    {
        public SpriteNode()
        : base()
        {
            this.pSpriteBase = null;
            this.pBackSpriteNodeMan = null;
        }

        //public void Set(GameSprite.Name name)
        //{
        //    // Go find it
        //    this.pSpriteBase = GameSpriteMan.Find(name);
        //    Debug.Assert(this.pSpriteBase != null);
        //}

        //public void Set(BoxSprite.Name name)
        //{
        //    // Go find it
        //    this.pSpriteBase = BoxSpriteMan.Find(name);
        //    Debug.Assert(this.pSpriteBase != null);
        //}

        //public void Set(ProxySprite pNode)
        //{
        //    // associate it
        //    Debug.Assert(pNode != null);
        //    this.pSpriteBase = pNode;
        //}

        public void Set(SpriteBase pNode, SpriteNodeMan _pSpriteNodeMan)
        {
            Debug.Assert(pNode != null);
            this.pSpriteBase = pNode;

            // Set the back pointer
            // Allows easier deletion in the future
            Debug.Assert(pSpriteBase != null);
            this.pSpriteBase.SetSpriteNode(this);

            Debug.Assert(_pSpriteNodeMan != null);
            this.pBackSpriteNodeMan = _pSpriteNodeMan;
        }

        public SpriteBase GetSpriteBase()
        {
            return this.pSpriteBase;
        }

        public SpriteNodeMan GetSBNodeMan()
        {
            Debug.Assert(this.pBackSpriteNodeMan != null);
            return this.pBackSpriteNodeMan;
        }
        public SpriteBatch GetSpriteBatch()
        {
            Debug.Assert(this.pBackSpriteNodeMan != null);
            return this.pBackSpriteNodeMan.GetSpriteBatch();
        }

        public void Wash()
        {
            this.pSpriteBase = null;
        }

        // Data: ----------------------------------------------
        public SpriteBase pSpriteBase;
        // Keenan(delete.C)
        private SpriteNodeMan pBackSpriteNodeMan;
    }
}
