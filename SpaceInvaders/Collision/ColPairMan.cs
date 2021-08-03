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

    abstract public class ColPairMan_MLink : Manager
    {
        public ColPair_Link poActive;
        public ColPair_Link poReserve;
    }
    public class ColPairMan : ColPairMan_MLink
    {
        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        private ColPairMan(int reserveNum = 3, int reserveGrow = 1)
        : base() // <--- Kick the can (delegate)
        {
            // At this point ImageMan is created, now initialize the reserve
            this.BaseInitialize(reserveNum, reserveGrow);

            // no link... used in Process
            this.pActiveColPair = null;

            // initialize derived data here
            this.poNodeCompare = new ColPair();
        }

        ~ColPairMan()
        {

        }

        //----------------------------------------------------------------------
        // Static Methods
        //----------------------------------------------------------------------
        public static void Create(int reserveNum = 3, int reserveGrow = 1)
        {
            // make sure values are ressonable 
            Debug.Assert(reserveNum > 0);
            Debug.Assert(reserveGrow > 0);

            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new ColPairMan(reserveNum, reserveGrow);
            }

        }



        public static void Destroy()
        {
            // Get the instance

        }

        public static ColPair Add(ColPair.Name colpairName, GameObject treeRootA, GameObject treeRootB)
        {
            // Get the instance
            ColPairMan pMan = ColPairMan.privGetInstance();
            Debug.Assert(pMan != null);

            // Go to Man, get a node from reserve, add to active, return it
            ColPair pColPair = (ColPair)pMan.baseAdd();
            Debug.Assert(pColPair != null);

            // Initialize Image
            pColPair.Set(colpairName, treeRootA, treeRootB);

            return pColPair;
        }

        public static void Process()
        {
            // get the singleton
            ColPairMan pColPairMan = ColPairMan.privGetInstance();

            ColPair pColPair = (ColPair)pColPairMan.baseGetActive();

            while (pColPair != null)
            {
                // set the current active  <--- Key concept: set this before
                pColPairMan.pActiveColPair = pColPair;

                // do the check for a single pair
                pColPair.Process();

                // advance to next
                pColPair = (ColPair)pColPair.pNext;
            }
        }

        public static ColPair Find(ColPair.Name name)
        {
            ColPairMan pMan = ColPairMan.privGetInstance();
            Debug.Assert(pMan != null);

            // Compare functions only compares two Nodes

            // So:  Use the Compare Node - as a reference
            //      use in the Compare() function
            pMan.poNodeCompare.SetName(name);

            ColPair pData = (ColPair)pMan.baseFind(pMan.poNodeCompare);
            return pData;
        }
        public static void Remove(ColPair pNode)
        {
            ColPairMan pMan = ColPairMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemove(pNode);
        }
        public static void Dump()
        {
            ColPairMan pMan = ColPairMan.privGetInstance();
            Debug.Assert(pMan != null);

            pMan.baseDump();
        }

        static public ColPair GetActiveColPair()
        {
            // get the singleton
            ColPairMan pMan = ColPairMan.privGetInstance();

            return pMan.pActiveColPair;
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected DLink derivedCreateNode()
        {
            DLink pNode = new ColPair();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected Boolean derivedCompare(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            ColPair pDataA = (ColPair)pLinkA;
            ColPair pDataB = (ColPair)pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        override protected void derivedWash(DLink pLink)
        {
            Debug.Assert(pLink != null);
            ColPair pNode = (ColPair)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            ColPair pData = (ColPair)pLink;
            pData.Dump();
        }

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
        private static ColPairMan privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // Data - unique data for this manager 
        //----------------------------------------------------------------------
        private static ColPairMan pInstance = null;
        private ColPair poNodeCompare;
        private ColPair pActiveColPair;
    }
}