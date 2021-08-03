using System;
using System.Diagnostics;

namespace SpaceInvaders
{

    abstract public class SpriteBatchMan_Link : Manager
    {
        public SpriteBatch_Link poActive;
        public SpriteBatch_Link poReserve;
    }

    public class SpriteBatchMan : SpriteBatchMan_Link
    {

        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        public  SpriteBatchMan(int reserveNum = 3, int reserveGrow = 1)
        : base() // <--- Kick the can (delegate)
        {
            // At this point SpriteBatchMan is created, now initialize the reserve
            this.BaseInitialize(reserveNum, reserveGrow);

            // initialize derived data here
            //this.poNodeCompare = new SpriteBatch();
        }

        private SpriteBatchMan()
        : base() // <--- Kick the can (delegate)
        {
            SpriteBatchMan.pActiveSBMan = null;
            // initialize derived data here
            SpriteBatchMan.poNodeCompare = new SpriteBatch();
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
        //        pInstance = new SpriteBatchMan(reserveNum, reserveGrow);
        //    }
        //}

        public static void Create()
        {
            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new SpriteBatchMan();
            }
        }

        public static void Destroy()
        {

            // Do something clever here
            // track peak number of active nodes
            // print stats on destroy
            // invalidate the singleton

        }

        public static SpriteBatch Add(SpriteBatch.Name name, int priority, int reserveNum = 3, int reserveGrow = 1)
        {
            SpriteBatchMan pMan = SpriteBatchMan.pActiveSBMan;
            Debug.Assert(pMan != null);

            SpriteBatch pNode = (SpriteBatch)pMan.baseSortedAdd(priority);
            Debug.Assert(pNode != null);

            pNode.Set(name, reserveNum, reserveGrow);
            return pNode;
        }

        public static void SetActive(SpriteBatchMan pSBMan)
        {
            SpriteBatchMan pMan = SpriteBatchMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pSBMan != null);
            SpriteBatchMan.pActiveSBMan = pSBMan;
        }

        public static void Draw()
        {
            SpriteBatchMan pMan = SpriteBatchMan.pActiveSBMan;
            Debug.Assert(pMan != null);

            // walk through the list and render
            SpriteBatch pSpriteBatch = (SpriteBatch)pMan.baseGetActive();

            while (pSpriteBatch != null)
            {
                // Each sprite batch has a switch which says if it should render
                if (pSpriteBatch.ShouldRender())
                {
                    SpriteNodeMan pSBNodeMan = pSpriteBatch.GetSBNodeMan();
                    Debug.Assert(pSBNodeMan != null);

                    // Kick the can
                    pSBNodeMan.Draw();
                }

                pSpriteBatch = (SpriteBatch)pSpriteBatch.pNext;
            }

        }

        public static SpriteBatch Find(SpriteBatch.Name name)
        {
            //SpriteBatchMan pMan = SpriteBatchMan.privGetInstance();
            SpriteBatchMan pMan = SpriteBatchMan.pActiveSBMan;
            Debug.Assert(pMan != null);

            // Compare functions only compares two Nodes

            // So:  Use the Compare Node - as a reference
            //      use in the Compare() function
            SpriteBatchMan.poNodeCompare.name = name;

            SpriteBatch pData = (SpriteBatch)pMan.baseFind(SpriteBatchMan.poNodeCompare);
            return pData;
        }
        public static void Remove(SpriteBatch pNode)
        {
            SpriteBatchMan pMan = SpriteBatchMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemove(pNode);
        }

        public static void Remove(SpriteNode pSpriteBatchNode)
        {
            Debug.Assert(pSpriteBatchNode != null);
            SpriteNodeMan pSpriteNodeMan = pSpriteBatchNode.GetSBNodeMan();

            Debug.Assert(pSpriteNodeMan != null);
            pSpriteNodeMan.Remove(pSpriteBatchNode);
        }

        public static void Dump()
        {
            SpriteBatchMan pMan = SpriteBatchMan.privGetInstance();
            Debug.Assert(pMan != null);

            pMan.baseDump();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected DLink derivedCreateNode()
        {
            DLink pNode = new SpriteBatch();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected Boolean derivedCompare(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            SpriteBatch pDataA = (SpriteBatch)pLinkA;
            SpriteBatch pDataB = (SpriteBatch)pLinkB;

            Boolean status = false;
            // stubbed out

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        override protected void derivedWash(DLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBatch pNode = (SpriteBatch)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBatch pData = (SpriteBatch)pLink;
            pData.Dump();
        }

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
        private static SpriteBatchMan privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        public static void SetCollisionSB(SpriteBatch.Name sbName)
        {
            //SpriteBatchMan pMan = privGetInstance();
            SpriteBatchMan pMan = pActiveSBMan;
            SpriteBatch sb = SpriteBatchMan.Find(sbName);
            pMan.pSB_Collision = sb;
            sb.SetRender(false);
        }

        public static SpriteBatch GetCollisionSB()
        {
            //SpriteBatchMan pMan = privGetInstance();
            SpriteBatchMan pMan = pActiveSBMan;
            return pMan.pSB_Collision;
        }

        //----------------------------------------------------------------------
        // Data - unique data for this manager 
        //----------------------------------------------------------------------
        private static SpriteBatchMan pInstance = null;
        private static SpriteBatchMan pActiveSBMan = null;
        private static SpriteBatch poNodeCompare;
        private SpriteBatch pSB_Collision;
    }
}

// End of File
