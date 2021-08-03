
using System.Diagnostics;

namespace SpaceInvaders
{
    abstract class SLink
    {
        protected SLink()
        {
            this.pSNext = null;
        }

        public static void AddToFront(ref SLink pHead, SLink pNode)
        {
            // Will work for Active or Reserve List

            // add to front
            Debug.Assert(pNode != null);

            // add node
            if (pHead == null)
            {
                // push to the front
                pHead = pNode;
                pNode.pSNext = null;
            }
            else
            {
                // push to front
                pNode.pSNext = pHead;
                pHead = pNode;
            }

            // worst case, pHead was null initially, now we added a node so... this is true
            Debug.Assert(pHead != null);
        }

        // Data: ---------------
        public SLink pSNext;
    }
}

// End of File
