using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    class InputSubject
    {
        public void Attach(InputObserver observer)
        {
            // protection
            Debug.Assert(observer != null);

            observer.pSubject = this;

            // add to front
            if (head == null)
            {
                head = observer;
                observer.pNext = null;
                observer.pPrev = null;
            }
            else
            {
                observer.pNext = head;
                observer.pPrev = null;
                head.pPrev = observer;
                head = observer;
            }
        }


        public void Notify()
        {
            InputObserver pNode = this.head;

            while (pNode != null)
            {
                // Fire off listener
                pNode.Notify();

                pNode = (InputObserver)pNode.pNext;
            }
        }

        public void Detach()
        {
        }


        // Data: ------------------------
        private InputObserver head;



    }
}
