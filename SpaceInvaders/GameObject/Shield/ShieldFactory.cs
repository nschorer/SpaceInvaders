using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ShieldFactory
    {
        public ShieldFactory(SpriteBatch.Name spriteBatchName, SpriteBatch.Name collisionSpriteBatch)
        {
            this.pSpriteBatch = SpriteBatchMan.Find(spriteBatchName);
            Debug.Assert(this.pSpriteBatch != null);

            this.pCollisionSpriteBatch = SpriteBatchMan.Find(collisionSpriteBatch);
            Debug.Assert(this.pCollisionSpriteBatch != null);
        }
        public void SetParent(GameObject pParentNode)
        {
            // OK being null
            Debug.Assert(pParentNode != null);
            this.pTree = (Composite)pParentNode;
        }
        ~ShieldFactory()
        {
            this.pSpriteBatch = null;
        }
        public GameObject Create(ShieldCategory.Type type, GameObject.Name gameName, float posX = 0.0f, float posY = 0.0f)
        {
            //GameObject pShield = null;
            GameObject pShield = GameObjectMan.ReuseGameObject(gameName);

            if (pShield == null)
            {

                switch (type)
                {
                    case ShieldCategory.Type.Brick:
                        pShield = new ShieldBrick(gameName, GameSprite.Name.Brick, posX, posY);
                        break;

                    case ShieldCategory.Type.LeftTop1:
                        pShield = new ShieldBrick(gameName, GameSprite.Name.Brick_LeftTop1, posX, posY);
                        break;

                    case ShieldCategory.Type.LeftTop0:
                        pShield = new ShieldBrick(gameName, GameSprite.Name.Brick_LeftTop0, posX, posY);
                        break;

                    case ShieldCategory.Type.LeftBottom:
                        pShield = new ShieldBrick(gameName, GameSprite.Name.Brick_LeftBottom, posX, posY);
                        break;

                    case ShieldCategory.Type.RightTop1:
                        pShield = new ShieldBrick(gameName, GameSprite.Name.Brick_RightTop1, posX, posY);
                        break;

                    case ShieldCategory.Type.RightTop0:
                        pShield = new ShieldBrick(gameName, GameSprite.Name.Brick_RightTop0, posX, posY);
                        break;

                    case ShieldCategory.Type.RightBottom:
                        pShield = new ShieldBrick(gameName, GameSprite.Name.Brick_RightBottom, posX, posY);
                        break;

                    case ShieldCategory.Type.Root:
                        pShield = new ShieldRoot(gameName, GameSprite.Name.NullObject, posX, posY);
                        Debug.Assert(false);
                        break;

                    case ShieldCategory.Type.Grid:
                        pShield = new ShieldGrid(gameName, GameSprite.Name.NullObject, posX, posY);
                        break;

                    case ShieldCategory.Type.Column:
                        pShield = new ShieldColumn(gameName, GameSprite.Name.NullObject, posX, posY);
                        break;

                    default:
                        // something is wrong
                        Debug.Assert(false);
                        break;
                }
            }
            else
            {
                pShield.x = posX;
                pShield.y = posY;
            }


            // add to the tree
            Debug.Assert(pTree != null);
            this.pTree.Add(pShield);

            // Attached to Group
            pShield.ActivateGameSprite(this.pSpriteBatch);
            pShield.ActivateCollisionSprite(this.pCollisionSpriteBatch);

            return pShield;
        }

        public ShieldRoot CreateShieldRoot()
        {
            pTree = new ShieldRoot(GameObject.Name.ShieldRoot, GameSprite.Name.NullObject, 0.0f, 0.0f);
            pTree.ActivateGameSprite(this.pSpriteBatch);
            pTree.ActivateCollisionSprite(this.pCollisionSpriteBatch);
            return (ShieldRoot)this.pTree;
        }

        public GameObject CreateEntireShield(GameObject pShieldRoot, GameObject.Name gameName, float posX = 0.0f, float posY = 0.0f)
        {
            this.SetParent(pShieldRoot);
            GameObject pShieldGrid = this.Create(ShieldCategory.Type.Grid, GameObject.Name.ShieldGrid);

            // load by column
            {
                int j = 0;

                GameObject pColumn;

                this.SetParent(pShieldGrid);
                pColumn = Create(ShieldCategory.Type.Column, GameObject.Name.ShieldColumn_0 + j++);

                this.SetParent(pColumn);

                float start_x = posX;
                float start_y = posY;
                float off_x = 0;
                float brickWidth = ShieldFactory.BRICK_WIDTH;
                float brickHeight = ShieldFactory.BRICK_HEIGHT;

                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y + brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y + 2 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y + 3 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y + 4 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y + 5 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y + 6 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x, start_y + 7 * brickHeight);
                this.Create(ShieldCategory.Type.LeftTop1, GameObject.Name.Brick_LeftTop1, start_x, start_y + 8 * brickHeight);
                this.Create(ShieldCategory.Type.LeftTop0, GameObject.Name.Brick_LeftTop0, start_x, start_y + 9 * brickHeight);

                this.SetParent(pShieldGrid);
                pColumn = this.Create(ShieldCategory.Type.Column, GameObject.Name.ShieldColumn_0 + j++);

                this.SetParent(pColumn);

                off_x += brickWidth;
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 2 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 3 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 4 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 5 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 6 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 7 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 8 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 9 * brickHeight);

                this.SetParent(pShieldGrid);
                pColumn = this.Create(ShieldCategory.Type.Column, GameObject.Name.ShieldColumn_0 + j++);

                this.SetParent(pColumn);

                off_x += brickWidth;
                this.Create(ShieldCategory.Type.LeftBottom, GameObject.Name.Brick_LeftBottom, start_x + off_x, start_y + 2 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 3 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 4 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 5 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 6 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 7 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 8 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 9 * brickHeight);

                this.SetParent(pShieldGrid);
                pColumn = this.Create(ShieldCategory.Type.Column, GameObject.Name.ShieldColumn_0 + j++);

                this.SetParent(pColumn);

                off_x += brickWidth;
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 3 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 4 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 5 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 6 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 7 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 8 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 9 * brickHeight);

                this.SetParent(pShieldGrid);
                pColumn = this.Create(ShieldCategory.Type.Column, GameObject.Name.ShieldColumn_0 + j++);

                this.SetParent(pColumn);

                off_x += brickWidth;
                this.Create(ShieldCategory.Type.RightBottom, GameObject.Name.Brick_RightBottom, start_x + off_x, start_y + 2 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 3 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 4 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 5 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 6 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 7 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 8 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 9 * brickHeight);

                this.SetParent(pShieldGrid);
                pColumn = this.Create(ShieldCategory.Type.Column, GameObject.Name.ShieldColumn_0 + j++);

                this.SetParent(pColumn);

                off_x += brickWidth;
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 0 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 1 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 2 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 3 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 4 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 5 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 6 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 7 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 8 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 9 * brickHeight);

                this.SetParent(pShieldGrid);
                pColumn = this.Create(ShieldCategory.Type.Column, GameObject.Name.ShieldColumn_0 + j++);

                this.SetParent(pColumn);

                off_x += brickWidth;
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 0 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 1 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 2 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 3 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 4 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 5 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 6 * brickHeight);
                this.Create(ShieldCategory.Type.Brick, GameObject.Name.Brick, start_x + off_x, start_y + 7 * brickHeight);
                this.Create(ShieldCategory.Type.RightTop1, GameObject.Name.Brick_RightTop1, start_x + off_x, start_y + 8 * brickHeight);
                this.Create(ShieldCategory.Type.RightTop0, GameObject.Name.Brick_RightTop0, start_x + off_x, start_y + 9 * brickHeight);

            }

            return pShieldGrid;
        }

        // Data: ---------------------
        private SpriteBatch pSpriteBatch;
        private readonly SpriteBatch pCollisionSpriteBatch;
        private Composite pTree;

        public static float BRICK_WIDTH = 12.0f;
        public static float BRICK_HEIGHT = 6.0f;
    }
}

// End of File
