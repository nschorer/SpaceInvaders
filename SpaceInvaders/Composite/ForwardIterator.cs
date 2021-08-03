using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ForwardIterator : Iterator
    {
        public ForwardIterator(Component pStart)
        {
            Debug.Assert(pStart != null);
            Debug.Assert(pStart.holder == Component.Container.COMPOSITE);

            this.pCurr = pStart;
            this.pRoot = pStart;
        }

        private Component PrivNextStep(Component pNode, Component pParent, Component pChild, Component pSibling)
        {
            if (pChild != null)
            {
                pNode = pChild;
            }
            else
            {
                if (pSibling != null)
                {
                    pNode = pSibling;
                }
                else
                {
                    // No more 
                    //       siblings... 
                    //       children...
                    // Go up a level to the parent

                    if (pParent == null || pParent == this.pRoot)
                    {
                        pNode = null;
                        return pNode;
                    }

                    // We should stop looking once we get back to the root, even if it has siblings
                    while (pParent != null)
                    {
                        pNode = GetSibling(pParent);
                        if (pNode != null)
                        {
                            // Found one
                            break;
                        }
                        else
                        {
                            // Go fish
                            pParent = GetParent(pParent);
                        }
                    }
                    
                }
            }

            return pNode;
        }

        override public Component First()
        {
            Debug.Assert(this.pRoot != null);
            Component pNode = this.pRoot;

            Debug.Assert(pNode != null);
            this.pCurr = pNode;

            // Debug.WriteLine("---> {0} ", this.pCurr.GetHashCode());
            return this.pCurr;
        }
        override public Component Next()
        {
            Debug.Assert(this.pCurr != null);

            Component pNode = this.pCurr;

            Component pChild = GetChild(pNode);
            Component pSibling = GetSibling(pNode);
            Component pParent = GetParent(pNode);

            // Start - Depth first iteration
            pNode = PrivNextStep(pNode, pParent, pChild, pSibling);

            this.pCurr = pNode;

            //if (this.pCurr != null)
            //{
            //    Debug.WriteLine("---> {0}", this.pCurr.GetHashCode());
            //}
            //else
            //{
            //    Debug.WriteLine("---> null");
            //}

            return this.pCurr;
        }

        override public bool IsDone()
        {
            return (this.pCurr == null);
        }

        private Component pCurr;
        private Component pRoot;

    }
}
