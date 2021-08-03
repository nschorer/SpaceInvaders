using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class Composite : GameObject
    {
        public Composite()
        {
            this.holder = Container.COMPOSITE;
            this.poHead = null;
            this.poLast = null;
            //Debug.Write(" creating--> ");
            //this.DumpNode();
        }

        public Composite(GameObject.Name gameName, GameSprite.Name spriteName)
        : base(gameName, spriteName)
        {
            this.holder = Container.COMPOSITE;
            this.poHead = null;
            this.poLast = null;
            //Debug.Write(" creating--> ");
            //this.DumpNode();
        }

        override public void Add(Component pComponent)
        {
            Debug.Assert(pComponent != null);
            DLink.AddToEnd(ref this.poHead, ref this.poLast, pComponent);
            pComponent.pParent = this;
        }

        override public void Remove(Component pComponent)
        {
            Debug.Assert(pComponent != null);
            DLink.RemoveNode(ref this.poHead, ref this.poLast, pComponent);
        }

        override public Component GetFirstChild()
        {
            DLink pNode = this.poHead;

            // Sometimes it returns null... that's ok
            // Scenario - we have a group without a child
            // i.e. composite with no children
            // Debug.Assert(pNode != null);

            return (Component)pNode;
        }

        public virtual bool IsCompositeEmpty()
        {
            return (Iterator.GetChild(this) == null);
        }

        override public void DumpNode()
        {
            if (Iterator.GetParent(this) != null)
            {
                Debug.WriteLine(" GameObject Name:({0}) parent:{1} <---- Composite", this.GetHashCode(), Iterator.GetParent(this).GetHashCode());
            }
            else
            {
                Debug.WriteLine(" GameObject Name:({0}) parent:null <---- Composite", this.GetHashCode());
            }
        }

        public override void Print()
        {
            DLink pNode = this.poHead;

            while (pNode != null)
            {
                Component pComponent = (Component)pNode;
                pComponent.Print();

                pNode = pNode.pNext;
            }

        }

        // Removes all (remaining) children from this composite.
        // Does NOT remove the composite itself
        public virtual void RemoveAllChildren()
        {
            Debug.WriteLine("\n\n");


            ReverseIterator it = new ReverseIterator(this);

            GameObject pObj = (GameObject)it.First();
            //pObj.Dump();

            // Skip the first node (the composite)
            //Debug.Assert(pObj == this);
            //pObj = (GameObject)it.Next();

            while (!it.IsDone())
            {
                // Skip the composite!
                if (pObj != this)
                {
                    //pObj.Dump();
                    pObj.Remove();
                    //pObj.bMarkForDeath = true;
                }
                pObj = (GameObject)it.Next();
            }
        }

        public DLink poHead;
        public DLink poLast;
    }
}
