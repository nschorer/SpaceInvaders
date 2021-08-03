using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class LivesMan
    {
        public LivesMan(int numLives = 3)
        {
            this.numLives = numLives;
            LivesText = FontMan.Find(Font.Name.Lives);
            SetLives();
        }

        public int GetLives()
        {
            return numLives;
        }

        private void SetLives()
        {
            LivesText.UpdateMessage(numLives.ToString());

            if (numLives > 2)
            {
                if (pShip1 == null)
                {
                    AddShipUI(1);
                }
                if (pShip2 == null)
                {
                    AddShipUI(2);
                }
            }else if (numLives == 2)
            {
                if (pShip1 == null)
                {
                    AddShipUI(1);
                }
                if (pShip2 != null)
                {
                    RemoveShipUI(2);
                }
            }
            else
            {
                if (pShip1 != null)
                {
                    RemoveShipUI(1);
                }
                if (pShip2 != null)
                {
                    RemoveShipUI(2);
                }
            }
        }

        private void AddShipUI(int shipNumber)
        {
            ProxySprite pShip = ProxySpriteMan.Add(GameSprite.Name.Ship);
            SpriteBatchMan.Find(SpriteBatch.Name.UI).Attach(pShip);

            if (shipNumber == 1)
            {
                pShip1 = pShip;
                pShip1.x = 50;
                pShip1.y = 20;
            }
            else if (shipNumber == 2)
            {
                pShip2 = pShip;
                pShip2.x = 100;
                pShip2.y = 20;
            }
            else
            {
                Debug.Assert(false);
            }
        }

        private void RemoveShipUI(int shipNumber)
        {
            ProxySprite pShip = null;

            if (shipNumber == 1)
            {
                pShip = pShip1;
                pShip1 = null;
            }
            else if (shipNumber == 2)
            {
                pShip = pShip2;
                pShip2 = null;
            }

            Debug.Assert(pShip != null);

            ProxySpriteMan.Remove(pShip);
            SpriteBatchMan.Remove(pShip.GetSpriteNode());
        }

        public void LoseLife()
        {
            numLives--;
            SetLives();
        }

        public void ResetLives()
        {
            numLives = 3;
            SetLives();
        }

        Font LivesText;
        int numLives;
        ProxySprite pShip1;
        ProxySprite pShip2;
    }
}
