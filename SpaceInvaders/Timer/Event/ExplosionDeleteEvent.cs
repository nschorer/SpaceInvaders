using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ExplosionDeleteEvent : Command
    {
        public ExplosionDeleteEvent(ProxySprite pSprite)
        {
            this.pSprite = pSprite;
        }

        public override void Execute(float deltaTime)
        {
            ProxySpriteMan.Remove(pSprite);
            SpriteBatchMan.Remove(pSprite.GetSpriteNode());
        }

        ProxySprite pSprite;
    }
}
