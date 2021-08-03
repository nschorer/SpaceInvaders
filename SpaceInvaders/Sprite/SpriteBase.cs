
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class SpriteBase : DLink
    {
        // Create a single sprite and all dynamic objects ONCE and ONLY ONCE (OOO- tm)
        public SpriteBase()
            : base()
        {
            this.pBackSpriteNode = null;
        }

        abstract public void Update();
        abstract public void Render();

        public SpriteNode GetSpriteNode()
        {
            //Debug.Assert(this.pBackSpriteNode != null);
            return this.pBackSpriteNode;
        }
        public void SetSpriteNode(SpriteNode pSpriteBatchNode)
        {
            Debug.Assert(pSpriteBatchNode != null);
            this.pBackSpriteNode = pSpriteBatchNode;
        }

        public bool HasSpriteNode()
        {
            return this.pBackSpriteNode != null;
        }

        // Data: -------------------------------------------

        // Keenan(delete.B)
        // If you remove a SpriteBase initiated by gameObject... 
        //     its hard to get the spriteBatchNode
        //     so have a back pointer to it
        private SpriteNode pBackSpriteNode;

    }
}

// End of File
