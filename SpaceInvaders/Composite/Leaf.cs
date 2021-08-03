using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class Leaf : GameObject
    {
        public Leaf(GameObject.Name gameName, GameSprite.Name spriteName)
               : base(gameName, spriteName)
        {
            this.holder = Container.LEAF;
        }

        public Leaf(GameObject.Name gameName)
            : base(gameName)
        {
            this.holder = Container.LEAF;
        }

        public Leaf()
        {
           
        }
        override public void Add(Component c)
        {
            Debug.Assert(false);
        }

        override public void Remove(Component c)
        {
            Debug.Assert(false);
        }

        override public void Print()
        {
            Debug.WriteLine(" GameObject Name: {0} ({1})", this.GetName(), this.GetHashCode());
        }

        override public Component GetFirstChild()
        {
            Debug.Assert(false);
            return null;
        }
        override public void DumpNode()
        {
            Debug.WriteLine(" GameObject Name: {0} ({1}) parent:{2}", this.GetName(), this.GetHashCode(), Iterator.GetParent(this).GetHashCode());
        }
    }
}
