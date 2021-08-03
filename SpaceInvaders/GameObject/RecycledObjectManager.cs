using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    // Objects ADDED to this manager are game objects that were removed.
    // Each ACTIVE node is an initialized GameObjectNode that was deleted from the game.
    // Each RESERVE node is an active node that was recycled and now it's GameObjectNode is empty.
    class RecycledObjectManager : Manager
    {
        public RecycledObjectManager(int reserveNum = 3, int reserveGrow = 1)
        : base() // <--- Kick the can (delegate)
        {
            this.BaseInitialize(reserveNum, reserveGrow);
            RecycledObjectManager.poNodeCompare = new GameObjectNode();
            RecycledObjectManager.poNullGameObject = new NullGameObject();

            RecycledObjectManager.poNodeCompare.pGameObj = RecycledObjectManager.poNullGameObject;
        }

        // Store our used game object in our manager
        public void Store(GameObject pObj)
        {
            Debug.Assert(pObj != null);

            GameObjectNode pNode = (GameObjectNode)this.baseAdd();
            Debug.Assert(pNode != null);

            pNode.Set(pObj);
        }

        // Recycle the game object and send the container (GameObjectNode) to the reserve list
        public GameObject Reuse(GameObject.Name name)
        {
            // Compare functions only compares two Nodes
            RecycledObjectManager.poNodeCompare.pGameObj.name = name;

            GameObjectNode pNode = (GameObjectNode)this.baseFind(RecycledObjectManager.poNodeCompare);
            GameObject pObj = null;

            // Recycle the game object if we have it
            if (pNode != null)
            {
                pObj = pNode.pGameObj;
                Debug.Assert(pObj != null);
                this.baseRemove(pNode);
            }

            return pObj;
        }

        override protected DLink derivedCreateNode()
        {
            DLink pNode = new GameObjectNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected Boolean derivedCompare(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            GameObjectNode pDataA = (GameObjectNode)pLinkA;
            GameObjectNode pDataB = (GameObjectNode)pLinkB;

            Boolean status = false;

            if (pDataA.pGameObj.GetName() == pDataB.pGameObj.GetName())
            {
                status = true;
            }

            return status;
        }
        override protected void derivedWash(DLink pLink)
        {
            Debug.Assert(pLink != null);
            GameObjectNode pNode = (GameObjectNode)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            GameObjectNode pData = (GameObjectNode)pLink;
            pData.Dump();
        }

        private static GameObjectNode poNodeCompare;
        private static NullGameObject poNullGameObject;
    }
}
