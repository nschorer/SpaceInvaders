using System;
using System.Diagnostics;

namespace SpaceInvaders
{

    //---------------------------------------------------------------------------------------------------------
    // Design Notes:
    //
    //  Singleton class - use only public static methods for customers
    //
    //  * One single compare node is owned by this singleton - used for find, prevent extra news
    //  * Create one - NULL Object - Image Default
    //  * Dependency - TextureMan needs to be initialized before ImageMan
    //
    //---------------------------------------------------------------------------------------------------------
    public class TimerMan : Manager
    {
        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        public TimerMan(int reserveNum = 3, int reserveGrow = 1)
        : base() // <--- Kick the can (delegate)
        {
            // At this point TimerMan is created, now initialize the reserve
            this.BaseInitialize(reserveNum, reserveGrow);

            // initialize derived data here
            //TimerMan.poNodeCompare = new TimeEvent();
        }

        private TimerMan()
        {
            TimerMan.pActiveTMan = null;
            TimerMan.poNodeCompare = new TimeEvent();
        }

        //----------------------------------------------------------------------
        // Static Methods
        //----------------------------------------------------------------------
        //public static void Create(int reserveNum = 3, int reserveGrow = 1)
        //{
        //    // make sure values are ressonable 
        //    Debug.Assert(reserveNum > 0);
        //    Debug.Assert(reserveGrow > 0);

        //    // initialize the singleton here
        //    Debug.Assert(pInstance == null);

        //    // Do the initialization
        //    if (pInstance == null)
        //    {
        //        pInstance = new TimerMan(reserveNum, reserveGrow);
        //    }
        //}

        public static void Create()
        {

            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new TimerMan();
            }
        }

        public static TimeEvent Add(TimeEvent.Name timeName, Command pCommand, float deltaTimeToTrigger)
        {
            //TimerMan pMan = TimerMan.PrivGetInstance();
            TimerMan pMan = TimerMan.pActiveTMan;
            Debug.Assert(pMan != null);

            TimeEvent pNode = (TimeEvent)pMan.baseSortedAdd(TimerMan.GetCurrTime() + deltaTimeToTrigger);
            Debug.Assert(pNode != null);

            Debug.Assert(pCommand != null);
            Debug.Assert(deltaTimeToTrigger >= 0.0f);

            pNode.Set(timeName, pCommand, deltaTimeToTrigger);
            return pNode;
        }

        public static void SetActive(TimerMan pTMan)
        {
            TimerMan pMan = TimerMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pTMan != null);
            TimerMan.pActiveTMan = pTMan;

            TimerMan.UnpauseTimerMan();
        }

        public static TimeEvent Find(TimeEvent.Name name)
        {
            //TimerMan pMan = TimerMan.PrivGetInstance();
            TimerMan pMan = TimerMan.pActiveTMan;
            Debug.Assert(pMan != null);

            // Compare functions only compares two Nodes

            // So:  Use the Compare Node - as a reference
            //      use in the Compare() function
            Debug.Assert(TimerMan.poNodeCompare != null);
            TimerMan.poNodeCompare.Wash();
            TimerMan.poNodeCompare.name = name;

            TimeEvent pData = (TimeEvent)pMan.baseFind(TimerMan.poNodeCompare);
            return pData;
        }
        public static void Remove(TimeEvent pNode)
        {
            //TimerMan pMan = TimerMan.PrivGetInstance();
            TimerMan pMan = TimerMan.pActiveTMan;
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemove(pNode);
        }
        public static void Dump()
        {
            //TimerMan pMan = TimerMan.PrivGetInstance();
            TimerMan pMan = TimerMan.pActiveTMan;
            Debug.Assert(pMan != null);

            pMan.baseDump();
        }

        public static void Update(float totalTime)
        {
            // Get the instance
            //TimerMan pMan = TimerMan.PrivGetInstance();
            TimerMan pMan = TimerMan.pActiveTMan;
            Debug.Assert(pMan != null);

            // squirrel away
            pMan.mCurrTime = totalTime;

            // walk the list
            TimeEvent pEvent = (TimeEvent)pMan.baseGetActive();
            TimeEvent pNextEvent = null;

            // Walk the list until there is no more list OR currTime is greater than timeEvent 
            // ToDo Fix: List needs to be sorted
            while (pEvent != null)// && (pMan.mCurrTime >= pEvent.triggerTime))
            {
                // Difficult to walk a list and remove itself from the list
                // so squirrel away the next event now, use it at bottom of while
                pNextEvent = (TimeEvent)pEvent.pNext;

                if (pMan.mCurrTime >= pEvent.triggerTime)
                {
                    // call it
                    pEvent.Process();

                    // remove from list
                    pMan.baseRemove(pEvent);

                    // advance the pointer
                    pEvent = pNextEvent;
                }

                // List is sorted. Once we find first "future" event, the rest will also be in the future.
                else
                {
                    break;
                }
            }
        }
        public static float GetCurrTime()
        {
            // Get the instance
            //TimerMan pTimerMan = TimerMan.PrivGetInstance();
            TimerMan pTimerMan = TimerMan.pActiveTMan;

            // return time
            return pTimerMan.mCurrTime;
        }

        private static void UnpauseTimerMan()
        {
            TimerMan pMan = TimerMan.pActiveTMan;
            float deltaTime = Simulation.GetTotalTime() - pMan.mCurrTime;

            // walk the list
            TimeEvent pEvent = (TimeEvent)pMan.baseGetActive();
            while (pEvent != null)
            {
                // Since we are updating ALL time events, we don't have to worry about changing the order
                pEvent.triggerTime += deltaTime;
                pEvent = (TimeEvent)pEvent.pNext;
            }

            // UPDATE right away. We've unpaused so let's make sure our current time is up to date
            TimerMan.Update(Simulation.GetTotalTime());
        }

        // We should be pretty careful about this...
        // We want to do this inbetween waves or when we switch between select/game over and the play screens...
        // But we DON'T want to do this when switching between players 1 and 2
        public static void ClearTimerEvents()
        {
            //TimerMan pMan = TimerMan.PrivGetInstance();
            TimerMan pMan = TimerMan.pActiveTMan;
            Debug.Assert(pMan != null);

            TimeEvent pEvent = (TimeEvent)pMan.baseGetActive();
            TimeEvent pNextEvent = null;

            // Walk the list and delete all the nodes
            while (pEvent != null)
            {
                pNextEvent = (TimeEvent)pEvent.pNext;
                TimerMan.Remove(pEvent);
                pEvent = pNextEvent;
            }
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected DLink derivedCreateNode()
        {
            DLink pNode = new TimeEvent();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected Boolean derivedCompare(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            TimeEvent pDataA = (TimeEvent)pLinkA;
            TimeEvent pDataB = (TimeEvent)pLinkB;

            Boolean status = false;

            if (pDataA.name == pDataB.name)
            {
                status = true;
            }

            return status;
        }
        override protected void derivedWash(DLink pLink)
        {
            Debug.Assert(pLink != null);
            TimeEvent pNode = (TimeEvent)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            TimeEvent pData = (TimeEvent)pLink;
            pData.Dump();
        }

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
        private static TimerMan privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // Data - unique data for this manager 
        //----------------------------------------------------------------------
        private static TimerMan pInstance = null;
        private static TimerMan pActiveTMan = null;
        private static TimeEvent poNodeCompare;
        protected float mCurrTime = 0.0f; // important default value
    }
}

// End of File
