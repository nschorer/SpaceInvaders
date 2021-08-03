using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class EnemyAnimation : AnimationSprite
    {
        public EnemyAnimation(GameSprite.Name spriteName, EnemyGrid pGrid)
            :base(spriteName)
        {
            this.pGrid = pGrid;
        }

        public override void Execute(float deltaTime)
        {
            ImageHolder pImageHolder = (ImageHolder)this.pCurrImage.pSNext;

            // if at end of list, set to first
            if (pImageHolder == null)
            {
                pImageHolder = (ImageHolder)poFirstImage;
            }

            // squirrel away for next timer event
            this.pCurrImage = pImageHolder;

            // change image
            this.pSprite.SwapImage(pImageHolder.pImage);

            // Add itself back to timer
            //TimerMan.Add(TimeEvent.Name.SpriteAnimation, this, pGrid.GetGridSpeed());
        }

        EnemyGrid pGrid;
    }
}
