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
    //  * Create one - NULL Object - Sprite Default
    //  * Dependency - ImageMan needs to be initialized before SpriteMan
    //
    //---------------------------------------------------------------------------------------------------------
    abstract public class ProxySpriteMan_MLink : Manager
    {
        public ProxySprite_Base poActive;
        public ProxySprite_Base poReserve;
    }
    public class ProxySpriteMan : ProxySpriteMan_MLink
    {
        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        public ProxySpriteMan(int reserveNum = 50, int reserveGrow = 1)
        : base() // <--- Kick the can (delegate)
        {
            // At this point ImageMan is created, now initialize the reserve
            this.BaseInitialize(reserveNum, reserveGrow);

            // initialize derived data here
            //this.poNodeCompare = new ProxySprite();
        }

        private ProxySpriteMan()
        : base() // <--- Kick the can (delegate)
        {
            ProxySpriteMan.pActivePSMan = null;
            // initialize derived data here
            ProxySpriteMan.poNodeCompare = new ProxySprite();
        }

        ~ProxySpriteMan()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~GameSpriteMan():{0}", this.GetHashCode());
#endif
            ProxySpriteMan.poNodeCompare = null;
            ProxySpriteMan.pInstance = null;
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
        //        pInstance = new ProxySpriteMan(reserveNum, reserveGrow);
        //    }
        //}

        public static void Create()
        {
            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new ProxySpriteMan();
            }
        }

        public static void Destroy()
        {

        }

        public static ProxySprite Add(GameSprite.Name name)
        {
            //ProxySpriteMan pMan = ProxySpriteMan.PrivGetInstance();
            ProxySpriteMan pMan = ProxySpriteMan.pActivePSMan;
            Debug.Assert(pMan != null);

            ProxySprite pNode = (ProxySprite)pMan.baseAdd();
            Debug.Assert(pNode != null);

            pNode.Set(name);

            return pNode;
        }

        public static ProxySprite Add(GameSprite.Name nameGS, SpriteBatch.Name nameSB)
        {
            ProxySprite pSprite = ProxySpriteMan.Add(nameGS);
            SpriteBatchMan.Find(nameSB).Attach(pSprite);
            return pSprite;
        }

        public static void Show(ProxySprite pSprite, SpriteBatch pSB)
        {
            Debug.Assert(pSprite != null);
            pSB.Attach(pSprite);
        }

        public static void Hide(ProxySprite pSprite)
        {
            SpriteNode pSpriteNode = pSprite.GetSpriteNode();
            if (pSpriteNode != null)
            {
                SpriteBatchMan.Remove(pSprite.GetSpriteNode());
            }
        }

        public static void SetActive(ProxySpriteMan pPSMan)
        {
            ProxySpriteMan pMan = ProxySpriteMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pPSMan != null);
            ProxySpriteMan.pActivePSMan = pPSMan;
        }

        public static ProxySprite Find(ProxySprite.Name name)
        {
            //ProxySpriteMan pMan = ProxySpriteMan.PrivGetInstance();
            ProxySpriteMan pMan = ProxySpriteMan.pActivePSMan;
            Debug.Assert(pMan != null);

            // Compare functions only compares two Nodes

            // So:  Use the Compare Node - as a reference
            //      use in the Compare() function
            ProxySpriteMan.poNodeCompare.SetName(name);

            ProxySprite pData = (ProxySprite)pMan.baseFind(ProxySpriteMan.poNodeCompare);
            return pData;
        }

        public static void Remove(ProxySprite pSprite)
        {
            // ProxySpriteMan pMan = ProxySpriteMan.PrivGetInstance();
            ProxySpriteMan pMan = ProxySpriteMan.pActivePSMan;
            Debug.Assert(pMan != null);

            Debug.Assert(pSprite != null);
            pMan.baseRemove(pSprite);
        }
        public static void Dump()
        {
            ProxySpriteMan pMan = ProxySpriteMan.privGetInstance();
            Debug.Assert(pMan != null);

            pMan.baseDump();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected DLink derivedCreateNode()
        {
            DLink pNode = new ProxySprite();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected Boolean derivedCompare(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            ProxySprite pDataA = (ProxySprite)pLinkA;
            ProxySprite pDataB = (ProxySprite)pLinkB;

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
            ProxySprite pNode = (ProxySprite)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            ProxySprite pData = (ProxySprite)pLink;
            pData.Dump();
        }

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
        private static ProxySpriteMan privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // Data - unique data for this manager 
        //----------------------------------------------------------------------
        private static ProxySpriteMan pInstance = null;
        private static ProxySpriteMan pActivePSMan = null;
        private static ProxySprite poNodeCompare;
    }
}
