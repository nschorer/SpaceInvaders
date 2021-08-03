
using System.Diagnostics;

namespace SpaceInvaders
{
    class DelayedObjectMan
    {
        static public void Attach(ColObserver observer)
        {
            // protection
            Debug.Assert(observer != null);

            DelayedObjectMan pDelayMan = DelayedObjectMan.PrivGetInstance();

            // add to front
            if (pDelayMan.head == null)
            {
                pDelayMan.head = observer;
                observer.pNext = null;
                observer.pPrev = null;
            }
            else
            {
                observer.pNext = pDelayMan.head;
                observer.pPrev = null;
                pDelayMan.head.pPrev = observer;
                pDelayMan.head = observer;
            }
        }
        private void PrivDetach(ColObserver node, ref ColObserver head)
        {
            // protection
            Debug.Assert(node != null);

            if (node.pPrev != null)
            {	// middle or last node
                node.pPrev.pNext = node.pNext;
            }
            else
            {  // first
                head = (ColObserver)node.pNext;
            }

            if (node.pNext != null)
            {	// middle node
                node.pNext.pPrev = node.pPrev;
            }
        }
        static public void Process()
        {
            DelayedObjectMan pDelayMan = DelayedObjectMan.PrivGetInstance();

            ColObserver pNode = pDelayMan.head;

            while (pNode != null)
            {
                // Fire off listener
                pNode.Execute();

                pNode = (ColObserver)pNode.pNext;
            }


            // remove
            pNode = pDelayMan.head;
            ColObserver pTmp = null;

            while (pNode != null)
            {
                pTmp = pNode;
                pNode = (ColObserver)pNode.pNext;

                // remove
                pDelayMan.PrivDetach(pTmp, ref pDelayMan.head);
            }
        }
        private DelayedObjectMan()
        {
            this.head = null;
        }
        private static DelayedObjectMan PrivGetInstance()
        {
            // Do the initialization
            if (instance == null)
            {
                instance = new DelayedObjectMan();
            }

            // Safety - this forces users to call create first
            Debug.Assert(instance != null);

            return instance;
        }

        // -------------------------------------------
        // Data: 
        // -------------------------------------------

        private ColObserver head;
        private static DelayedObjectMan instance = null;
    }
}

// End of File
