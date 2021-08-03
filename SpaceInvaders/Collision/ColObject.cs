using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ColObject
    {
        public ColObject(ProxySprite pProxySprite)
        {
            Debug.Assert(pProxySprite != null);

            // Create Collision Rect
            // Use the reference sprite to set size and shape
            // need to refactor if you want it different
            GameSprite pSprite = pProxySprite.pSprite;
            Debug.Assert(pSprite != null);

            // Origin is in the UPPER RIGHT 
            this.poColRect = new ColRect(pSprite.GetScreenRect());
            Debug.Assert(this.poColRect != null);

            // Create the sprite
            this.pColSprite = BoxSpriteMan.Add(BoxSprite.Name.Box, this.poColRect.x, this.poColRect.y, this.poColRect.width, this.poColRect.height);
            Debug.Assert(this.pColSprite != null);
            this.pColSprite.SetLineColor(1.0f, 1.0f, 0.0f);
        }

        public void UpdatePos(float x, float y)
        {
            this.poColRect.x = x;
            this.poColRect.y = y;

            this.pColSprite.x = this.poColRect.x;
            this.pColSprite.y = this.poColRect.y;

            this.pColSprite.SetScreenRect(this.poColRect.x, this.poColRect.y, this.poColRect.width, this.poColRect.height);
            this.pColSprite.Update();
        }

        // -------------------------------------------------------
        // Data:
        // -------------------------------------------------------
        public BoxSprite pColSprite;
        public ColRect poColRect;
    }
}