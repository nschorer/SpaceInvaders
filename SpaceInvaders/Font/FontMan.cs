using System;
using System.Xml;
using System.Diagnostics;

namespace SpaceInvaders
{
    abstract public class FontMan_MLink : Manager
    {
        public Font_DLink poActive;
        public Font_DLink poReserve;
    }
    public class FontMan : FontMan_MLink
    {
        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        public FontMan(int reserveNum = 3, int reserveGrow = 1)
            : base()
        {
            this.BaseInitialize(reserveNum, reserveGrow);
            //FontMan.poNodeCompare = (Font)this.derivedCreateNode();
        }

        private FontMan()
            : base()
        {
            FontMan.pActiveFMan = null;
            FontMan.poNodeCompare = new Font();
        }

        ~FontMan()
        {

        }

        //----------------------------------------------------------------------
        // Static Manager methods can be implemented with base methods 
        // Can implement/specialize more or less methods your choice
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
        //        pInstance = new FontMan(reserveNum, reserveGrow);
        //    }
        //}

        public static void Create()
        {
            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new FontMan();
            }
        }

        public static void Destroy()
        {

        }
        public static Font Add(Font.Name name, SpriteBatch.Name SB_Name, String pMessage, Glyph.Name glyphName, float xStart, float yStart, bool addToSB = true)
        {
            //FontMan pMan = FontMan.privGetInstance();
            FontMan pMan = FontMan.pActiveFMan;
            Debug.Assert(pMan != null);

            Font pNode = (Font)pMan.baseAdd();
            Debug.Assert(pNode != null);

            pNode.Set(name, pMessage, glyphName, xStart, yStart);

            if (addToSB)
            {
                // Add to sprite batch
                SpriteBatch pSB = SpriteBatchMan.Find(SB_Name);
                Debug.Assert(pSB != null);
                Debug.Assert(pNode.pFontSprite != null);
                pSB.Attach(pNode.pFontSprite);
            }

            return pNode;
        }

        public static void AddToSB(Font pFont, SpriteBatch pSB)
        {
            // Add to sprite batch
            Debug.Assert(pSB != null);
            Debug.Assert(pFont != null);
            pSB.Attach(pFont.pFontSprite);
        }

        public static void RemoveFromSB(Font pFont, SpriteBatch pSB)
        {
            Debug.Assert(pSB != null);
            Debug.Assert(pFont != null);

            // hack hack hack
            if (pFont.pFontSprite.HasSpriteNode())
            {
                SpriteBatchMan.Remove(pFont.pFontSprite.GetSpriteNode());
            }
        }

        public static void SetActive(FontMan pFMan)
        {
            FontMan pMan = FontMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pFMan != null);
            FontMan.pActiveFMan = pFMan;
        }

        public static void AddXml(Glyph.Name glyphName, String assetName,Texture.Name textName)
        {
            GlyphMan.AddXml(glyphName, assetName, textName);
        }

        public static void Remove(Glyph pNode)
        {
            Debug.Assert(pNode != null);
            //FontMan pMan = FontMan.privGetInstance();
            FontMan pMan = FontMan.pActiveFMan;
            pMan.baseRemove(pNode);
        }
        public static Font Find(Font.Name name)
        {
            //FontMan pMan = FontMan.privGetInstance();
            FontMan pMan = FontMan.pActiveFMan;
            Debug.Assert(pMan != null);

            // Compare functions only compares two Nodes
            FontMan.poNodeCompare.name = name;

            Font pData = (Font)pMan.baseFind(FontMan.poNodeCompare);
            return pData;
        }


        public static void Dump()
        {
            FontMan pMan = FontMan.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Font Manager ------");
            pMan.baseDump();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected Boolean derivedCompare(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            Font pDataA = (Font)pLinkA;
            Font pDataB = (Font)pLinkB;

            Boolean status = false;

            if (pDataA.name == pDataB.name )
            {
                status = true;
            }

            return status;
        }
        override protected DLink derivedCreateNode()
        {
            DLink pNode = new Font();
            Debug.Assert(pNode != null);
            return pNode;
        }
        override protected void derivedWash(DLink pLink)
        {
            Debug.Assert(pLink != null);
            Font pNode = (Font)pLink;
            pNode.Wash();
        }

        override protected void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            Font pNode = (Font)pLink;

            Debug.Assert(pNode != null);
            pNode.Dump();
        }

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
        private static FontMan privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // Data
        //----------------------------------------------------------------------
        private static FontMan pInstance = null;
        private static FontMan pActiveFMan = null;
        private static Font poNodeCompare = null;
        //private Font pRefNode;
    }
}
