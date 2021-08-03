using System.Diagnostics;

namespace SpaceInvaders
{
    //---------------------------------------------------------------------------------------------------------
    // Design Notes:
    //
    //  Singleton class - use only public static methods for customers
    //
    //  * One single compare node is owned by this singleton - used for find, prevent extra news
    //  * Create one - NULL Object - BoxSprite Default
    //
    //---------------------------------------------------------------------------------------------------------
    public class BoxSpriteMan : Manager
    {
        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        public BoxSpriteMan(int reserveNum = 3, int reserveGrow = 1)
        : base() // <--- Kick the can (delegate)
        {
            // At this point ImageMan is created, now initialize the reserve
            this.BaseInitialize(reserveNum, reserveGrow);

            // initialize derived data here
            //this.poNodeCompare = new BoxSprite();
        }

        private BoxSpriteMan()
        {
            BoxSpriteMan.pActiveBSMan = null;
            BoxSpriteMan.poNodeCompare = new BoxSprite();
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
        //        pInstance = new BoxSpriteMan(reserveNum, reserveGrow);
        //    }
        //}

        // This is the real (singleton) entry point
        public static void Create()
        { 
            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new BoxSpriteMan();
            }
        }

        public static void Destroy()
        {
            //BoxSpriteMan pMan = BoxSpriteMan.privGetInstance();
            BoxSpriteMan pMan = BoxSpriteMan.pActiveBSMan;
            Debug.Assert(pMan != null);

            // Do something clever here
            // track peak number of active nodes
            // print stats on destroy
            // invalidate the singleton
        }
        public static BoxSprite Add(BoxSprite.Name name, float x, float y, float width, float height, Azul.Color pColor = null)
        {
            //BoxSpriteMan pMan = BoxSpriteMan.privGetInstance();
            BoxSpriteMan pMan = BoxSpriteMan.pActiveBSMan;
            Debug.Assert(pMan != null);

            BoxSprite pNode = (BoxSprite)pMan.baseAdd();
            Debug.Assert(pNode != null);

            pNode.Set(name, x, y, width, height, pColor);

            return pNode;
        }

        public static void SetActive(BoxSpriteMan pBSMan)
        {
            BoxSpriteMan pMan = BoxSpriteMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pBSMan != null);
            BoxSpriteMan.pActiveBSMan = pBSMan;
        }

        public static BoxSprite Find(BoxSprite.Name name)
        {
            //BoxSpriteMan pMan = BoxSpriteMan.privGetInstance();
            BoxSpriteMan pMan = BoxSpriteMan.pActiveBSMan;

            // Compare functions only compares two Nodes

            // So:  Use the Compare Node - as a reference
            //      use in the Compare() function
            BoxSpriteMan.poNodeCompare.SetName(name);

            BoxSprite pData = (BoxSprite)pMan.baseFind(BoxSpriteMan.poNodeCompare);
            return pData;
        }
        public static void Remove(BoxSprite pNode)
        {
            //BoxSpriteMan pMan = BoxSpriteMan.privGetInstance();
            BoxSpriteMan pMan = BoxSpriteMan.pActiveBSMan;
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemove(pNode);
        }
        public static void Dump()
        {
            BoxSpriteMan pMan = BoxSpriteMan.privGetInstance();
            Debug.Assert(pMan != null);

            pMan.baseDump();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected DLink derivedCreateNode()
        {
            DLink pNode = new BoxSprite();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected bool derivedCompare(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            BoxSprite pDataA = (BoxSprite)pLinkA;
            BoxSprite pDataB = (BoxSprite)pLinkB;

            bool status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        override protected void derivedWash(DLink pLink)
        {
            Debug.Assert(pLink != null);
            BoxSprite pNode = (BoxSprite)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            BoxSprite pData = (BoxSprite)pLink;
            pData.Dump();
        }

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
        private static BoxSpriteMan privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // Data - unique data for this manager 
        //----------------------------------------------------------------------
        private static BoxSpriteMan pInstance = null;
        private static BoxSpriteMan pActiveBSMan = null;
        private static BoxSprite poNodeCompare;
    }
}
