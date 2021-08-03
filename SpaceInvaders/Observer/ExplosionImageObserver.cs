using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class ExplosionImageObserver : ColObserver
    {
        public ExplosionImageObserver(GameSprite.Name exploSprite, float length, bool useLHS = false)
        {
            this.pExplo = GameSpriteMan.Find(exploSprite);
            this.exploLength = length;
            this.useLHS = useLHS;
        }

        public override void Notify()
        {
            GameObject pObj = useLHS ? this.pSubject.pObjA : this.pSubject.pObjB;

            ProxySprite pSprite = ProxySpriteMan.Add(pExplo.GetName());
            pSprite.x = pObj.x;
            pSprite.y = pObj.y;
            SpriteBatchMan.Find(SpriteBatch.Name.Explosions).Attach(pSprite);

            TimerMan.Add(TimeEvent.Name.SpriteAnimation, new ExplosionDeleteEvent(pSprite), exploLength);
        }

        GameSprite pExplo;
        float exploLength;
        bool useLHS;
    }
}
