using System.Diagnostics;

namespace SpaceInvaders
{
    abstract public class WeaponCategory : Leaf
    {
        public WeaponCategory(GameObject.Name name, GameSprite.Name spriteName)
            : base(name, spriteName)
        {

        }
    }
}
