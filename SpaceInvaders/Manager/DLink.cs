
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class DLink
    {

        protected DLink()
        {
            this.Clear();
        }

        public void Clear()
        {
            this.pNext = null;
            this.pPrev = null;
            this.mSortKey = 0; // Add to front
        }

        public static void AddToFront(ref DLink pHead, DLink pNode)
        {
            // Will work for Active or Reserve List

            // Design Check:
            //     make sure both pNext and pPrev are written once

            // add to front
            Debug.Assert(pNode != null);

            // add node
            if (pHead == null)
            {
                // push to the front
                pHead = pNode;
                pNode.pNext = null;
                pNode.pPrev = null;
            }
            else
            {
                // push to front
                pNode.pPrev = null;
                pNode.pNext = pHead;

                // update head
                pHead.pPrev = pNode;
                pHead = pNode;
            }

            // worst case, pHead was null initially, now we added a node so... this is true
            Debug.Assert(pHead != null);
        }

        public static void AddToEnd(ref DLink pHead, ref DLink pEnd, DLink pNode)
        {
            // Will work for Active or Reserve List

            // add to front
            Debug.Assert(pNode != null);

            // add node
            if (pEnd == null)
            {
                // no nodes on list
                pHead = pNode;
                pEnd = pNode;
                pNode.pNext = null;
                pNode.pPrev = null;
            }
            else
            {
                // add to end
                pEnd.pNext = pNode;
                pNode.pPrev = pEnd;
                pNode.pNext = null;
                pEnd = pNode;

                // update front
                // Adding to end --> front doesn't change
            }

            // worst case, pHead,pEnd was null initially, now we added a node so... this is true
            Debug.Assert(pHead != null);
            Debug.Assert(pEnd != null);
        }

        public static void AddSorted(ref DLink pHead, DLink pNode, float SortKey)
        {
            // Should only be called for Active (why sort the reserve list?)

            // Design Check:
            //     make sure both pNext and pPrev are written once

            // add to front
            Debug.Assert(pNode != null);
            Debug.Assert(SortKey >= 0); // 0 is always push to front

            pNode.mSortKey = SortKey;

            // add node
            if (pHead == null)
            {
                // push to the front
                pHead = pNode;
                pNode.pNext = null;
                pNode.pPrev = null;
            }
            else
            {

                DLink pNext = pHead;
                DLink pPrev = pHead.pPrev;
                // Handles when we add node to front or in the middle
                while (pNext != null)
                {
                    // We found where to put it
                    if (pNode.mSortKey <= pNext.mSortKey) // If tie in priority, newcomer goes first in front
                    {
                        pNode.pPrev = pPrev;
                        pNode.pNext = pNext; // Remember, this is not null!

                        if (pPrev != null) pPrev.pNext = pNode;
                        else pHead = pNode;

                        pNext.pPrev = pNode;

                        pNext = null; // end the loop
                    }

                    // Otherwise, iterate the loop
                    else
                    {
                        pPrev = pNext;
                        pNext = pNext.pNext;
                    }
                }

                // Handle when we add node to the end
                if (pNode.pNext == null)
                {
                    pNode.pPrev = pPrev;
                    pPrev.pNext = pNode;
                }
            }

            // worst case, pHead was null initially, now we added a node so... this is true
            Debug.Assert(pHead != null);
        }

        public static DLink PullFromFront(ref DLink pHead)
        {
            // There should always be something on list
            Debug.Assert(pHead != null);

            // return node
            DLink pNode = pHead;

            // Update head (OK if it points to NULL)
            pHead = pHead.pNext;
            if (pHead != null)
            {
                pHead.pPrev = null;
                // do not change pEnd
            }
            else
            {
                // only one on the list
                // pHead == null
            }

            // remove any lingering links
            // HUGELY important - otherwise its crossed linked 
            pNode.Clear();

            return pNode;
        }
        public static void RemoveNode(ref DLink pHead, DLink pNode)
        {
            // protection
            Debug.Assert(pHead != null);
            Debug.Assert(pNode != null);

            // Quick HACK... might be a bug... need to diagram

            // 4 different conditions... 
            if (pNode.pPrev != null)
            {	// middle part 1/2
                pNode.pPrev.pNext = pNode.pNext;
            }
            else
            {  // first
                pHead = pNode.pNext;
            }

            if (pNode.pNext != null)
            {	// middle node part 2/2
                pNode.pNext.pPrev = pNode.pPrev;
            }

            // remove any lingering links
            // HUGELY important - otherwise its crossed linked 
            pNode.Clear();
        }

        public static void RemoveNode(ref DLink pHead, ref DLink pEnd, DLink pNode)
        {
            // protection
            Debug.Assert(pHead != null);
            Debug.Assert(pEnd != null);
            Debug.Assert(pNode != null);

            // Quick HACK... might be a bug... need to diagram

            // 4 different conditions... 
            if (pNode.pPrev != null)
            {	// middle part 1/2
                pNode.pPrev.pNext = pNode.pNext;

                // last node
                if (pNode == pEnd)
                {
                    pEnd = pNode.pPrev;
                }
            }
            else
            {  // first
                pHead = pNode.pNext;

                if (pNode == pEnd)
                {
                    // Only one node
                    pEnd = pNode.pNext;
                }
                else
                {
                    // Only first not the last
                    // do nothing more
                }
            }

            if (pNode.pNext != null)
            {	// middle node part 2/2
                pNode.pNext.pPrev = pNode.pPrev;
            }

            // remove any lingering links
            // HUGELY important - otherwise its crossed linked 
            pNode.Clear();
        }

        // Data: -----------------------------
        public DLink pNext;
        public DLink pPrev;
        private float mSortKey;

    }
}
