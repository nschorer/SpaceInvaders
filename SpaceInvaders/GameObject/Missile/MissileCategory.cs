using System.Diagnostics;

namespace SpaceInvaders
{
    abstract public class MissileCategory : Leaf
    {
        public MissileCategory(GameObject.Name name, GameSprite.Name spriteName)
            : base(name, spriteName)
        {

        }
    }
}
