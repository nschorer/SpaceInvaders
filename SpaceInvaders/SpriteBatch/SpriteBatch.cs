using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    abstract public class SpriteBatch_Link : DLink
    {

    }

    public class SpriteBatch : SpriteBatch_Link
    {
        public enum Name
        {
            PacMan,
            AngryBirds,
            Aliens,
            Boxes,
            Stitch,
            Enemies,
            Spaceship,
            UI,
            Shields,
            Explosions,
            

            Uninitialized
        }

        public SpriteBatch()
            : base()
        {
            this.name = SpriteBatch.Name.Uninitialized;
            this.poSpriteNodeMan = new SpriteNodeMan();
            Debug.Assert(this.poSpriteNodeMan != null);
        }

        public void Set(SpriteBatch.Name name, int reserveNum = 3, int reserveGrow = 1)
        {
            this.name = name;
            this.poSpriteNodeMan.Set(name, reserveNum, reserveGrow);
        }

        public void Attach(SpriteBase pNode)
        {
            Debug.Assert(pNode != null);

            // Go to Man, get a node from reserve, add to active, return it
            SpriteNode pSpriteNode = (SpriteNode)this.poSpriteNodeMan.Attach(pNode);
            Debug.Assert(pSpriteNode != null);

            // Initialize SpriteBatchNode
            pSpriteNode.Set(pNode, this.poSpriteNodeMan);

            this.poSpriteNodeMan.SetSpriteBatch(this);
        }

        public void Wash()
        {
        }

        public void Dump()
        {
        }

        public void SetName(SpriteBatch.Name inName)
        {
            this.name = inName;
        }

        public SpriteBatch.Name GetName()
        {
            return this.name;
        }

        public SpriteNodeMan GetSBNodeMan()
        {
            return this.poSpriteNodeMan;
        }

        public void ToggleRender()
        {
            shouldRender = !shouldRender;
        }

        public void SetRender(bool tf)
        {
            shouldRender = tf;
        }

        public bool ShouldRender()
        {
            return shouldRender;
        }

        // Data -------------------------------
        public SpriteBatch.Name name;
        private readonly SpriteNodeMan poSpriteNodeMan;
        private bool shouldRender = true;
    }
}

// End of File
