
using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class EnemyFactory
    {
        public EnemyFactory(SpriteBatch.Name spriteBatchName)
        {
            this.pSpriteBatch = SpriteBatchMan.Find(spriteBatchName);
            Debug.Assert(this.pSpriteBatch != null);
        }

        ~EnemyFactory()
        {
            //Debug.WriteLine("~EnemyFactor():");
            this.pSpriteBatch = null;
        }

        public GameObject Create(GameObject.Name name, EnemyCategory.Type type, float posX = 0.0f, float posY = 0.0f)
        {
            GameObject pGameObj = GameObjectMan.ReuseGameObject(name);

            if (pGameObj == null)
            {

                switch (type)
                {
                    case EnemyCategory.Type.Octopus:
                        pGameObj = new Octopus(name, GameSprite.Name.OctopusA, posX, posY);
                        break;

                    case EnemyCategory.Type.Squid:
                        pGameObj = new Squid(name, GameSprite.Name.SquidA, posX, posY);
                        break;

                    case EnemyCategory.Type.Crab:
                        pGameObj = new Crab(name, GameSprite.Name.CrabA, posX, posY);
                        break;

                    case EnemyCategory.Type.Column:
                        pGameObj = new EnemyColumn(name, GameSprite.Name.NullObject, 0.0f, 0.0f);
                        break;

                    case EnemyCategory.Type.Grid:
                        pGameObj = new EnemyGrid(name, GameSprite.Name.NullObject, 0.0f, 0.0f);
                        break;

                    default:
                        // something is wrong
                        Debug.Assert(false);
                        break;
                }

            }
            else
            {
                pGameObj.x = posX;
                pGameObj.y = posY;
            }

            // add it to the gameObjectManager
            Debug.Assert(pGameObj != null);
            //GameObjectMan.Attach(pGameObj);

            // Attached to Group
            pGameObj.ActivateGameSprite(this.pSpriteBatch);
            pGameObj.ActivateCollisionSprite(SpriteBatchMan.GetCollisionSB());

            return pGameObj;
        }

        // Data: ---------------------

        SpriteBatch pSpriteBatch;
    }
}
