using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class BombSpawnEvent : Command
    {
        public BombSpawnEvent(Random pRandom)
        {
            this.pBombRoot = GameObjectMan.Find(GameObject.Name.BombRoot);
            Debug.Assert(this.pBombRoot != null);

            this.pSB_Birds = SpriteBatchMan.Find(SpriteBatch.Name.Enemies);
            Debug.Assert(this.pSB_Birds != null);

            this.pSB_Boxes = SpriteBatchMan.Find(SpriteBatch.Name.Boxes);
            Debug.Assert(this.pSB_Boxes != null);

            this.pRandom = pRandom;
        }

        override public void Execute(float deltaTime)
        {
            //Debug.WriteLine("event: {0}", deltaTime);

            // Create Bomb
            float value = pRandom.Next(100, 500);
            Bomb pBomb = Bomb.CreateBomb(GameObject.Name.BombStraight, value, 350.0f);
            //Bomb pBomb = new Bomb(GameObject.Name.Bomb, GameSprite.Name.BombStraight, new FallStraight(), value, 350.0f);
            //     Debug.WriteLine("----x:{0}", value);

            pBomb.ActivateCollisionSprite(this.pSB_Boxes);
            pBomb.ActivateGameSprite(this.pSB_Birds);

            // Attach the missile to the Bomb root
            GameObject pBombRoot = GameObjectMan.Find(GameObject.Name.BombRoot);
            Debug.Assert(pBombRoot != null);

            // Add to GameObject Tree - {update and collisions}
            pBombRoot.Add(pBomb);

        }

        GameObject pBombRoot;
        SpriteBatch pSB_Birds;
        SpriteBatch pSB_Boxes;
        Random pRandom;
    }
}

// End of File
